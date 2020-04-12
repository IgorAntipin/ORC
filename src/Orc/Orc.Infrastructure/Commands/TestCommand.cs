using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure.Commands
{
	/// <summary>
	/// Test command - is used for testing purpose only
	/// </summary>
	public class TestCommand : ICommand
	{
		/// <summary>
		/// Input parameter of the command
		/// </summary>
		public int Integer { get; set; }

		/// <summary>
		/// This string property is used for testing purpose only to verify the result of command execution, i.e. it is like an output parameter.
		/// When necessary to get the result of execution of some function , it is suggested to use process IQuery.
		/// </summary>
		public string Text { get; set; }
	}
}
