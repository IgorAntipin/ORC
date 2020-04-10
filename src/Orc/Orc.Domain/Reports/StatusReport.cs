using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static Orc.Common.Enums;

namespace Orc.Domain.Reports
{
	public class StatusReport : IRobotReport
	{
		public RobotInstructionStatus Status { get; set; }
		public string Error { get; set; }
	}
}
