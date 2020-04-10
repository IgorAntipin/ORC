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
		Task<IRobotReport> RunInstructionAsync(IRobotInstruction instruction);
		Task<bool> AddJobAsync(IRobotJob job);
		Task RunAllJobsAsync();
	}
}
