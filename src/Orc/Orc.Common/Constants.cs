using Orc.Common.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Common
{
	public static class Constants
	{

		/// <summary>
		/// The maximum number of commands a robot can execute in one task
		/// </summary>
		public const int MAX_COMMANDS = 10000;

		/// <summary>
		/// The maximum number of steps a robot can take in one direction
		/// </summary>
		public const int MAX_STEPS = 100000;

		public const int WORLD_X_LIMIT = 100000;
		public const int WORLD_Y_LIMIT = 100000;

		public const int WORLD_X_MIN = -1 * WORLD_X_LIMIT;
		public const int WORLD_X_MAX = 1 * WORLD_X_LIMIT;

		public const int WORLD_Y_MIN = -1 * WORLD_Y_LIMIT;
		public const int WORLD_Y_MAX = 1 * WORLD_Y_LIMIT;


	}
}
