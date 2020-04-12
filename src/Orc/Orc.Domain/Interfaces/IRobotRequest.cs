using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Interfaces
{
	/// <summary>
	/// General abstraction for a robot request 
	/// </summary>
	/// <typeparam name="TReport"></typeparam>
	public interface IRobotRequest<TResponse>
		where TResponse : IRobotResponse
	{
	}
}
