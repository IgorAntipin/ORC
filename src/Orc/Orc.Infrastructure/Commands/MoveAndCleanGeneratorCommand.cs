using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure.Commands
{
	public class MoveAndCleanGeneratorCommand : InspructionGeneratorCommand
	{
		public int Count { get; set; }
		public Queue<IInstruction> Queue { get; set; }
	}
}
