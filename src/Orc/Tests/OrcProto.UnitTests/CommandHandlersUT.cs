using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Orc.Common.Types;
using Orc.Domain.Commands;
using Orc.Infrastructure.CommandHandlers;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrcProto.UnitTests
{
    [TestFixture]
    public class CommandHandlersUT
    {

        [Test]
        public void Ctor_MoveAndCleanCommandHandler_WhenControllerFacadeNull_ShouldThrow()
        {
            // Arrange
            IControllerFacade facade = null;

            // Act
            Action act = () =>
            {
                var hander = new MoveAndCleanCommandHandler(facade);
            };

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .And.Message.Should().Contain("controllerFacade");
        }

        [Test]
        public void Ctor_MoveAndCleanCommandHandler_WhenArgumentsNotNull_ShouldNotThrow()
        {
            // Arrange
            IControllerFacade facade = A.Fake<IControllerFacade>();
            MoveAndCleanCommandHandler hander = null;

            // Act
            Action act = () =>
            {
                hander = new MoveAndCleanCommandHandler(facade);
            };

            // Assert
            act.Should().NotThrow();
            hander.Should().NotBeNull();
        }

        public class DirectionTestCase
        {
            public Vector2d Direction { get; set; }
        }

        public static IEnumerable<DirectionTestCase> DirectionTestCases()
        {
            yield return new DirectionTestCase() { Direction = Vector2d.NORTH };
            yield return new DirectionTestCase() { Direction = Vector2d.SOUTH };
            yield return new DirectionTestCase() { Direction = Vector2d.EAST };
            yield return new DirectionTestCase() { Direction = Vector2d.WEST };

        }

        [Test, TestCaseSource("DirectionTestCases")]
        public void MoveAndCleanCommandHandler_ShouldCallMoveToInGivenDirrection(DirectionTestCase testCase)
        {
            // Arrange
            int steps = 3;
            MoveAndCleanCommand command = new MoveAndCleanCommand(testCase.Direction, steps);

            IControllerFacade facade = A.Fake<IControllerFacade>();
            var moveToMethod = A.CallTo(() => facade.MoveToAsync(A<Vector2d>.That.IsEqualTo(testCase.Direction)));
            MoveAndCleanCommandHandler hander = new MoveAndCleanCommandHandler(facade);

            // Act
            Func<Task> act = async () =>
            {
                await hander.HandleAsync(command);
            };

            // Assert
            act.Should().NotThrow();
            moveToMethod.MustHaveHappened(steps, Times.Exactly);
        }

        [Test]
        public void MoveAndCleanCommandHandler_ShouldCallClean()
        {
            // Arrange
            int steps = 3;
            MoveAndCleanCommand command = new MoveAndCleanCommand(Vector2d.NORTH, steps);

            IControllerFacade facade = A.Fake<IControllerFacade>();
            var cleanMethod = A.CallTo(() => facade.CleanAsync());
            MoveAndCleanCommandHandler hander = new MoveAndCleanCommandHandler(facade);

            // Act
            Func<Task> act = async () =>
            {
                await hander.HandleAsync(command);
            };

            // Assert
            act.Should().NotThrow();
            cleanMethod.MustHaveHappened(steps, Times.Exactly);
        }
    }
}
