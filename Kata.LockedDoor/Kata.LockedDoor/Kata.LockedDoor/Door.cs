using Functional;

namespace Kata.LockedDoor
{
	public class Door
	{
		private readonly bool _locked;

		public Door(bool locked)
		{
			_locked = locked;
		}

		public Result<Unit, DoorError> Open()
		{
			return Result.Create(!_locked, Unit.Value, DoorError.CantOpenLockedDoor);
		}
	}
}