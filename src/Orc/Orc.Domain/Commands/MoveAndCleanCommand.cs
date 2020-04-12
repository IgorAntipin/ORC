using Orc.Common.Types;
using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Commands
{
	public class MoveAndCleanCommand : ICommand
	{
		public MoveAndCleanCommand(Vector2d direction, int steps)
		{
			Direction = direction;
			NumberOfSteps = steps;
		}

		public Vector2d Direction { get; set; }
		public int NumberOfSteps { get; set; }
	}
}
