using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Components
{
	public class ReportWriter : IReportWriter
	{
		private readonly IProcessor _processor;
		private TextWriter _textWriter;

		public ReportWriter(IProcessor processor)
		{
			_processor = processor ?? throw new ArgumentNullException(nameof(processor));
		}

		public void UseTextWriter(TextWriter textWriter)
		{
			_textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
		}

		public async Task WriteAsync(IRobotResponse report)
		{
			if(_textWriter == null)
				throw new NullReferenceException($"Failed to write a report. TextWriter is null.");

			var command = report as ICommand;
			if(command != null)
			{
				await _processor.ExecuteAsync(command);
			}

			await Task.CompletedTask;

			
		}
	}
}
