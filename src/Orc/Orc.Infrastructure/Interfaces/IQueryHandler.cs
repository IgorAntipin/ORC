using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Defines methods for a query handler 
	/// that is responsible for executing a particular query 
	/// and encapsulates logic of that query 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IQueryHandler<TResult> : ICommand
	{
		/// <summary>
		/// Execute the specified query and return the result of the expected type
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		Task<TResult> HandleAsync(IQuery<TResult> query);
	}
}
