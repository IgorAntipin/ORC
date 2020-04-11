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
	public class RobotIT
	{
		[SetUp]
		public void Setup()
		{

		}


		[Test]
		public void MethodName_ExpectedBehavior_When_StateUnderTest()
		{
			// Arrange

			// Act
			// Assert

		}
	}
}
