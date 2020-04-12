using Orc.Domain.RobotResponses;
using Orc.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.CommandHandlers
{
	public class JobReportResponseCommandHandler : CommandHandlerBase<JobReportResponse>
	{
		protected override async Task HandleCommandAsync(JobReportResponse command)
		{
			if (command.TextWriter == null)
				throw new NullReferenceException($"Failed to write report of type '{typeof(JobReportResponse)}'. TextWriter is null.");
			await command.TextWriter.WriteLineAsync($"=> Cleaned: {command.UniqueCleanedVerticesCount}");
		}
	}
}
