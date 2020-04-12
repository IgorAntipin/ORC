using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResponse"></typeparam>
	public interface IRobotQueryRequest<TResponse> : IRobotRequest<TResponse>
		where TResponse : IRobotResponse
	{
		/// <summary>
		/// Query for execution
		/// </summary>
		IQuery<TResponse> Query { get; }
	}
}
