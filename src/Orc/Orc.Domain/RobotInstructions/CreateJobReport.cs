using Orc.Domain.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotInstructions
{
	public class CreateJobReport : BaseRobotInstruction<JobReport>
	{
		public Guid JobId { get; set; }
	}
}
