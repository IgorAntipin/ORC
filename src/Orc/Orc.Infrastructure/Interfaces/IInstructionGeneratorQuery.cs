using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Orc.Infrastructure.Interfaces
{
	public interface IInstructionGeneratorQuery <out TResult> : IQuery<TResult>
	{
		TextReader Reader { get; set; }
	}
}
