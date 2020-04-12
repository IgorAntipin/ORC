using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Interfaces
{
	/// <summary>
	/// Interface for a query that can be executed by IQueryProcessor 
	/// in order to get a result of execution
	/// </summary>
	/// <typeparam name="TResult">expected result</typeparam>
	public interface IQuery<out TResult> : ICommand
	{
		Type ResultType { get; }
	}
}
