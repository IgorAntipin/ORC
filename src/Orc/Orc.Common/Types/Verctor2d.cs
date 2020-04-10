using System;
using System.Collections.Generic;
using System.Text;

namespace Orc.Common.Types
{
	public readonly partial struct Vector2d : IEquatable<Vector2d>
	{
		public Vector2d(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Vector2d(Vector2d src)
		{
			X = src.X;
			Y = src.Y;
		}

		public readonly int X;
		public readonly int Y;

		public override int GetHashCode()
		{
			unchecked
			{
				//long hash = 2;
				//hash ^= X + 2654435769 + (hash << 6) + (hash >> 2);
				//hash ^= Y + 2654435769 + (hash << 6) + (hash >> 2);

				//int h = (int)hash ^ (int)(hash >> 32);
				//return h;

				int hash = 2166113;
				hash = hash * 16777619 ^ X;
				hash = hash * 16777619 ^ Y;

				return hash;
				
			}
		}

		public override string ToString()
		{
			return $"{X}:{Y}";
		}

		public bool Equals(Vector2d other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			if(obj is Vector2d)
			{
				return Equals((Vector2d)obj);
			}
			return false;			
		}
	}
}
