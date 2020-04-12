using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Interfaces
{
	/// <summary>
	/// Interface for an instruction containing a command to execute
	/// </summary>
	public interface ICommandInstruction : IInstruction
	{
		ICommand Command { get; }
	}
}
