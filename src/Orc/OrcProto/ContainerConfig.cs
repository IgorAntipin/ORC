using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacSerilogIntegration;
using Microsoft.Extensions.DependencyInjection;
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
		public static IContainer GetContainer(bool isTest = false, Action<ContainerBuilder> overrider = null)
		{
			var builder = new ContainerBuilder();

			builder.Populate(new ServiceCollection()); // the only purpose of this line is to make IServiceProvider resolvable using Autofac

			builder.RegisterLogger();

			builder.RegisterType<OrcApp>()
				.As<IOrcApp>()
				.InstancePerDependency();

			builder.RegisterModule(new DIModule() { IsTest = isTest });

			overrider?.Invoke(builder);

			return builder.Build();
		}
	}
}
