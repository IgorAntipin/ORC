using Orc.Infrastructure.Core;
using Orc.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.QueryHandlers
{
	public abstract class InstructionGeneratorQueryHandlerBase<TQuery, TResult> : QueryHandlerBase<TQuery, TResult>
		where TQuery : InstructionGeneratorQuery<TResult>
	{
		protected TQuery Query;

		public async override Task<TResult> HandleQueryAsync(TQuery query)
		{
			Query = query;

			var input = await ReadInputAsync(query);
			if (!ValidateInputString(input))
			{
				throw new InvalidOperationException($"Failed to parse command of type '{typeof(TQuery)}'. '{input}' is not a valid format.");
			}

			TResult result = Parse(input);

			return result;
		}

		private async Task<string> ReadInputAsync(InstructionGeneratorQuery<TResult> query)
		{
			var str = await query.Reader.ReadLineAsync();
			return str;
		}

		protected abstract bool ValidateInputString(string input);

		protected abstract TResult Parse(string input);

	}
}
