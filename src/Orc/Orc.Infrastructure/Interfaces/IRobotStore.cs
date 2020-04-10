using Orc.Common.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface IRobotStore
	{
		Task<Vector2d> GetCurrentPositionAsync();
		Task UpdateCurrentPositionAsync(Vector2d absolute);

		Task AddCleanedPointAsync(Vector2d point);

		Task<int> GetCleanedPointsCountAsync();
	}
}
