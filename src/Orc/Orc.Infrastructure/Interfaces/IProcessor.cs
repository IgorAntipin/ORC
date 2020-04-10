using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Interface for a processor that can execute commands and queries
	/// </summary>
	public interface IProcessor : ICommandProcessor, IQueryProcessor
	{
	}
}
