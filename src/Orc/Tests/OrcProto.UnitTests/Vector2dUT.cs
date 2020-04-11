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
	public class Vector2dUT
	{
		[SetUp]
		public void Setup()
		{
		}

		public class ArithmeticTestCase
		{
			public Vector2d A { get; set; }
			public Vector2d B { get; set; }
			public Vector2d Result { get; set; }

			public ArithmeticTestCase(Vector2d a, Vector2d b, Vector2d result)
			{
				A = a;
				B = b;
				Result = result;
			}
		}

		private static IEnumerable<ArithmeticTestCase> SumTestCases()
		{
			
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.ZERO, Vector2d.ZERO);
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.NORTH, Vector2d.NORTH);
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.SOUTH, Vector2d.SOUTH);
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.WEST, Vector2d.WEST);
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.EAST, Vector2d.EAST);
			yield return new ArithmeticTestCase(Vector2d.NORTH, Vector2d.SOUTH, Vector2d.ZERO);
			yield return new ArithmeticTestCase(Vector2d.WEST, Vector2d.EAST, Vector2d.ZERO);
			yield return new ArithmeticTestCase(new Vector2d(13,6), new Vector2d(-4, 1), new Vector2d(9,7));
			yield return new ArithmeticTestCase(new Vector2d(int.MaxValue, int.MinValue), new Vector2d(1, -1), new Vector2d(int.MinValue, int.MaxValue));
			yield return new ArithmeticTestCase(new Vector2d(int.MaxValue, 0), new Vector2d(1, 0), new Vector2d(int.MinValue, 0));
		}

		[Test, TestCaseSource(nameof(SumTestCases))]
		public void Vector2d_SumTest(ArithmeticTestCase testCase)
		{
			// Arrange		
			Vector2d? sum = null;

			// Act
			Action act = () =>
			{
				sum = testCase.A + testCase.B;
			};

			// Assert
			act.Should().NotThrow();
			sum.Should().NotBeNull().And.Be(testCase.Result);
		}

		private static IEnumerable<ArithmeticTestCase> SubTestCases()
		{

			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.ZERO, Vector2d.ZERO);
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.NORTH, Vector2d.SOUTH);
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.SOUTH, Vector2d.NORTH);
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.WEST, Vector2d.EAST);
			yield return new ArithmeticTestCase(Vector2d.ZERO, Vector2d.EAST, Vector2d.WEST);
			yield return new ArithmeticTestCase(Vector2d.NORTH, Vector2d.SOUTH, new Vector2d(0, 2));
			yield return new ArithmeticTestCase(Vector2d.SOUTH, Vector2d.NORTH, new Vector2d(0, -2));
			yield return new ArithmeticTestCase(Vector2d.WEST, Vector2d.EAST, new Vector2d(-2, 0));
			yield return new ArithmeticTestCase(Vector2d.EAST, Vector2d.WEST, new Vector2d(2, 0));
			yield return new ArithmeticTestCase(new Vector2d(13, 6), new Vector2d(-4, 1), new Vector2d(17, 5));
			yield return new ArithmeticTestCase(new Vector2d(int.MinValue, int.MaxValue), new Vector2d(1, -1), new Vector2d(int.MaxValue, int.MinValue));
			yield return new ArithmeticTestCase(new Vector2d(int.MaxValue, 0), new Vector2d(-1, 0), new Vector2d(int.MinValue, 0));
		}

		[Test, TestCaseSource(nameof(SubTestCases))]
		public void Vector2d_SubTest(ArithmeticTestCase testCase)
		{
			// Arrange		
			Vector2d? sum = null;

			// Act
			Action act = () =>
			{
				sum = testCase.A - testCase.B;
			};

			// Assert
			act.Should().NotThrow();
			sum.Should().NotBeNull().And.Be(testCase.Result);
		}


		[TestCase(10)]
		[TestCase(100)]
		[TestCase(1000)]
		[TestCase(10000)]
		public void Vector2d_HashCollisionTest(int limit)
		{
			// Arrange
			int max = (limit * 2 + 1) * (limit * 2 + 1);
			int half = max / 2;
			HashSet<int> hashset = new HashSet<int>(max);

			// Act
			for (int x = -1 * limit; x <= 1 * limit; x++)
			{
				for (int y = -1 * limit; y <= 1 * limit; y++)
				{
					Vector2d point = new Vector2d(x, y);
					int hash = point.GetHashCode();
					hashset.Add(hash);
				}
			}

			// Assert
			hashset.Count.Should().BeGreaterThan(half);
		}
	}
}
