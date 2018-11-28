using System;

namespace Kata.TicTacToe
{
	public class DrawEvent : GameEvent, IEquatable<DrawEvent>
	{
		public bool Equals(DrawEvent other)
			=> !(other is null);

		public override bool Equals(object obj)
			=> Equals(obj as DrawEvent);

		public override int GetHashCode()
			=> nameof(DrawEvent).GetHashCode();

		public static bool operator ==(DrawEvent left, DrawEvent right) 
			=> Equals(left, right);

		public static bool operator !=(DrawEvent left, DrawEvent right) 
			=> !Equals(left, right);
	}
}