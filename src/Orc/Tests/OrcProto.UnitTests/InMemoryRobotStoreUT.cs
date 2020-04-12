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
	public class InMemoryRobotStoreUT
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void AddCleanedPointAsync_ShouldStoreDifferentPoints()
		{
			// Arrange
			var store = new InMemoryRobotStore();

			Vector2d[] points = new Vector2d[]
			{
				new Vector2d(1, 10),
				new Vector2d(2, 13),
				new Vector2d(1, 13),
				new Vector2d(-1, -10),
			};

			int? count = null;

			// Act
			Func<Task> act = async () =>
			{
				foreach (var p in points)
					await store.AddCleanedPointAsync(p);

				count = await store.GetCleanedPointsCountAsync();
			};

			// Assert
			act.Should().NotThrow();
			count.Should().NotBeNull()
				.And.Be(points.Length);
		}

		[Test]
		public void AddCleanedPointAsync_ShouldNotStoreDuplicatePoints()
		{
			// Arrange
			var store = new InMemoryRobotStore();

			Vector2d[] points = new Vector2d[]
			{
				new Vector2d(-1, 2),
				new Vector2d(2, -1)
			};

			int? count = null;

			// Act
			Func<Task> act = async () =>
			{
				// first add
				foreach (var p in points)
					await store.AddCleanedPointAsync(p);
				// second add
				foreach (var p in points)
					await store.AddCleanedPointAsync(p);

				count = await store.GetCleanedPointsCountAsync();
			};

			// Assert
			act.Should().NotThrow();
			count.Should().NotBeNull()
				.And.Be(points.Length);
		}

		[Test]
		public void UpdateCurrentPositionAsync_ShouldStoreAbsolutePossition()
		{
			// Arrange
			Vector2d @default = new Vector2d(33, 44);
			Vector2d @new = new Vector2d(7, -576);

			var store = new InMemoryRobotStore(@default);
			Vector2d? current = null;
						

			// Act
			Func<Task> act = async () =>
			{
				await store.UpdateCurrentPositionAsync(@new);
				current = await store.GetCurrentPositionAsync();
			};

			// Assert
			act.Should().NotThrow();
			current.Should().NotBeNull();
			current.Value.Should().NotBe(@default);
			current.Value.Should().Be(@new);

		}
	}
}
