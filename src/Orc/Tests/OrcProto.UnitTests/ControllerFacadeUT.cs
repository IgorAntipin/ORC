using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Orc.Common.Types;
using Orc.Infrastructure.Interfaces;
using Orc.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrcProto.UnitTests
{
	[TestFixture]
	public class ControllerFacadeUT
	{
		private IRobotStore _store;
		private IPathValidationService _validator;
		private IControllerFacade _facade;

		[SetUp]
		public void Setup()
		{
			_store = A.Fake<IRobotStore>();
			_validator = A.Fake<IPathValidationService>();

			_facade = new ControllerFacade(_store, _validator);
		}

		[Test]
		public void GetCurrentPositionAsync_ShouldGetAndReturnCurrentPositionFromStore()
		{
			// Arrange
			Vector2d currentPosition = new Vector2d(3, 7);

			var getCurrentPositionMethod = A.CallTo(() => _store.GetCurrentPositionAsync());
			getCurrentPositionMethod.Returns(currentPosition);

			Vector2d? captured = null;

			// Act
			Func<Task> act = async () =>
			{
				captured = await _facade.GetCurrentPositionAsync();
			};

			// Assert
			act.Should().NotThrow();
			getCurrentPositionMethod.MustHaveHappenedOnceExactly();
			captured.Should().NotBeNull().And.Be(currentPosition);
		}


		[Test]
		public void CleanAsync_ShouldCleanAndAddPointAtCurrentPosition()
		{
			// Arrange
			Vector2d currentPosition = new Vector2d(3, 7);

			var getCurrentPositionMethod = A.CallTo(() => _store.GetCurrentPositionAsync());
			getCurrentPositionMethod.Returns(currentPosition);
			var addCleanedPointMethod = A.CallTo(() => _store.AddCleanedPointAsync(A<Vector2d>.That.IsEqualTo(currentPosition)));

			// Act
			Func<Task> act = async () =>
			{
				await _facade.CleanAsync();
			};


			// Assert
			act.Should().NotThrow();
			getCurrentPositionMethod.MustHaveHappenedOnceExactly();
			addCleanedPointMethod.MustHaveHappenedOnceExactly();
		}


		[Test]
		public void MoveToAsync_WhenPathNotValid_ShouldThrow()
		{
			// Arrange
			var checkMoveMethod = A.CallTo(() => _validator.CheckMoveAsync(A<Vector2d>._, A<Vector2d>._));
			checkMoveMethod.Returns(false);

			var relative = new Vector2d(1, 3);


			// Act
			Func<Task> act = async () =>
			{
				await _facade.MoveToAsync(relative);
			};


			// Assert
			act.Should().Throw<InvalidOperationException>()
				.And.Message
				.Should().Contain("Failed to find path");

			checkMoveMethod.MustHaveHappened();
		}

		[Test]
		public void MoveToAsync_WhenPathValid_ShouldNotThrow()
		{
			// Arrange
			var checkMoveMethod = A.CallTo(() => _validator.CheckMoveAsync(A<Vector2d>._, A<Vector2d>._));
			checkMoveMethod.Returns(true);

			var relative = new Vector2d(1, 3);


			// Act
			Func<Task> act = async () =>
			{
				await _facade.MoveToAsync(relative);
			};


			// Assert
			act.Should().NotThrow();
			checkMoveMethod.MustHaveHappened();
		}


		public class RelativePositionTestCase
		{
			public Vector2d CurrentPosition { get; set; }
			public Vector2d Relative { get; set; }
			public Vector2d ExpectedResult { get; set; }

			public RelativePositionTestCase(Vector2d cur, Vector2d relative, Vector2d result)
			{
				CurrentPosition = cur;
				Relative = relative;
				ExpectedResult = result;
			}
		}

		private static IEnumerable<RelativePositionTestCase> RelativePositionTestCases()
		{
			yield return  new RelativePositionTestCase(new Vector2d(0,0), new Vector2d(1,5), new Vector2d(1,5));
			yield return new RelativePositionTestCase(new Vector2d(17, 4), new Vector2d(-3, 5), new Vector2d(14, 9));
			yield return new RelativePositionTestCase(new Vector2d(6, 3), new Vector2d(0, -1), new Vector2d(6, 2));
		}

		[Test, TestCaseSource(nameof(RelativePositionTestCases))]
		public void MoveToAsync_WhenPathValid_ShouldMoveToRelativePosition(RelativePositionTestCase testCase)
		{
			// Arrange
			var checkMoveMethod = A.CallTo(() => _validator.CheckMoveAsync(A<Vector2d>._, A<Vector2d>._));
			checkMoveMethod.Returns(true);

			var getCurrentPositionMethod = A.CallTo(() => _store.GetCurrentPositionAsync());
			getCurrentPositionMethod.Returns(testCase.CurrentPosition);
			var updateCurrentPositionMethod = A.CallTo(() => _store.UpdateCurrentPositionAsync(A<Vector2d>.That.IsEqualTo(testCase.ExpectedResult)));


			// Act
			Func<Task> act = async () =>
			{
				await _facade.MoveToAsync(testCase.Relative);
			};


			// Assert
			act.Should().NotThrow();
			getCurrentPositionMethod.MustHaveHappenedOnceExactly();
			updateCurrentPositionMethod.MustHaveHappenedOnceExactly();
		}

		[Test]
		public void MoveToAsync_WhenRelativeZero_ShouldNotMove()
		{
			// Arrange
			var relative = new Vector2d(0, 0);

			var checkMoveMethod = A.CallTo(() => _validator.CheckMoveAsync(A<Vector2d>._, A<Vector2d>._));
			var getCurrentPositionMethod = A.CallTo(() => _store.GetCurrentPositionAsync());
			var updateCurrentPositionMethod = A.CallTo(() => _store.UpdateCurrentPositionAsync(A<Vector2d>._));


			// Act
			Func<Task> act = async () =>
			{
				await _facade.MoveToAsync(relative);
			};


			// Assert
			act.Should().NotThrow();
			getCurrentPositionMethod.MustNotHaveHappened();
			checkMoveMethod.MustNotHaveHappened();
			updateCurrentPositionMethod.MustNotHaveHappened();
		}

	}
}
