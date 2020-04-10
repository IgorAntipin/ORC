using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Interfaces
{

	public interface IRobotJob
	{
		/// <summary>
		/// Job Id
		/// </summary>
		Guid Id { get; set; }

		/// <summary>
		/// Queue of commands
		/// </summary>
		Queue<IRobotInstruction> Instructions { get; set; }
	}
}
