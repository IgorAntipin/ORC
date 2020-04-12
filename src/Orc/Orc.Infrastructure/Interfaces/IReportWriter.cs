using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface IReportWriter
	{
		void UseTextWriter(TextWriter textWriter);
		Task WriteAsync(IRobotResponse report);
	}
}
