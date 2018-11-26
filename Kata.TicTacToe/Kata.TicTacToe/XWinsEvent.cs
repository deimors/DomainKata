using System;

namespace Kata.TicTacToe
{
	public class XWinsEvent : GameEvent, IEquatable<XWinsEvent>
	{
		public bool Equals(XWinsEvent other)
			=> !(other is null);

		public override bool Equals(object obj)
			=> Equals(obj as XWinsEvent);

		public override int GetHashCode()
			=> nameof(XWinsEvent).GetHashCode();

		public static bool operator ==(XWinsEvent left, XWinsEvent right) 
			=> Equals(left, right);

		public static bool operator !=(XWinsEvent left, XWinsEvent right) 
			=> !Equals(left, right);
	}
}