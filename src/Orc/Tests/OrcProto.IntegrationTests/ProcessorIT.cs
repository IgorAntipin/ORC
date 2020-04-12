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
using Orc.Infrastructure.Core;
using Orc.Domain.Interfaces;
using Orc.Infrastructure.Commands;
using Orc.Infrastructure.Queries;

namespace OrcProto.IntegrationTests
{

	[TestFixture]
	public class ProcessorIT
	{
		private IServiceProvider _serviceProvider;

		[SetUp]
		public void Setup()
		{
			var container = ContainerConfig.GetContainer();
			_serviceProvider = new AutofacServiceProvider(container);
		}

		[Test]
		public void Ctor_WhenServiceProviderNull_ShouldThrow()
		{
			//Arrange
			IServiceProvider serviceProvider = null;

			// Act
			Action act = () =>
			{
				var processor = new Processor(serviceProvider);
			};

			// Assert
			act.Should().Throw<ArgumentNullException>()
				.And.Message.Contains("serviceProvider");
		}

		[Test]
		public void Ctor_WhenArgumentsNotNull_ShouldInstanciateAndNotThrow()
		{
			// Arrange
			IServiceProvider serviceProvider = A.Fake<IServiceProvider>();
			IProcessor processor = null;
			
			// Act
			Action act = () =>
			{
				processor = new Processor(serviceProvider);
			};

			// Assert
			act.Should().NotThrow();
			processor.Should().NotBeNull();
		}


		[Test]
		public void DIContainer_WhenAllDependenciesRegistered_ShouldResolveIProcessor()
		{
			// Arrange
			IProcessor processor = null;

			var container = ContainerConfig.GetContainer();
			var serviceProvider = new AutofacServiceProvider(container);

			// Act
			Action act = () =>
			{
				processor = serviceProvider.GetService<IProcessor>();
			};

			// Assert
			act.Should().NotThrow();
			processor.Should().NotBeNull();
		}

		/// <summary>
		/// Unknown command that does not have handler registered in DI container
		/// </summary>
		protected internal class UnknownCommand : ICommand
		{
		}

		[Test]
		public void ExecuteAsync_WhenUnknownCommandGiven_ShouldThrow()
		{
			// Arrange
			var processor = _serviceProvider.GetService<IProcessor>();

			var command = new UnknownCommand();

			// Act
			Func<Task> act = async () =>
			{
				await processor.ExecuteAsync(command);
			};

			// Assert
			act.Should().Throw<InvalidOperationException>()
				.And.Message.Contains(command.GetType().Name);
		}

		[Test]
		public void ExecuteAsync_WhenTestCommandGiven_ShouldProcessCommand()
		{
			// Arrange
			int val = 4012;
			var command = new TestCommand();    // test command
			command.Integer = val;             // test parameter
			command.Text = null;                // when command is processed, handler will set this parameter to some value.
												// this approach is used for testing purpose only.
												// when necessary to get a result of processing, use a query instead of a command.

			var processor = _serviceProvider.GetService<IProcessor>();

			// Act
			Func<Task> act = async () =>
			{
				await processor.ExecuteAsync(command);
			};

			// Assert
			act.Should().NotThrow();
			command.Should().NotBeNull();
			command.Text.Should().NotBeNullOrEmpty().And.Contain($"{val}");
		}


		/// <summary>
		/// Unknown query that does not have handler
		/// </summary>
		protected internal class UnknownQuery : IQuery<string>
		{
			public Type ResultType { get { return typeof(string); } }

			public IQuery<string> GetConcreteQuery()
			{
				return this;
			}
		}

		[Test]
		public void ExecuteAsync_WhenUnknownQueryGiven_ShouldThrow()
		{
			// Arrange
			var processor = _serviceProvider.GetService<IProcessor>();

			var query = new UnknownQuery();
			string result = null;

			// Act
			Action act = () =>
			{
				result = processor.ExecuteAsync(query).GetAwaiter().GetResult();
			};

			// Assert
			act.Should().Throw<InvalidOperationException>()
				.And.Message.Contains(query.GetType().Name);
			result.Should().BeNull();
		}



		[Test]
		public void ExecuteAsync_WhenTestQueryGiven_ShouldReturnProcessedResult()
		{
			// Arrange
			var code = 1001;

			var processor = _serviceProvider.GetService<IProcessor>();

			var query = new TestQuery(code);
			string result = null;

			// Act
			Action act = () =>
			{
				result = processor.ExecuteAsync(query).GetAwaiter().GetResult();
			};

			// Assert
			act.Should().NotThrow();
			result.Should().NotBeNullOrWhiteSpace();
			result.Should().Be($"TestQuery processsed with code '{code}'.");
		}
	}

}
