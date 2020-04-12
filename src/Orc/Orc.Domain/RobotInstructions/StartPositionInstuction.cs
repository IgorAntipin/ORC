using Orc.Common.Types;
using Orc.Domain.Commands;
using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotInstructions
{
	public class StartPositionInstuction : ICommandInstruction
	{
		public StartPositionInstuction(Vector2d absolute, bool clean)
		{
			AbsolutePosition = absolute;
			CleanInPosition = clean;
		}
		public Vector2d AbsolutePosition { get; }

		public bool CleanInPosition { get; }

		public ICommand Command => new Lazy<StartPositionCommand>(()=>new StartPositionCommand(AbsolutePosition, CleanInPosition)).Value;
	}
}
