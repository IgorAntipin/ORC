using Orc.Domain.Interfaces;
using Orc.Domain.RobotResponses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Queries
{
	public class JobReportQuery : IQuery<JobReportResponse>
	{
		public JobReportQuery(Guid jobId)
		{
			JobId = jobId;
		}

		public Type ResultType => typeof(JobReportResponse);

		public Guid JobId { get; set; }
	}
}
