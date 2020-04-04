using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface IReportProcessor
	{
		Task ProcessAsync(IRobotReport report);
	}
}
