using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Core
{
	/// <summary>
	/// Base query handler
	/// </summary>
	/// <typeparam name="TQuery"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public abstract class QueryHandlerBase<TQuery, TResult> : CommandHandlerBase<TQuery>, IQueryHandler<TResult>
			where TQuery : class, IQuery<TResult>
	{

		public async Task<TResult> HandleAsync(IQuery<TResult> query)
		{
			if (query == null)
				throw new ArgumentNullException(nameof(query));

			var concrete = GetConcreteCommand(query);

			return await HandleQueryAsync(concrete);
		}
		
		protected sealed override async Task HandleCommandAsync(TQuery query)
		{
			await HandleQueryAsync(query);
		}

		public abstract Task<TResult> HandleQueryAsync(TQuery query);
	}
}
