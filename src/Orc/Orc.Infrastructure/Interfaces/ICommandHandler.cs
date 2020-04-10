using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Defines methods for a command handler 
	/// that is responsible for executing a particular command 
	/// and encapsulates logic of that command 
	/// </summary>
	public interface ICommandHandler
	{
		/// <summary>
		/// Call this method to determine if the handler can execute the specified command
		/// </summary>
		/// <param name="command">ICommand</param>
		/// <returns>true if the command can be handled, otherwise false</returns>
		bool CanHandle(ICommand command);

		/// <summary>
		/// Execute the specified command
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		Task HandleAsync(ICommand command);
	}
}
