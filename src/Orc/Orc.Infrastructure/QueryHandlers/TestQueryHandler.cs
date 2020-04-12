using Orc.Infrastructure.Core;
using Orc.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.QueryHandlers
{
	/// <summary>
	/// Handler for the test query
	/// </summary>
	public class TestQueryHandler : QueryHandlerBase<TestQuery, string>
	{
		public async override Task<string> HandleQueryAsync(TestQuery query)
		{
			await Task.CompletedTask;

			return $"TestQuery processsed with code '{query.Code}'.";
		}
	}
}
