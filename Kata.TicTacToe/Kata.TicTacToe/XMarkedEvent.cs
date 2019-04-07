using System;

namespace Kata.TicTacToe
{
	public class XMarkedEvent : GameEvent, IEquatable<XMarkedEvent>
	{
		public int X { get; }
		public int Y { get; }

		public XMarkedEvent(int x, int y)
		{
			X = x;
			Y = y;
		}

		public bool Equals(XMarkedEvent other)
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
			return Equals((XMarkedEvent) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X * 397) ^ Y;
			}
		}

		public static bool operator ==(XMarkedEvent left, XMarkedEvent right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(XMarkedEvent left, XMarkedEvent right)
		{
			return !Equals(left, right);
		}
	}
}