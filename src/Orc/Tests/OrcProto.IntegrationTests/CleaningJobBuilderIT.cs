using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Orc.Domain.Interfaces;
using Orc.Domain.RobotResponses;
using Orc.Infrastructure.Components;
using Orc.Infrastructure.Interfaces;
using Serilog;

namespace OrcProto.IntegrationTests
{
	[TestFixture]
	public class CleaningJobBuilderIT
	{
		private IProcessor _processor;
		private ILogger _logger;

		[SetUp]
		public void Setup()
		{
			_processor = A.Fake<IProcessor>();
			_logger = A.Fake<ILogger>();
		}

		[Test]
		public void Ctor_WhenProcessorNull_ShouldThrow()
		{
			//Arrange
			IProcessor processor = null;
			ILogger logger = A.Fake<ILogger>();

			// Act
			Action act = () =>
			{
				var builder = new CleaningJobBuilder(processor, logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message.Contains("processor");
		}

		[Test]
		public void Ctor_WhenLoggerNull_ShouldThrow()
		{
			//Arrange
			IProcessor processor = A.Fake<IProcessor>(); ;
			ILogger logger = null;

			// Act
			Action act = () =>
			{
				var builder = new CleaningJobBuilder(processor, logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message.Contains("logger");
		}

		[Test]
		public void Ctor_WhenArgumentsNotNull_ShouldNotThrow()
		{
			//Arrange
			IRobotJobBuilder builder = null;

			// Act
			Action act = () =>
			{
				builder = new CleaningJobBuilder(_processor, _logger);
			};

			// Assert
			act.Should().NotThrow();
			builder.Should().NotBeNull();
		}

		[Test]
		public void Build_WhenTextReaderNotSet_ShouldThrow()
		{
			//Arrange
			IRobotJobBuilder builder = new CleaningJobBuilder(_processor, _logger);

			// Act
			Action act = () =>
			{
				IRobotJob job = builder.BuildAsync().GetAwaiter().GetResult();
			};

			// Assert
			act.Should().Throw<NullReferenceException>()
				.And.Message.Should().Contain($"Failed to build a job. TextReader is null."); 
		}


		public class InputTestCase
		{
			public string Input { get; set; }
			public int ExpectedCount { get; set; }
			public InputTestCase(string[] inputs, int count)
			{
				StringBuilder inputSB = new StringBuilder();
				foreach (var str in inputs)
					inputSB.AppendLine(str);
				Input = inputSB.ToString();
				ExpectedCount = count;
			}
		}

		public static  IEnumerable<InputTestCase> InputTestCases()
		{
			yield return new InputTestCase(new string[] 
			{
				"2",
				"10 22",
				"E 2",
				"N 1"
			}, 3);

			yield return new InputTestCase(new string[]
			{
				"0",
				"100000 -100000",
			}, 1);
		}

		[Test,TestCaseSource("InputTestCases")]
		public void Build_WhenTextReaderSet_ShouldBuildJobFromInput(InputTestCase testCase)
		{
			//Arrange
			IRobotJob job = null;
			TextReader textReader = new StringReader(testCase.Input);

			var container = ContainerConfig.GetContainer(true);
			var serviceProvider = new AutofacServiceProvider(container);

			IRobotJobBuilder builder = serviceProvider.GetService<IRobotJobBuilder>();
			builder.UseReader(textReader);

			// Act
			Action act = () =>
			{
				job = builder.BuildAsync().GetAwaiter().GetResult();
			};

			// Assert
			act.Should().NotThrow();
			job.Should().NotBeNull();
			job.Instructions.Should().NotBeNull();
			job.Instructions.Count.Should().Be(testCase.ExpectedCount);
		}

	}
}
