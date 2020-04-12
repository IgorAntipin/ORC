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
using Orc.Domain.Jobs;
using Orc.Infrastructure.Services;

namespace OrcProto.IntegrationTests
{
	[TestFixture]
	public class RobotIT
	{
		private IProcessor _processor;

		[SetUp]
		public void Setup()
		{
			_processor = A.Fake<IProcessor>();
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

			IRobotStore inMemoryStore = new InMemoryRobotStore();

			var container = ContainerConfig.GetContainer(false, (builder) =>
			{
				builder.RegisterInstance(inMemoryStore).As<IRobotStore>().SingleInstance();
			});
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
		public void ProcessRequestAsync_WhenRequestNull_ShouldThrow()
		{
			// Arrange
			JobReportResponse response = null;

			JobReportRequest request =null;

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				response = await robot.ProcessRequestAsync(request);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message.Should().Contain($"request");
		}

		public class QueryNullRequest : IRobotQueryRequest<JobReportResponse>
		{
			public IQuery<JobReportResponse> Query => null;

			public Guid JobId { get; }
		}

		[Test]
		public void ProcessRequestAsync_WhenRequestQueryNull_ShouldThrow()
		{
			// Arrange
			JobReportResponse response = null;

			QueryNullRequest request = new QueryNullRequest();

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				response = await robot.ProcessRequestAsync(request);
			};

			// Assert
			act.Should().Throw<NullReferenceException>()
				.And.Message.Should().Contain($"Invalid reportQuery. Query is null.");
		}

		public class UnknownRequest : IRobotRequest<JobReportResponse>
		{
		}

		[Test]
		public void ProcessRequestAsync_WhenUnknownRequest_ShouldThrow()
		{
			// Arrange
			JobReportResponse response = null;

			UnknownRequest request = new UnknownRequest();

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				response = await robot.ProcessRequestAsync(request);
			};

			// Assert
			act.Should().Throw<NotImplementedException>()
				.And.Message.Should().Contain($"{typeof(UnknownRequest)}");
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

		[Test]
		public void RunInstructionAsync_WhenInstructionNull_ShouldThrow()
		{
			// Arrange
			StartPositionInstuction instuction = null;

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				await robot.RunInstructionAsync(instuction);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message.Should().Contain($"instruction");
		}

		public class CommandNullInstruction : ICommandInstruction
		{
			public ICommand Command => null;
		}

		[Test]
		public void RunInstructionAsync_WhenInstructionCommandNull_ShouldThrow()
		{
			// Arrange
			CommandNullInstruction instuction = new CommandNullInstruction();

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				await robot.RunInstructionAsync(instuction);
			};

			// Assert
			act.Should().Throw<NullReferenceException>()
				.And.Message.Should().Contain($"{typeof(CommandNullInstruction)}");
		}

		public class UnknownInstruction : IInstruction
		{
		}

		[Test]
		public void RunInstructionAsync_WhenUnknownInstruction_ShouldThrow()
		{
			// Arrange
			UnknownInstruction instuction = new UnknownInstruction();

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				await robot.RunInstructionAsync(instuction);
			};

