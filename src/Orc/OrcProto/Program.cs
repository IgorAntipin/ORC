﻿using Autofac.Extensions.DependencyInjection;
using Orc.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Serilog;
using System.Threading.Tasks;
using OrcProto.App;

namespace OrcProto
{
	class Program
	{
		static async Task Main(string[] args)
		{
			ConfigureLogger();

			var container = ContainerConfig.GetContainer();

			var serviceProvider = new AutofacServiceProvider(container);

			try
			{
				var app = serviceProvider.GetService<IOrcApp>();
				await app.StartAsync(Console.In, Console.Out);
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "ORC Proto crushed due to a critical error.");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static void ConfigureLogger()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File("OrcProto.log")
				.CreateLogger();
		}
	}
}
