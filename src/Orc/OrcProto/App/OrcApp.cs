using Orc.Infrastructure.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrcProto.App
{
	/// <summary>
	/// Orc application integrates all components together and executes main business logic
	/// </summary>
	public class OrcApp : IOrcApp
	{
		private readonly ILogger _logger;
		private readonly IRobotJobBuilder _jobBuilder;
		private readonly IRobot _robot;
		private readonly IReportProcessor _reportProcessor;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="jobBuilder"></param>
		/// <param name="robot"></param>
		/// <param name="reportProcessor"></param>
		/// <param name="logger"></param>
		public OrcApp(IRobotJobBuilder jobBuilder, IRobot robot, IReportProcessor reportProcessor, ILogger logger)
		{
			_jobBuilder = jobBuilder ?? throw new ArgumentNullException(nameof(jobBuilder));
			_robot = robot ?? throw new ArgumentNullException(nameof(robot));
			_reportProcessor = reportProcessor ?? throw new ArgumentNullException(nameof(reportProcessor));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Start the application with given arguments
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public async Task StartAsync(string[] args)
		{
			_jobBuilder.UseArguments(args);

			var job = _jobBuilder.Build();

			await _robot.AddJobAsync(job);
			await _robot.RunAllJobsAsync();

			var report = await _robot.GetReportAsync(job.Id);
			await _reportProcessor.ProcessAsync(report);
		}
	}
}
