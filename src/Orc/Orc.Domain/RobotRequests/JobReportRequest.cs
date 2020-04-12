using Orc.Domain.Interfaces;
using Orc.Domain.Queries;
using Orc.Domain.RobotResponses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotRequests
{
	public class JobReportRequest : IRobotQueryRequest<JobReportResponse>
	{
		public JobReportRequest(Guid jobId)
		{
			JobId = jobId;
		}

		public IQuery<JobReportResponse> Query => new Lazy<JobReportQuery>(() => new JobReportQuery(JobId)).Value;

		public Guid JobId { get;}
	}
}
