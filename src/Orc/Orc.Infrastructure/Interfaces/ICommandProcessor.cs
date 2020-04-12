using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Defines a method that execute a command
	/// </summary>
	public interface ICommandProcessor
	{
		/// <summary>
		/// Run asynchronously the specified command
		/// </summary>
		/// <param name="command">command to execute</param>
		/// <returns></returns>
		Task ExecuteAsync(ICommand command);
	}
}
