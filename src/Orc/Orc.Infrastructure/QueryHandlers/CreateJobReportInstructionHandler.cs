using Orc.Domain.Reports;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.QueryHandlers
{
	public class CreateJobReportInstructionHandler : RobotInstructionQueryHandlerBase<CreateJobReportInstruction, JobReport>
	{

		private readonly IRobotStore _robotStore;

		public CreateJobReportInstructionHandler(IRobotStore robotStore)
		{
			_robotStore = robotStore ?? throw new ArgumentNullException(nameof(robotStore));
		}

		public override async Task<JobReport> HandleQueryAsync(CreateJobReportInstruction query)
		{
			var count = await _robotStore.GetCleanedPointsCountAsync();

			return new JobReport()
			{
				UniqueCleanedVerticesCount = count
			};
		}
	}
}
