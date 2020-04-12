using Orc.Domain.RobotInstructions;
using Orc.Infrastructure.Core;
using Orc.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.QueryHandlers
{
	public class CountGeneratorQueryHandler : QueryHandlerBase<CountGeneratorQuery, int>
	{
		public async override Task<int> HandleQueryAsync(CountGeneratorQuery query)
		{
			var str = await ReadInputAsync(query);
			if (!ValidateInputString(str))
			{
				
			}

			var instruction = Parse(str);

			return instruction;
		}

		private async Task<string> ReadInputAsync(CountGeneratorQuery query)
		{
			var str = await query.Reader.ReadLineAsync();
			return str;
		}

		private bool ValidateInputString(string input)
		{
			return true;
		}

		private int Parse(string input)
		{
			if (int.TryParse(input, out int count))
			{
				return count;
			}

			return 0;
		}
	}
}
