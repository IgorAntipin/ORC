using Orc.Common;
using Orc.Common.Types;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure.QueryHandlers
{
	public class StartPositionGeneratorQueryHandler : InstructionGeneratorQueryHandlerBase<StartPositionGeneratorQuery, StartPositionInstuction>
	{
		protected override StartPositionInstuction Parse(string input)
		{
			string[] coords = input.Split(' ');

			if(coords.Length != 2)
			{
				throw new InvalidOperationException($"Failed to parse the coordinates of the start position . '{input}' is not a valid format.");
			}

			if (!int.TryParse(coords[0], out int x))
			{
				throw new InvalidOperationException($"Failed to parse X coordinate of the start position. '{coords[0]}' in not a valid format.");
			}
			
			ValidateX(x);

			if (!int.TryParse(coords[1], out int y))
			{
				throw new InvalidOperationException($"Failed to parse Y coordinate of the start position. '{coords[1]}' in not valid coordinate.");
			}
			
			ValidateY(y);

			Vector2d startPosition = new Vector2d(x,y);

			return new StartPositionInstuction(startPosition, true);
		}

		private void ValidateX(int x)
		{
			if (x < Constants.WORLD_X_MIN || x > Constants.WORLD_X_MAX)
				throw new ArgumentOutOfRangeException($"Invalid X coordinate for StartPositionInstuction. '{x}' is not in the range ['{Constants.WORLD_X_MIN}'..'{Constants.WORLD_X_MAX}'] ");
		}

		private void ValidateY(int y)
		{
			if (y < Constants.WORLD_Y_MIN || y > Constants.WORLD_Y_MAX)
				throw new ArgumentOutOfRangeException($"Invalid X coordinate for StartPositionInstuction. '{y}' is not in the range ['{Constants.WORLD_Y_MIN}'..'{Constants.WORLD_Y_MAX}'] ");
		}

		protected override bool ValidateInputString(string input)
		{
			return true;
		}
	}
}
