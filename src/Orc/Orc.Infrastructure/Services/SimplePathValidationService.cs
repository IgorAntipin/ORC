using Orc.Common.Types;
using Orc.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Services
{
	/// <summary>
	/// Always true path validation service
	/// </summary>
	public class SimplePathValidationService : IPathValidationService
	{
		public async Task<bool> CheckMoveAsync(Vector2d src, Vector2d dest)
		{
			await Task.CompletedTask;
			return true;
		}
	}
}
