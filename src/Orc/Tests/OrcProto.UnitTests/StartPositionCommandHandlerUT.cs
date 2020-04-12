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
	public class StartPositionCommandHandlerUT
	{

        [Test]
        public void Ctor_WhenControllerFacadeNull_ShouldThrow()
        {
            // Arrange
            IControllerFacade facade = null;

            // Act
            Action act = () =>
            {
                var hander = new StartPositionCommandHandler(facade);
            };

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .And.Message.Should().Contain("controllerFacade");
        }

        [Test]
        public void Ctor_WhenArgumentsNotNull_ShouldNotThrow()
        {
            // Arrange
            IControllerFacade facade = A.Fake<IControllerFacade>();
            StartPositionCommandHandler hander = null;

            // Act
            Action act = () =>
            {
                hander = new StartPositionCommandHandler(facade);
            };

            // Assert
            act.Should().NotThrow();
            hander.Should().NotBeNull();
        }

        [Test]
        public void HandleAsync_WhenCleanTrue_ShouldCallClean()
        {
            // Arrange
            bool shouldClean = true;
            Vector2d absolutePosition = new Vector2d(1000, -513);
            StartPositionCommand command = new StartPositionCommand(absolutePosition, shouldClean);

            IControllerFacade facade = A.Fake<IControllerFacade>();
            var cleanMethod = A.CallTo(() => facade.CleanAsync());
            StartPositionCommandHandler hander = new StartPositionCommandHandler(facade);

            // Act
            Func<Task> act = async () =>
            {
                await hander.HandleAsync(command);
            };

            // Assert
            act.Should().NotThrow();
            cleanMethod.MustHaveHappenedOnceExactly();
        }

        [Test]
        public void HandleAsync_WhenCleanFalse_ShouldNotCallClean()
        {
            // Arrange
            bool shouldClean = false;
            Vector2d absolutePosition = new Vector2d(1000, -513);
            StartPositionCommand command = new StartPositionCommand(absolutePosition, shouldClean);

            IControllerFacade facade = A.Fake<IControllerFacade>();
            var cleanMethod = A.CallTo(() => facade.CleanAsync());
            StartPositionCommandHandler hander = new StartPositionCommandHandler(facade);

            // Act
            Func<Task> act = async () =>
            {
                await hander.HandleAsync(command);
            };

            // Assert
            act.Should().NotThrow();
            cleanMethod.MustNotHaveHappened();
        }

        [Test]
        public void HandleAsync_ShouldCallGetCurrentPosition()
        {
            // Arrange
            bool shouldClean = true;
            Vector2d absolutePosition = new Vector2d(1, 3);
            StartPositionCommand command = new StartPositionCommand(absolutePosition, shouldClean);

            IControllerFacade facade = A.Fake<IControllerFacade>();
            var getcurrentPositionMethod = A.CallTo(() => facade.GetCurrentPositionAsync());

            StartPositionCommandHandler hander = new StartPositionCommandHandler(facade);

            // Act
            Func<Task> act = async () =>
            {
                await hander.HandleAsync(command);
            };

            // Assert
            act.Should().NotThrow();
            getcurrentPositionMethod.MustHaveHappened();
        }

        [Test]
        public void HandleAsync_WhenNotInCurrentPosition_ShouldCallMoveToRelativePosition()
        {
            // Arrange
            bool shouldClean = true;
            Vector2d current = new Vector2d(10, 5);
            Vector2d dest = new Vector2d(1, 3);
            Vector2d expectedOffset = dest - current;
            StartPositionCommand command = new StartPositionCommand(dest, shouldClean);

            IControllerFacade facade = A.Fake<IControllerFacade>();
            var getcurrentPositionMethod = A.CallTo(() => facade.GetCurrentPositionAsync());
            getcurrentPositionMethod.Returns(current);

            var moveToMethod = A.CallTo(() => facade.MoveToAsync(A<Vector2d>.That.IsEqualTo(expectedOffset)));

            StartPositionCommandHandler hander = new StartPositionCommandHandler(facade);

            // Act
            Func<Task> act = async () =>
            {
                await hander.HandleAsync(command);
            };

            // Assert
            act.Should().NotThrow();
            getcurrentPositionMethod.MustHaveHappened();
            moveToMethod.MustHaveHappened();
        }

        [Test]
        public void HandleAsync_WhenAlreadyInCurrentPosition_ShouldNotCallMoveToRelativePosition()
        {
            // Arrange
            bool shouldClean = true;
            Vector2d current = new Vector2d(33, -9);
            Vector2d dest = new Vector2d(33, -9);
            Vector2d expectedOffset = dest - current;
            StartPositionCommand command = new StartPositionCommand(dest, shouldClean);

            IControllerFacade facade = A.Fake<IControllerFacade>();
            var getcurrentPositionMethod = A.CallTo(() => facade.GetCurrentPositionAsync());
            getcurrentPositionMethod.Returns(current);

            var moveToMethod = A.CallTo(() => facade.MoveToAsync(A<Vector2d>.That.IsEqualTo(expectedOffset)));

            StartPositionCommandHandler hander = new StartPositionCommandHandler(facade);

            // Act
            Func<Task> act = async () =>
            {
                await hander.HandleAsync(command);
            };

            // Assert
            act.Should().NotThrow();
            expectedOffset.Should().Be(Vector2d.ZERO);
            getcurrentPositionMethod.MustHaveHappened();
            moveToMethod.MustNotHaveHappened();
        }

    }
}
