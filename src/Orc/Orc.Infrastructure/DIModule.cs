using Autofac;
using Orc.Domain.Commands;
using Orc.Domain.Interfaces;
using Orc.Domain.Queries;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.CommandHandlers;
using Orc.Infrastructure.Commands;
using Orc.Infrastructure.Components;
using Orc.Infrastructure.Core;
using Orc.Infrastructure.Interfaces;
using Orc.Infrastructure.Queries;
using Orc.Infrastructure.QueryHandlers;
using Orc.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure
{
	/// <summary>
	/// Autofac module that registers all dependencies related to Orc Infrastructure
	/// </summary>
	public class DIModule : Module
	{
		public bool IsTest { get; set; }

		/// <summary>
		/// Register dependencies of the module
		/// </summary>
		/// <param name="builder"> ContainerBuilder </param>
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<Robot>()
				.As<IRobot>()
				.InstancePerDependency();

			builder.RegisterType<CleaningJobBuilder>()
				.As<IRobotJobBuilder>()
				.InstancePerDependency();

			builder.RegisterType<ReportWriter>()
				.As<IReportWriter>()
				.InstancePerDependency();


			builder.RegisterType<Processor>()
				.As<IQueryProcessor>()
				.As<ICommandProcessor>()
				.As<IProcessor>()
				.InstancePerLifetimeScope();
			
			if(IsTest == false)
			{
				builder.RegisterType<InMemoryRobotStore>()
					.As<IRobotStore>()
					.SingleInstance();
			}

			builder.RegisterType<ControllerFacade>()
				.As<IControllerFacade>()
				.SingleInstance();

			builder.RegisterType<SimpleNavigationService>()
				.As<INavigationService>()
				.SingleInstance();

			builder.RegisterGeneric(typeof(QueryHandlerBase<,>))
				.As(typeof(CommandHandlerBase<>))
				.As(typeof(IQueryHandler<>))
				.InstancePerDependency();
			
			builder.RegisterAssemblyTypes(typeof(TestCommandHandler).Assembly)
				.AsClosedTypesOf(typeof(CommandHandlerBase<>));

			if (IsTest == true)
			{
				builder.RegisterAssemblyTypes(typeof(TestCommand).Assembly)
				.As<ICommand>();
			}

			builder.RegisterAssemblyTypes(typeof(JobReportQueryHandler).Assembly)
				.AsClosedTypesOf(typeof(QueryHandlerBase<,>));

			builder.RegisterAssemblyTypes(typeof(CountGeneratorQuery).Assembly)
				.AsClosedTypesOf(typeof(IQuery<>));

			builder.RegisterAssemblyTypes(typeof(JobReportQuery).Assembly)
				.AsClosedTypesOf(typeof(IQuery<>));

			builder.RegisterAssemblyTypes(typeof(StartPositionCommand).Assembly)
				.As<ICommand>();

			base.Load(builder);
		}
	}
}
