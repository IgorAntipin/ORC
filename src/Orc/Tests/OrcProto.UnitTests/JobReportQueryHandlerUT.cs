using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Orc.Domain.Queries;
using Orc.Domain.RobotResponses;
using Orc.Infrastructure.Interfaces;
using Orc.Infrastructure.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrcProto.UnitTests
{
	[TestFixture]
	public class JobReportQueryHandlerUT
	{

        [Test]
        public void Ctor_WhenRobotStoreNull_ShouldThrow()
        {
            // Arrange
            IRobotStore robotStore = null;

            // Act
            Action act = () =>
            {
                var handler = new JobReportQueryHandler(robotStore);
            };

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .And.Message.Should().Contain("robotStore");
        }

        [Test]
        public void Ctor_WhenArgumentsNotNull_ShouldNotThrow()
        {
            // Arrange
            IRobotStore robotStore = A.Fake<IRobotStore>();
            JobReportQueryHandler handler = null;

            // Act
            Action act = () =>
            {
                handler = new JobReportQueryHandler(robotStore);
            };

            // Assert
            act.Should().NotThrow();
            handler.Should().NotBeNull();
        }

        [Test]
        public void HandleQueryAsync_ShouldCreateJobReportResponse()
        {
            // Arrange
            JobReportResponse response = null;
            var id = Guid.NewGuid();
            JobReportQuery query = new JobReportQuery(id);

            IRobotStore fakeStore = A.Fake<IRobotStore>();
            JobReportQueryHandler handler = new JobReportQueryHandler(fakeStore);

            // Act
            Func<Task> act = async () =>
            {
                response = await handler.HandleQueryAsync(query);
            };

            // Assert
            act.Should().NotThrow();
            response.Should().NotBeNull();
        }

        [Test]
        public void HandleQueryAsync_ShouldReturnCountFromRobotStore()
        {
            // Arrange
            int count = 10;
            JobReportResponse response = null;
            var id = Guid.NewGuid();            
            JobReportQuery query = new JobReportQuery(id);

            IRobotStore fakeStore = A.Fake<IRobotStore>();
            var getCountMethod = A.CallTo(() => fakeStore.GetCleanedPointsCountAsync());
            getCountMethod.Returns(count);

            JobReportQueryHandler handler = new JobReportQueryHandler(fakeStore);

            // Act
            Func<Task> act = async () =>
            {
                response = await handler.HandleQueryAsync(query);
            };

            // Assert
            act.Should().NotThrow();
            response.Should().NotBeNull();
            response.UniqueCleanedVerticesCount.Should().Be(count);
            getCountMethod.MustHaveHappened();
        }
    }
}
