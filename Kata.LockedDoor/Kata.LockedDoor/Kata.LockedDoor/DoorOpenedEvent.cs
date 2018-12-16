using System;

namespace Kata.LockedDoor
{
	public class DoorOpenedEvent : IEquatable<DoorOpenedEvent>
	{
		public bool Equals(DoorOpenedEvent other)
			=> !(other is null);

		public override bool Equals(object obj)
			=> Equals(obj as DoorOpenedEvent);

		public override int GetHashCode()
			=> nameof(DoorOpenedEvent).GetHashCode();

		public static bool operator ==(DoorOpenedEvent left, DoorOpenedEvent right) 
			=> Equals(left, right);

		public static bool operator !=(DoorOpenedEvent left, DoorOpenedEvent right) 
			=> !Equals(left, right);
	}
}