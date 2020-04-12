using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Core;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orc.Domain.Queries;
using Orc.Domain.RobotResponses;

namespace Orc.Infrastructure.QueryHandlers
{
	public class JobReportQueryHandler : RobotRequestQueryHandlerBase<JobReportQuery, JobReportResponse>
	{

		private readonly IRobotStore _robotStore;

		public JobReportQueryHandler(IRobotStore robotStore)
		{
			_robotStore = robotStore ?? throw new ArgumentNullException(nameof(robotStore));
		}

		public override async Task<JobReportResponse> HandleQueryAsync(JobReportQuery query)
		{
			var count = await _robotStore.GetCleanedPointsCountAsync();

			return new JobReportResponse()
			{
				UniqueCleanedVerticesCount = count
			};
		}
	}
}
