using Orc.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface IJobStore
	{
		Task AddJobAsync(IRobotJob job);

		Task<IRobotJob> GetNextJobAsync();

	}
}
