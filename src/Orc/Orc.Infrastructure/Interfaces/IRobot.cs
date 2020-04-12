using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Robot API
	/// </summary>
	public interface IRobot
	{
		/// <summary>
		/// Executes single instruction
		/// </summary>
		/// <param name="instruction"></param>
		/// <returns></returns>
		Task RunInstructionAsync(IInstruction instruction);

		/// <summary>
		/// Executes all instuction from the job
		/// </summary>
		/// <param name="job"></param>
		/// <returns></returns>
		Task<bool> RunJobAsync(IRobotJob job);

		/// <summary>
		/// Executes request and return response
		/// </summary>
		/// <typeparam name="TResponse">Response type</typeparam>
		/// <param name="request">request</param>
		/// <returns>response</returns>
		Task<TResponse> ProcessRequestAsync<TResponse>(IRobotRequest<TResponse> request) where TResponse : IRobotResponse;
	}
}
