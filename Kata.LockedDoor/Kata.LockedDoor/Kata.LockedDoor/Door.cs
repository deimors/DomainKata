using System;
using System.Reactive.Subjects;
using Functional;

namespace Kata.LockedDoor
{
	public class Door : IObservable<DoorOpenedEvent>
	{
		private readonly bool _locked;
		private readonly Subject<DoorOpenedEvent> _events = new Subject<DoorOpenedEvent>();

		public Door(bool locked)
		{
			_locked = locked;
		}

		public Result<Unit, DoorError> Open()
		{
			_events.OnNext(new DoorOpenedEvent());

			return Result.Create(!_locked, Unit.Value, DoorError.CantOpenLockedDoor);
		}

		public IDisposable Subscribe(IObserver<DoorOpenedEvent> observer)
		{
			return _events.Subscribe(observer);
		}
	}
}