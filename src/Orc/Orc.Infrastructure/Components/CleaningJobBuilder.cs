﻿using Orc.Domain.Interfaces;
using Orc.Domain.Jobs;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Commands;
using Orc.Infrastructure.Interfaces;
using Orc.Infrastructure.Queries;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Components
{
	/// <summary>
	/// Can read from given text reader and build a Cleaning job
	/// </summary>
	public class CleaningJobBuilder : IRobotJobBuilder
	{
		private readonly IProcessor _processor;
		private readonly ILogger _logger;
		
		private TextReader _textReader;


		public CleaningJobBuilder(IProcessor processor, ILogger logger)
		{
			_processor = processor ?? throw new ArgumentNullException(nameof(processor));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Build a cleaning job
		/// </summary>
		/// <returns></returns>
		public async Task<IRobotJob> BuildAsync()
		{
			if (_textReader == null)
				throw new NullReferenceException($"Failed to build a job. TextReader is null.");

			int totalCount = 1;
			var instructionCount = await GetCountInstruction();

			totalCount += instructionCount;

			var queue = new Queue<IInstruction>(totalCount);

			var startPosition = await GetStartPositionInstruction();
			queue.Enqueue(startPosition);

			await PopulateWithMoveAndCleanInstructions(instructionCount, queue);

			var job = CreateJob();
			job.Instructions = queue;

			_logger.Debug($"A new robot job is created.");
			_logger.Debug($"Id: '{job.Id}'.");
			_logger.Debug($"Instructions count: '{job.Instructions.Count}'.");

			return job;
		}

		/// <summary>
		/// Set TextReader as input data source
		/// </summary>
		/// <param name="textReader"></param>
		public void UseReader(TextReader textReader)
		{
			_textReader = textReader ?? throw new ArgumentNullException(nameof(textReader));
		}

		private IRobotJob CreateJob()
		{
			var job = new RobotJob();
			job.Id = Guid.NewGuid();
			
			return job;				 
		}

		private async Task<int> GetCountInstruction()
		{
			var query = new CountGeneratorQuery();
			query.Reader = _textReader;

			int count = await _processor.ExecuteAsync(query);

			return count;
		}

		private async Task<StartPositionInstuction> GetStartPositionInstruction()
		{
			var query = new StartPositionGeneratorQuery();
			query.Reader = _textReader;

			StartPositionInstuction instruction = await _processor.ExecuteAsync(query);

			return instruction;
		}

		private async Task PopulateWithMoveAndCleanInstructions(int count, Queue<IInstruction> queueToUpdate)
		{
			var command = new MoveAndCleanGeneratorCommand();
			command.Reader = _textReader;
			command.Count = count;
			command.Queue = queueToUpdate;

			await _processor.ExecuteAsync(command);
		}

	}
}

