using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Defines a method that execute a query and return a result
	/// </summary>
	public interface IQueryProcessor
	{
		/// <summary>
		/// Run asynchronously the specified query and return expected result
		/// </summary>
		/// <typeparam name="TResult">generic parameter for the expected result type</typeparam>
		/// <param name="query">query to execute</param>
		/// <returns>query execution result</returns>
		Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
	}
}
