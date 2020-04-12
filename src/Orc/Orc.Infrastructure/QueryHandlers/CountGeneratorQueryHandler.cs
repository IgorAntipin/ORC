using Orc.Common;
using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Core;
using Orc.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.QueryHandlers
{

	public class CountGeneratorQueryHandler : InstructionGeneratorQueryHandlerBase<CountGeneratorQuery, int>
	{
		protected override bool ValidateInputString(string input)
		{
			return true;
		}

		protected override int Parse(string input)
		{
			if (!int.TryParse(input, out int count))
			{
				throw new InvalidOperationException($"Failed to parse the number of instructions. '{input}' is not valid.");
			}
			ValidateCount(count);
			return count;
		}

		private void ValidateCount(int count)
		{
			if (count < 0 || count > Constants.MAX_COMMANDS)
				throw new ArgumentOutOfRangeException($"Invalid number of instructions. Must be in range [{0}..{Constants.MAX_COMMANDS}]");
		}
	}

	//public class CountGeneratorQueryHandler : QueryHandlerBase<CountGeneratorQuery, int>
	//{
	//	public async override Task<int> HandleQueryAsync(CountGeneratorQuery query)
	//	{
	//		var str = await ReadInputAsync(query);
	//		if (!ValidateInputString(str))
	//		{

	//		}

	//		var count = Parse(str);

	//		return count;
	//	}

	//	private async Task<string> ReadInputAsync(CountGeneratorQuery query)
	//	{
	//		var str = await query.Reader.ReadLineAsync();
	//		return str;
	//	}

	//	private bool ValidateInputString(string input)
	//	{
	//		return true;
	//	}

	//	private int Parse(string input)
	//	{
	//		if (int.TryParse(input, out int count))
	//		{
	//			return count;
	//		}

	//		return 0;
	//	}
	//}
}
