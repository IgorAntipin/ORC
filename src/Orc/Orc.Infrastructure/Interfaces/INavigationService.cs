using Orc.Common.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	/// <summary>
	/// Define methods for checking the path
	/// </summary>
	public interface INavigationService
	{

		/// <summary>
		/// Checks if there is a valid path between source and destination coordinates
		/// and it is possible to reach the destination point
		/// </summary>
		/// <param name="src">from point</param>
		/// <param name="dest">to point</param>
		/// <returns>true if destination is reachable</returns>
		Task<bool> CheckPathAsync(Vector2d src, Vector2d dest);
	}
}
