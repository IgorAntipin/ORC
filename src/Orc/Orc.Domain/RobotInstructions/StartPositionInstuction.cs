using Orc.Common.Types;
using Orc.Domain.Interfaces;
using Orc.Domain.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotInstructions
{
	public class StartPositionInstuction : BaseRobotInstruction<StatusReport>
	{
		public Vector2d AbsolutePosition { get; set; }
	}
}
