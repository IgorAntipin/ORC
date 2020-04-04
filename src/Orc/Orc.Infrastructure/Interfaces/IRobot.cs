using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Interface for Robot
	/// </summary>
	public interface IRobot
	{
		Task<bool> AddJobAsync(IRobotJob job);
		Task RunAllJobsAsync();
		Task<IRobotReport> GetReportAsync(Guid jobId);
	}
}
