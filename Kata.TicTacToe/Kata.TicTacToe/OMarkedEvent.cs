using System;

namespace Kata.TicTacToe
{
	public class OMarkedEvent : GameEvent, IEquatable<OMarkedEvent>
	{
		public int X { get; }
		public int Y { get; }

		public OMarkedEvent(int x, int y)
		{
			X = x;
			Y = y;
		}

		public bool Equals(OMarkedEvent other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((OMarkedEvent) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X * 397) ^ Y;
			}
		}

		public static bool operator ==(OMarkedEvent left, OMarkedEvent right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(OMarkedEvent left, OMarkedEvent right)
		{
			return !Equals(left, right);
		}
	}
}