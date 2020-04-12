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
using Orc.Infrastructure.Components;
using Orc.Domain.Interfaces;
using Orc.Domain.RobotInstructions;
using Orc.Domain.RobotRequests;
using Orc.Domain.RobotResponses;
using Orc.Common.Types;
using Autofac;

namespace OrcProto.IntegrationTests
{
	[TestFixture]
	public class RobotIT
	{
		private IProcessor _processor;

		[SetUp]
		public void Setup()
		{
			
		}


		[Test]
		public void Ctor_WhenProcessorNull_ShouldThrow()
		{
			// Arrange
			IRobot robot = null;
			IProcessor _processor = null;			

			// Act
			Action act = () =>
			{
				robot = new Robot(_processor);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message.Should().Contain("processor");
		}

		[Test]
		public void Ctor_WhenArgumentsNotNull_ShouldInstanciateAndNotThrow()
		{
			// Arrange
			IRobot robot = null;
			IProcessor _processor = A.Fake<IProcessor>();

			// Act
			Action act = () =>
			{
				robot = new Robot(_processor);
			};

			// Assert
			act.Should().NotThrow();
			robot.Should().NotBeNull();
		}

		[Test]
		public void DIContainer_WhenAllDependenciesRegistered_ShouldResolveIRobot()
		{
			// Arrange
			IRobot robot = null;

			var container = ContainerConfig.GetContainer();
			var serviceProvider = new AutofacServiceProvider(container);

			// Act
			Action act = () =>
			{
				robot = serviceProvider.GetService<IRobot>();
			};

			// Assert
			act.Should().NotThrow();
			robot.Should().NotBeNull();
		}


		[Test]
		public void ProcessRequestAsync_WhenJobReportRequestGiven_ShouldReturnJobReportResponse()
		{
			// Arrange
			JobReportResponse response = null;
			
			var request = new JobReportRequest(Guid.NewGuid());

			var container = ContainerConfig.GetContainer();
			var serviceProvider = new AutofacServiceProvider(container);

			IRobot robot = serviceProvider.GetService<IRobot>();

			// Act
			Func<Task> act = async () =>
			{
				response = await robot.ProcessRequestAsync(request);
			};

			// Assert
			act.Should().NotThrow();
			response.Should().NotBeNull();
		}

		[Test]
		public void RunInstructionAsync_WhenStartPositionInstuctionGiven_ShouldUpdateCurrentPosition()
		{
			// Arrange
			var coord = new Vector2d(11, 36);
			bool shouldClean = true;

			var instuction = new StartPositionInstuction(coord, shouldClean);

			IRobotStore fakeStore = A.Fake<IRobotStore>();
			var getCurrentPositionMethod = A.CallTo(() => fakeStore.GetCurrentPositionAsync());
			getCurrentPositionMethod.Returns(new Vector2d(4,5));
			var updateMethod = A.CallTo(() => fakeStore.UpdateCurrentPositionAsync(A<Vector2d>.That.IsEqualTo(coord)));

			var container = ContainerConfig.GetContainer(false, (builder) => 
			{
				builder.RegisterInstance(fakeStore).As<IRobotStore>().SingleInstance();
			});

			var serviceProvider = new AutofacServiceProvider(container);

			//IRobotStore store = serviceProvider.GetService<IRobotStore>();

			IRobot robot = serviceProvider.GetService<IRobot>();

			// Act
			Func<Task> act = async () =>
			{
				await robot.RunInstructionAsync(instuction);
			};

			// Assert
			act.Should().NotThrow();
			getCurrentPositionMethod.MustHaveHappened();
			updateMethod.MustHaveHappened();
		}
	}
}
