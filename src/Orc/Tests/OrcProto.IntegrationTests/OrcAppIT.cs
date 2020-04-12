using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Orc.Infrastructure.Interfaces;
using OrcProto.App;
using Serilog;
using System.IO;
using System.Threading.Tasks;
using Orc.Infrastructure.Services;
using Orc.Common.Types;
using Autofac;

namespace OrcProto.IntegrationTests
{
	[TestFixture]
	public class OrcAppIT
	{
		private IRobotJobBuilder _jobBuilder;
		private IRobot _robot;
		private IReportWriter _reportProcessor;
		private ILogger _logger;
		private TextWriter _textWriter;
		private TextReader _textReader;

		[SetUp]
		public void Setup()
		{
			_jobBuilder = A.Fake<IRobotJobBuilder>();
			_robot = A.Fake<IRobot>();
			_reportProcessor = A.Fake<IReportWriter>();
			_logger = A.Fake<ILogger>();

			_textWriter = new StringWriter();
		}

		[TearDown]
		public void Cleanup()
		{
			if (_textReader != null)
				_textReader.Dispose();

			if (_textWriter != null)
				_textWriter.Dispose();
		}

		[Test]
		public void Ctor_WhenJobBuilderNull_ShouldThrow()
		{
			// Arrange
			_jobBuilder = null;
			// Act
			Action act = () =>
			{
				var app = new OrcApp(_jobBuilder, _robot, _reportProcessor, _logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("jobBuilder");
		}

		[Test]
		public void Ctor_WhenRobotNull_ShouldThrow()
		{
			// Arrange
			_robot = null;

			// Act
			Action act = () =>
			{
				var app = new OrcApp(_jobBuilder, _robot, _reportProcessor, _logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("robot");
		}

		[Test]
		public void Ctor_WhenReportProcessorNull_ShouldThrow()
		{
			// Arrange
			_reportProcessor = null;

			// Act
			Action act = () =>
			{
				var app = new OrcApp(_jobBuilder, _robot, _reportProcessor, _logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("reportWriter");
		}

		[Test]
		public void Ctor_WhenLoggerNull_ShouldThrow()
		{
			// Arrange
			_logger = null;

			// Act
			Action act = () =>
			{
				var app = new OrcApp(_jobBuilder, _robot, _reportProcessor, _logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("logger");
		}

		[Test]
		public void Ctor_WhenArgumentsNoNull_ShouldInstanciateAndNotThrow()
		{
			// Arrange
			IOrcApp app = null;

			// Act
			Action act = () =>
			{
				app = new OrcApp(_jobBuilder, _robot, _reportProcessor, _logger);
			};

			// Assert
			act.Should().NotThrow();
			app.Should().NotBeNull();
		}

		[Test]
		public void DIContainer_WhenAllDependenciesRegistered_ShouldResolveIOrcApp()
		{
			// Arrange
			IOrcApp app = null;

			var container = ContainerConfig.GetContainer();
			var serviceProvider = new AutofacServiceProvider(container);

			// Act
			Action act = () =>
			{
				app = serviceProvider.GetService<IOrcApp>();
			};

			// Assert
			act.Should().NotThrow();
			app.Should().NotBeNull();
		}


		[Test]
		public void StartAsync_WhenReaderNull_ShouldThrow()
		{
			// Arrange
			var app = new OrcApp(_jobBuilder, _robot, _reportProcessor, _logger);

			// Act
			Func<Task> act = async () =>
			{
				await app.StartAsync(null, _textWriter);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("reader");
		}

		[Test]
		public void StartAsync_WhenWriterNull_ShouldThrow()
		{
			// Arrange
			var input = "0";
			_textReader = new StringReader(input);
			var app = new OrcApp(_jobBuilder, _robot, _reportProcessor, _logger);

			// Act
			Func<Task> act = async () =>
			{
				await app.StartAsync(_textReader, null);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("writer");
		}


		[Test]
		public void StartAsync_WhenArgumentsNoNull_ShouldNotThrow()
		{
			// Arrange
			var input = "0";
			_textReader = new StringReader(input);
			var app = new OrcApp(_jobBuilder, _robot, _reportProcessor, _logger);

			// Act
			Func<Task> act = async () =>
			{
				await app.StartAsync(_textReader, _textWriter);
			};

			// Assert
			act.Should().NotThrow();
		}

		public class InputOutputTestCase
		{
			public string Input { get; set; }
			public string ExpectedOutput { get; set; }
			public int ExpectedCount { get; set; }

			public InputOutputTestCase(string[] inputs, int count)
			{
				StringBuilder inputSB = new StringBuilder();
				foreach (var str in inputs)
					inputSB.AppendLine(str);
				Input = inputSB.ToString();
				ExpectedCount = count;
				ExpectedOutput = $"=> Cleaned: {count}\r\n";
			}
		}

		public static IEnumerable<InputOutputTestCase> InputOutputTestCases()
		{
			yield return new InputOutputTestCase(new string[] 
			{
				"2",
				"10 22",
				"E 2",
				"N 1"
			}, 4);

			yield return new InputOutputTestCase(new string[]
			{
				"0",
				"100000 -100000",
			}, 1);

			yield return new InputOutputTestCase(new string[]
			{
				"2",
				"50 -100000",
				"N 100000",
				"S 100000"
			}, 100001);
			yield return new InputOutputTestCase(new string[]
			{
				"2",
				"0 100000",
				"S 100000",
				"S 100000"
			}, 200001);
		}

		[Test, TestCaseSource("InputOutputTestCases")]
		public void StartAsync_WhenInputContainsValidInstructions_ShouldWriteCorrectOutput(InputOutputTestCase testCase)
		{
			// Arrange
			string output = null;

			StringBuilder outputSB = new StringBuilder();

			_textReader = new StringReader(testCase.Input);
			_textWriter = new StringWriter(outputSB);

			IRobotStore inMemoryStore = new InMemoryRobotStore(Vector2d.ZERO);

			var container = ContainerConfig.GetContainer(true, (builder) =>
			{
				builder.RegisterInstance(inMemoryStore).As<IRobotStore>().SingleInstance();
			});

			var serviceProvider = new AutofacServiceProvider(container);

			IOrcApp app = serviceProvider.GetService<IOrcApp>();

			// Act
			Func<Task> act = async () =>
			{
				await app.StartAsync(_textReader, _textWriter);
				
				output = outputSB.ToString();
			};			

			// Assert
			act.Should().NotThrow();
			output.Should().NotBeNull()
				.And.Be(testCase.ExpectedOutput);
		}

	}
}
