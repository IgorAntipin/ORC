using Autofac;
using Orc.Infrastructure.Components;
using Orc.Infrastructure.Core;
using Orc.Infrastructure.Interfaces;
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

			base.Load(builder);
		}
	}
}
