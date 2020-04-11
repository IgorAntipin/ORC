using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OrcProto.App
{
	/// <summary>
	/// Orc application
	/// </summary>
	public class OrcApp : IOrcApp
	{
		private readonly ILogger _logger;
		private readonly IRobotJobBuilder _jobBuilder;
		private readonly IRobot _robot;
		private readonly IReportWriter _reportWriter;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="jobBuilder"></param>
		/// <param name="robot"></param>
		/// <param name="reportProcessor"></param>
		/// <param name="logger"></param>
		public OrcApp(IRobotJobBuilder jobBuilder, IRobot robot, IReportWriter reportWriter, ILogger logger)
		{
			_jobBuilder = jobBuilder ?? throw new ArgumentNullException(nameof(jobBuilder));
			_robot = robot ?? throw new ArgumentNullException(nameof(robot));
			_reportWriter = reportWriter ?? throw new ArgumentNullException(nameof(reportWriter));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Start the application using the specified TextReader as input and textwriter as output
		/// </summary>
		/// <param name="reader">input</param>
		/// <param name="writer">output</param>
		/// <returns></returns>
		public async Task StartAsync(TextReader reader, TextWriter writer)
		{
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));

			if (writer == null)
				throw new ArgumentNullException(nameof(writer));

			_logger.Information("OrcApp started.");

			_logger.Information("OrcApp instructions parsing started.");

			_jobBuilder.UseReader(reader);
			var job = await _jobBuilder.BuildAsync();

			if (job == null)
				throw new NullReferenceException("OrcApp failed to get a job. Job is null.");

			_logger.Information("OrcApp robot deployment launched.");

			await _robot.AddJobAsync(job);
			await _robot.RunAllJobsAsync();

			_logger.Information("OrcApp report generation started.");

			var reportInstruction = new CreateJobReportInstruction();
			reportInstruction.JobId = job.Id;

			var report = await _robot.RunInstructionAsync(reportInstruction);

			_reportWriter.UseTextWriter(writer);
			await _reportWriter.WriteAsync(report);

			_logger.Information("OrcApp all done.");
		}
	}
}
