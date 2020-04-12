using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Orc.Domain.Interfaces
{
	public interface IReportCommand : ICommand
	{
		TextWriter TextWriter { set; get; }
	}
}
