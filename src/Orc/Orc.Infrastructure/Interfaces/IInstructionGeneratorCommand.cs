using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Orc.Infrastructure.Interfaces
{
	public interface IInstructionGeneratorCommand : ICommand
	{
		TextReader Reader { get; set; }
	}
}
