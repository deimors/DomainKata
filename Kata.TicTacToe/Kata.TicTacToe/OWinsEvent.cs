using System;

namespace Kata.TicTacToe
{
	public class OWinsEvent : GameEvent, IEquatable<OWinsEvent>
	{
		public bool Equals(OWinsEvent other)
			=> !(other is null);

		public override bool Equals(object obj)
			=> Equals(obj as OWinsEvent);

		public override int GetHashCode()
			=> nameof(OWinsEvent).GetHashCode();

		public static bool operator ==(OWinsEvent left, OWinsEvent right) 
			=> Equals(left, right);

		public static bool operator !=(OWinsEvent left, OWinsEvent right) 
			=> !Equals(left, right);
	}
}