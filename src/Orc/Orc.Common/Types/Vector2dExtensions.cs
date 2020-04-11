using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Common.Types
{
	public readonly partial struct Vector2d
	{
		public static Vector2d operator +(Vector2d a, Vector2d b)
		{
			unchecked
			{
				return new Vector2d(a.X + b.X, a.Y + b.Y);
			}			
		}

		public static Vector2d operator -(Vector2d a, Vector2d b)
		{
			unchecked
			{
				return new Vector2d(a.X - b.X, a.Y - b.Y);
			}
		}

		/// <summary>
		/// 0 : 0
		/// </summary>
		public static readonly Vector2d ZERO = new Vector2d(0, 0);

		/// <summary>
		/// 0 : 1
		/// </summary>
		public static readonly Vector2d NORTH = new Vector2d(0, 1);

		/// <summary>
		/// 0 : -1
		/// </summary>
		public static readonly Vector2d SOUTH = new Vector2d(0, -1);
		
		/// <summary>
		/// 1 : 0
		/// </summary>
		public static readonly Vector2d EAST = new Vector2d(1, 0);

		/// <summary>
		/// -1 : 0
		/// </summary>
		public static readonly Vector2d WEST = new Vector2d(-1, 0);
	}
}
