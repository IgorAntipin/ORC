using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure
{
	public class ReportProcessor : IReportProcessor
	{
		public ReportProcessor()
		{

		}

		public async Task ProcessAsync(IRobotReport report)
		{
			await Task.CompletedTask;
		}


	}
}
