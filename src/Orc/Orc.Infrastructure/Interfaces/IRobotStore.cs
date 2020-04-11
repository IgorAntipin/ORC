using Orc.Common.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Abstraction to a robot's data storage
	/// </summary>
	public interface IRobotStore
	{
		/// <summary>
		/// Get current position
		/// </summary>
		/// <returns>coordinates</returns>
		Task<Vector2d> GetCurrentPositionAsync();

		/// <summary>
		/// Change current position
		/// </summary>
		/// <param name="absolute">coordinates</param>
		/// <returns></returns>
		Task UpdateCurrentPositionAsync(Vector2d absolute);

		/// <summary>
		/// Register cleaned coordinates
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		Task AddCleanedPointAsync(Vector2d point);

		/// <summary>
		/// Get the total count of cleaned points
		/// </summary>
		/// <returns></returns>
		Task<int> GetCleanedPointsCountAsync();
	}
}
