using Orc.Common.Types;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Services
{
	/// <summary>
	/// A pretty good navigation
	/// </summary>
	public class SimpleNavigationService : INavigationService
	{
		/// <summary>
		/// Can find a path 100%
		/// </summary>
		/// <param name="src">from point</param>
		/// <param name="dest">to point</param>
		/// <returns>always true</returns>
		public async Task<bool> CheckPathAsync(Vector2d src, Vector2d dest)
		{
			await Task.CompletedTask;
			return true;
		}
	}
}
