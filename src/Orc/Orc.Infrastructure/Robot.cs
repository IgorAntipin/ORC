using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public class Robot : IRobot
	{
		public Robot()
		{

		}

		public async Task<bool> AddJobAsync(IRobotJob job)
		{

			await Task.CompletedTask;
			
			return true;
		}

		public async Task<IRobotReport> GetReportAsync(Guid jobId)
		{
			await Task.CompletedTask;

			return null;
		}

		public async Task RunAllJobsAsync()
		{
			await Task.CompletedTask;
		}
	}
}
