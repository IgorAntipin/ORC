using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Orc.Infrastructure.Commands
{
	public abstract class InspructionGeneratorCommand : IInstructionGeneratorCommand
	{
		public TextReader Reader { get; set; }		 
	}
}
