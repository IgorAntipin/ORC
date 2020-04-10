using Orc.Domain.Interfaces;
using Orc.Domain.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotInstructions
{
	public class CountInstruction : BaseRobotInstruction<VoidReport>
	{
		public int Count { get; set; }
	}
}
