using Orc.Common.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface IControllerFacade
	{
		Task<Vector2d> GetCurrentPositionAsync();


		Task MoveToAsync(Vector2d relative);

		/// <summary>
		/// Perform cleaning procedures at the current position
		/// </summary>
		/// <returns></returns>
		Task CleanAsync();
	
	}
}
