using Orc.Common.Types;
using Orc.Domain.Commands;
using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.RobotInstructions
{
	public class MoveAndCleanInstruction : ICommandInstruction
	{
		public MoveAndCleanInstruction(Vector2d direction, int steps)
		{
			Direction = direction;
			NumberOfSteps = steps;
		}

		public Vector2d Direction { get; }
		public int NumberOfSteps { get; }

		public ICommand Command => new Lazy<MoveAndCleanCommand>(()=> new MoveAndCleanCommand(Direction, NumberOfSteps)).Value;
	}
}
