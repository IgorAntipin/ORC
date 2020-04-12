using Orc.Domain.Interfaces;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Orc.Infrastructure.Queries
{
	public abstract class InstructionGeneratorQuery<TResult> : IInstructionGeneratorQuery<TResult>
	{
		public Type ResultType { get { return typeof(TResult); } }

		public TextReader Reader { get; set; }

	}
}
