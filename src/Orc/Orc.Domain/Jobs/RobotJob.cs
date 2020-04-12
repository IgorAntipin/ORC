using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Domain.Jobs
{
	public class RobotJob : IRobotJob
	{
		public Guid Id { get; set; }
		public Queue<IInstruction> Instructions { get; set; }
	}
}
