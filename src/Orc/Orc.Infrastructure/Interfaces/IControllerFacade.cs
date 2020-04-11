using Orc.Common.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface IControllerFacade
	{
		/// <summary>
		/// Get the absolute coordinates of the current position of the robot
		/// </summary>
		/// <returns></returns>
		Task<Vector2d> GetCurrentPositionAsync();

		/// <summary>
		/// Move to specified relative coordinates
		/// </summary>
		/// <param name="relative"></param>
		/// <returns></returns>
		Task MoveToAsync(Vector2d relative);

		/// <summary>
		/// Perform cleaning procedures at the current position
		/// </summary>
		/// <returns></returns>
		Task CleanAsync();
	
	}
}
