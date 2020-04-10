using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Components
{
	public class Robot : IRobot
	{
		private readonly IProcessor _processor;
		private readonly IJobStore _jobStore;

		public Robot(IProcessor processor)
		{
			_processor = processor ?? throw new ArgumentNullException(nameof(processor));
		}

		public async Task<bool> AddJobAsync(IRobotJob job)
		{
			await _jobStore.AddJobAsync(job);

			return true;
		}

		public async Task RunAllJobsAsync()
		{
			var job = await _jobStore.GetNextJobAsync();
			
			while(job != null)
			{
				await RunJobAsync(job);
				
				job = await _jobStore.GetNextJobAsync();
			}			
		}

		public async Task<IRobotReport> RunInstructionAsync(IRobotInstruction instruction)
		{
			await Task.CompletedTask;

			return null;
		}


		private async Task RunJobAsync(IRobotJob job)
		{
			while(job.Instructions.Count > 0)
			{
				var instruction = job.Instructions.Dequeue();
				var report = await RunInstructionAsync(instruction);
			}
		}
	}
}
