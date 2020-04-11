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
		public void Ctor_WhenArgumentsNoNull_ShouldNotThrow()
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


	}
}
