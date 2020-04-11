using Orc.Common.Types;
using Orc.Domain.Interfaces;
using Orc.Domain.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotInstructions
{
	public class MoveAndCleanInstruction : BaseRobotInstruction<StatusReport>
	{
		public Vector2d Direction { get; set; }
		public int NumberOfSteps { get; set; }
	}
}
