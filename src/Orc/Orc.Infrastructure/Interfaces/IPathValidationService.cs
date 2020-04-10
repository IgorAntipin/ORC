using Orc.Common.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orc.Infrastructure.Interfaces
{
	public interface IPathValidationService
	{
		Task<bool> CheckMoveAsync(Vector2d src, Vector2d dest);
	}
}
