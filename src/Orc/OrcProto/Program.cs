using Autofac.Extensions.DependencyInjection;
using Orc.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Serilog;
using System.Threading.Tasks;

namespace OrcProto
{
	class Program
	{
		static async Task<int> Main(string[] args)
		{
			Console.WriteLine("Orc prototype");

			ConfigureLogger();

			var container = ContainerConfig.GetContainer();

			var serviceProvider = new AutofacServiceProvider(container);

			try
			{
				var _robot = serviceProvider.GetService<IRobot>();
				await _robot.DoJobAsync();
				return 0;
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "ORC Proto crushed due to a critical error.");
				return 1;
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
