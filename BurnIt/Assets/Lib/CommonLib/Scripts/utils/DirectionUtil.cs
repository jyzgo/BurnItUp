using UnityEngine;

namespace MTUnity.Utils
{
	//  这里的值不能修改
	public enum Direction
	{
		None = 0, Up = 1, Down = 2, Left = 3, Right = 4, LeftUp = 5, RightUp = 6, LeftDown = 7, RightDown = 8
	}

	public static class DirectionUtil
	{
		/// <summary>
		/// Shorthand for writing @@Vector2(0, 0)@@.
		/// </summary>
		public static Vector2 v2None { get { return new Vector2 (0, 0); } }

		/// <summary>
		/// Shorthand for writing @@Vector2(-1, 1)@@.
		/// </summary>
		public static Vector2 v2LeftUp { get { return new Vector2 (-1, 1); } }

		/// <summary>
		/// Shorthand for writing @@Vector2(1, 1)@@.
		/// </summary>
		public static Vector2 v2RightUp { get { return new Vector2 (1, 1); } }

		/// <summary>
		/// Shorthand for writing @@Vector2(-1, -1)@@.
		/// </summary>
		public static Vector2 v2LeftDown { get { return new Vector2 (-1, -1); } }

		/// <summary>
		/// Shorthand for writing @@Vector2(1, -1)@@.
		/// </summary>
		public static Vector2 v2RightDown { get { return new Vector2 (1, -1); } }

		public static Vector2 dirToV2 (Direction dir)
		{
			switch (dir) {
			case Direction.Up:
				return Vector2.up;
			case Direction.Down:
				return Vector2.down;
			case Direction.Left:
				return Vector2.left;
			case Direction.Right:
				return Vector2.right;
			case Direction.LeftUp:
				return v2LeftUp;
			case Direction.RightUp:
				return v2RightUp;
			case Direction.LeftDown:
				return v2LeftDown;
			case Direction.RightDown:
				return v2RightDown;
			}
			return v2None;
		}

		public static Direction v2ToDir (Vector2 v)
		{
			if (v == Vector2.up) {
				return Direction.Up;
			} else if (v == Vector2.down) {
				return Direction.Down;
			} else if (v == Vector2.left) {
				return Direction.Left;
			} else if (v == Vector2.right) {
				return Direction.Right;
			} else if (v == v2LeftUp) {
				return Direction.LeftUp;
			} else if (v == v2RightUp) {
				return Direction.RightUp;
			} else if (v == v2LeftDown) {
				return Direction.LeftDown;
			} else if (v == v2RightDown) {
				return Direction.RightDown;
			}
			return Direction.None;
		}

		public static Direction intToDir (int dir)
		{
			switch (dir) {
			case 1:
				return Direction.Up;
			case 2:
				return Direction.Down;
			case 3:
				return Direction.Left;
			case 4:
				return Direction.Right;
			case 5:
				return Direction.LeftUp;
			case 6:
				return Direction.RightUp;
			case 7:
				return Direction.LeftDown;
			case 8:
				return Direction.RightDown;
			}
			return Direction.None;
		}

		public static Vector2 intToV2 (int dir)
		{
			return dirToV2 (intToDir (dir));
		}

		public static int dirToInt (Direction dir)
		{
			return (int)dir;
		}

		public static int v2ToInt (Vector2 v)
		{
			return (int)v2ToDir (v);
		}

		public static bool isSameLine (Direction dir1, Direction dir2)
		{
            if (dir1 == dir2 || dir1 == GetOppositeDiretion(dir2)) {
                return true;
            }
			return false;
        }

        public static Direction GetOppositeDiretion (Direction dir)
        {
            if (dir == Direction.Left) {
                return Direction.Right;
            } else if (dir == Direction.Right) {
                return Direction.Left;
            } else if (dir == Direction.Up) {
                return Direction.Down;
            } else if (dir == Direction.Down) {
                return Direction.Up;
            } else if (dir == Direction.LeftUp) {
                return Direction.RightDown;
            } else if (dir == Direction.LeftDown) {
                return Direction.RightUp;
            } else if (dir == Direction.RightDown) {
                return Direction.LeftUp;
            } else if (dir == Direction.RightUp) {
                return Direction.LeftDown;
            }
            return Direction.None;
        }
	}
}
