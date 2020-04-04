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

namespace OrcProto.IntegrationTests
{
	public class OrcAppIT
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Ctor_WithNullJobBuilder_WillThrow()
		{
			// Arrange
			IRobotJobBuilder jobBuilder = null;
			IRobot robot = A.Fake<IRobot>();
			IReportProcessor reportProcessor = A.Fake<IReportProcessor>();
			ILogger logger = A.Fake<ILogger>();

			// Act
			Action act = () =>
			{
				var app = new OrcApp(jobBuilder, robot, reportProcessor, logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("jobBuilder");
		}

		[Test]
		public void Ctor_WithNullRobot_WillThrow()
		{
			// Arrange
			IRobotJobBuilder jobBuilder = A.Fake<IRobotJobBuilder>();
			IRobot robot = null;
			IReportProcessor reportProcessor = A.Fake<IReportProcessor>();
			ILogger logger = A.Fake<ILogger>();

			// Act
			Action act = () =>
			{
				var app = new OrcApp(jobBuilder, robot, reportProcessor, logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("robot");
		}

		[Test]
		public void Ctor_WithNullReportProcessor_WillThrow()
		{
			// Arrange
			IRobotJobBuilder jobBuilder = A.Fake<IRobotJobBuilder>();
			IRobot robot = A.Fake<IRobot>();
			IReportProcessor reportProcessor = null;
			ILogger logger = A.Fake<ILogger>();

			// Act
			Action act = () =>
			{
				var app = new OrcApp(jobBuilder, robot, reportProcessor, logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("reportProcessor");
		}

		[Test]
		public void Ctor_WithNullLogger_WillThrow()
		{
			// Arrange
			IRobotJobBuilder jobBuilder = A.Fake<IRobotJobBuilder>();
			IRobot robot = A.Fake<IRobot>();
			IReportProcessor reportProcessor = A.Fake<IReportProcessor>();
			ILogger logger = null;

			// Act
			Action act = () =>
			{
				var app = new OrcApp(jobBuilder, robot, reportProcessor, logger);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message
				.Should().Contain("logger");
		}

		[Test]
		public void DiContainer_HasAllDependencies_ShouldResolveIOrcApp()
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
	}
}
