using Autofac;
using AutofacSerilogIntegration;
using Microsoft.Extensions.Logging;
using Orc.Infrastructure;
using OrcProto.App;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcProto
{
	public static class ContainerConfig
	{
		public static IContainer GetContainer()
		{
			var builder = new ContainerBuilder();

			builder.RegisterLogger();

			builder.RegisterType<OrcApp>()
				.As<IOrcApp>()
				.InstancePerDependency();

			builder.RegisterModule<DIModule>();

			return builder.Build();
		}
	}
}
