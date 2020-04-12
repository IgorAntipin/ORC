using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Orc.Domain.Interfaces;
using Orc.Domain.RobotResponses;
using Orc.Infrastructure.Components;
using Orc.Infrastructure.Interfaces;

namespace OrcProto.IntegrationTests
{
	[TestFixture]
	public class ReportWriterIT
	{

		private IProcessor _processor;

		[SetUp]
		public void Setup()
		{
			_processor  = A.Fake<IProcessor>();
		}

		[Test]
		public void Ctor_WhenProcessorNull_ShouldThrow()
		{
			//Arrange
			IProcessor processor = null;

			// Act
			Action act = () =>
			{
				var reportWriter = new ReportWriter(processor);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message.Contains("processor");
		}

		[Test]
		public void Ctor_WhenArgumentsNotNull_ShouldNotThrow()
		{
			//Arrange
			IReportWriter reportWriter = null;

			// Act
			Action act = () =>
			{
				reportWriter = new ReportWriter(_processor);
			};

			// Assert
			act.Should().NotThrow();
			reportWriter.Should().NotBeNull();
		}

		[Test]
		public void WriteAsync_WhenTextWriterNotSet_ShouldThrow()
		{
			//Arrange
			IRobotResponse report = new JobReportResponse();
			IReportWriter reportWriter = new ReportWriter(_processor);

			// Act
			Func<Task> act = async () =>
			{
				await reportWriter.WriteAsync(report);
			};

			// Assert
			act.Should().Throw<NullReferenceException>()
				.And.Message.Should().Contain("Failed to write a report. TextWriter is null.");
		}

		[Test]
		public void WriteAsync_WhenTextWriterSet_ShouldWriteToOutput()
		{
			//Arrange
			string output = null;
			int count = 9;
			JobReportResponse report = new JobReportResponse();
			report.UniqueCleanedVerticesCount = count;

			StringBuilder sb = new StringBuilder();
			TextWriter textWriter = new StringWriter(sb);

			var container = ContainerConfig.GetContainer(true);
			var serviceProvider = new AutofacServiceProvider(container);

			IReportWriter reportWriter = serviceProvider.GetService<IReportWriter>();

			reportWriter.UseTextWriter(textWriter);
			// Act
			Func<Task> act = async () =>
			{

				await reportWriter.WriteAsync(report);
				output = sb.ToString();
			};

			// Assert
			act.Should().NotThrow();
			output.Should().NotBeNullOrEmpty()
				.And.Be($"=> Cleaned: {count}\r\n");
		}
	}
}