			// Assert
			act.Should().Throw<NotImplementedException>()
				.And.Message.Should().Contain($"{typeof(UnknownInstruction)}");
		}


		public class JobTestCase
		{
			public Vector2d DefaultPosition { get; set; }
			public IRobotJob Job { get; set; }

			public int ExpectedCount { get; set; }

			public JobTestCase(IRobotJob job, int expected, Vector2d defaultPosition)
			{
				Job = job;
				ExpectedCount = expected;
				DefaultPosition = defaultPosition;
			}
		}


		public static IEnumerable<JobTestCase> JobTestCases()
		{
			yield return new JobTestCase(new RobotJob() 
			{ 
				Id = Guid.NewGuid(),
				Instructions = new Queue<IInstruction>( new IInstruction[] 
				{ 
					new StartPositionInstuction(new Vector2d(10,22), true),
					new MoveAndCleanInstruction(Vector2d.EAST, 2),
					new MoveAndCleanInstruction(Vector2d.NORTH, 1)
				})
			}, 
			4,
			Vector2d.ZERO);
		}

		[Test, TestCaseSource("JobTestCases")]
		public void RunJobAsync_WhenJobGiven_ShouldExecuteAllInstructions(JobTestCase testCase)
		{
			// Arrange
			bool? success = null;
			IRobotJob job = testCase.Job;

			IRobotStore inMemoryStore = new InMemoryRobotStore(testCase.DefaultPosition);

			var container = ContainerConfig.GetContainer(false, (builder) =>
			{
				builder.RegisterInstance(inMemoryStore).As<IRobotStore>().SingleInstance();
			});

			var serviceProvider = new AutofacServiceProvider(container);


			IRobot robot = serviceProvider.GetService<IRobot>();

			// Act
			Func<Task> act = async () =>
			{
				success = await robot.RunJobAsync(testCase.Job);
			};

			// Assert
			act.Should().NotThrow();
			success.Should().BeTrue();
			job.Instructions.Count.Should().Be(0);
		}


		[Test, TestCaseSource("JobTestCases")]
		public async Task RunJobAsync_WhenJobExecuted_ShouldReturnJobReportResponse(JobTestCase testCase)
		{
			// Arrange
			JobReportResponse response = null;

			bool? success = null;
			IRobotJob job = testCase.Job;

			IRobotStore inMemoryStore = new InMemoryRobotStore(testCase.DefaultPosition);

			var container = ContainerConfig.GetContainer(true, (builder) =>
			{
				builder.RegisterInstance(inMemoryStore).As<IRobotStore>().SingleInstance();
			});

			var serviceProvider = new AutofacServiceProvider(container);

			IRobot robot = serviceProvider.GetService<IRobot>();

			success = await robot.RunJobAsync(testCase.Job);

			var request = new JobReportRequest(testCase.Job.Id);

			// Act
			Func<Task> act = async () =>
			{
				response = await robot.ProcessRequestAsync(request);
			};

			// Assert
			act.Should().NotThrow();
			success.Should().BeTrue();
			job.Instructions.Count.Should().Be(0);

			response.Should().NotBeNull();
			response.UniqueCleanedVerticesCount.Should().Be(testCase.ExpectedCount);
		}



		[Test]
		public void RunJobAsync_WhenJobNull_ShouldThrow()
		{
			// Arrange
			IRobotJob job = null;

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				await robot.RunJobAsync(job);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message.Should().Contain($"job");
		}

		[Test]
		public void RunJobAsync_WhenJobContainsNullInstruction_ShouldThrow()
		{
			// Arrange
			IInstruction instruction = null;
			IRobotJob job = new RobotJob() {Instructions = new Queue<IInstruction>(new IInstruction[] { instruction }) };

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				await robot.RunJobAsync(job);
			};

			// Assert
			act.Should().Throw<NullReferenceException>()
				.And.Message.Should().Contain($"Failed to complete job. Instruction is null.");
		}

		[Test]
		public void RunJobAsync_WhenJobContainsInstructionCommandNull_ShouldThrow()
		{
			// Arrange
			IInstruction instruction = new CommandNullInstruction();
			IRobotJob job = new RobotJob() { Instructions = new Queue<IInstruction>(new IInstruction[] { instruction }) };

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				await robot.RunJobAsync(job);
			};

			// Assert
			act.Should().Throw<NullReferenceException>()
				.And.Message.Should().Contain($"{typeof(CommandNullInstruction)}");
		}

		[Test]
		public void RunJobAsync_WhenJobContainsUnknownInstruction_ShouldThrow()
		{
			// Arrange
			IInstruction instruction = new UnknownInstruction();
			IRobotJob job = new RobotJob() { Instructions = new Queue<IInstruction>(new IInstruction[] { instruction }) };

			IRobot robot = new Robot(_processor);

			// Act
			Func<Task> act = async () =>
			{
				await robot.RunJobAsync(job);
			};

			// Assert
			act.Should().Throw<NotImplementedException>()
				.And.Message.Should().Contain($"{typeof(UnknownInstruction)}");
		}

	}
}
