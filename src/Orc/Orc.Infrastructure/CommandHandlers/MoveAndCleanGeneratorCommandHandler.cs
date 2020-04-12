using Orc.Common;
using Orc.Common.Types;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Commands;
using Orc.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.CommandHandlers
{
	public class MoveAndCleanGeneratorCommandHandler : CommandHandlerBase<MoveAndCleanGeneratorCommand>
	{
		protected override async Task HandleCommandAsync(MoveAndCleanGeneratorCommand command)
		{
			for (int i = 0; i < command.Count; i++)
			{
				var input = await ReadInputAsync(command);
				if (!ValidateInputString(input))
				{
					throw new InvalidOperationException($"Failed to parse command #{i} of type '{typeof(MoveAndCleanGeneratorCommand)}'. '{input}' is not a valid format.");
				}

				var instuction = Parse(input, i);

				command.Queue.Enqueue(instuction);
			}
		}

		private async Task<string> ReadInputAsync(InspructionGeneratorCommand command)
		{
			var str = await command.Reader.ReadLineAsync();
			return str;
		}

		private bool ValidateInputString(string input)
		{
			return true;
		}

		private MoveAndCleanInstruction Parse(string input, int index)
		{
			string[] args = input.Split(' ');
			if(args.Length != 2)
			{
				throw new InvalidOperationException($"Failed to parse command #{index} of type '{typeof(MoveAndCleanGeneratorCommand)}'. '{input}' is not a valid format.");
			}

			Vector2d vector;

			if(Enum.TryParse<Enums.WorldDirection>(args[0], true, out Enums.WorldDirection direction))
			{
				switch (direction)
				{
					case Enums.WorldDirection.N:
						vector = Vector2d.NORTH;
						break;
					case Enums.WorldDirection.E:
						vector = Vector2d.EAST;
						break;
					case Enums.WorldDirection.S:
						vector = Vector2d.SOUTH;
						break;
					case Enums.WorldDirection.W:
						vector = Vector2d.WEST;
						break;
					default:
						throw new InvalidOperationException($"Failed to parse direction for command #{index} of type '{typeof(MoveAndCleanGeneratorCommand)}'. '{args[0]}' is not a valid format.");
						break;
				}
			}
			else
			{
				throw new InvalidOperationException($"Failed to parse direction for command #{index} of type '{typeof(MoveAndCleanGeneratorCommand)}'. '{args[0]}' is not a valid format.");
			}

			if(!int.TryParse(args[1], out int steps))
			{
				throw new InvalidOperationException($"Failed to parse the number of steps for command #{index} of type '{typeof(MoveAndCleanGeneratorCommand)}'. '{args[1]}' is not a valid format.");
			}

			ValidateSteps(steps, index);

			return new MoveAndCleanInstruction(vector, steps);
		}

		private void ValidateSteps(int steps, int index)
		{
			if(steps <= 0 || steps > Constants.MAX_STEPS)
			{
				throw new ArgumentOutOfRangeException($"Invalid number of steps for command #{index} of type '{typeof(MoveAndCleanGeneratorCommand)}. '{steps}' is not in the range ['{1}'..'{Constants.MAX_STEPS}'] ");
			}
		}
	}
}
