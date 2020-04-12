using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Components
{
	/// <summary>
	/// Robot with processor
	/// </summary>
	public class Robot : IRobot
	{
		private readonly IProcessor _processor;

		public Robot(IProcessor processor)
		{
			_processor = processor ?? throw new ArgumentNullException(nameof(processor));
		}

		public async Task<TResponse> ProcessRequestAsync<TResponse>(IRobotRequest<TResponse> request) 
			where TResponse : IRobotResponse
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));
			
			if (request is IRobotQueryRequest<TResponse>)
			{
				var queryableReq = request as IRobotQueryRequest<TResponse>;
				
				if (queryableReq.Query == null)
					throw new NullReferenceException($"Invalid reportQuery. Query is null.");

				var response = await _processor.ExecuteAsync(queryableReq.Query);
				return response;
			}

			throw new NotImplementedException($"Failed to process request. Unknown request type '{request.GetType()}'.");
		}

		public async Task RunInstructionAsync(IInstruction instruction)
		{
			if (instruction == null)
				throw new ArgumentNullException(nameof(instruction));

			await ExecuteAsync(instruction);
		}

		public async Task<bool> RunJobAsync(IRobotJob job)
		{
			if (job == null)
				throw new ArgumentNullException(nameof(job));

			bool success = true;

			while (job.Instructions.Count > 0)
			{
				var instruction = job.Instructions.Dequeue();
				if (instruction == null)
					throw new NullReferenceException($"Failed to complete job. Instruction is null.");

				await ExecuteAsync(instruction);
			}

			return success;
		}




		private async Task ExecuteAsync(IInstruction instruction)
		{
			if(instruction is ICommandInstruction)
			{
				var cmdInstruction = instruction as ICommandInstruction;

				if (cmdInstruction.Command == null)
					throw new NullReferenceException($"Failed to execute command instruction {cmdInstruction.GetType()}. Command is null.");
				
				await _processor.ExecuteAsync(cmdInstruction.Command);
				return;
			}

			throw new NotImplementedException($"Failed to execute instruction. Unknown instruction type {instruction.GetType()}.");
		}



	}
}
