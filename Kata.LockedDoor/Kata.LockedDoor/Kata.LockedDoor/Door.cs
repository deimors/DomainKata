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
			=> Result.Create(!_locked, Unit.Value, DoorError.CantOpenLockedDoor)
				.Do(_ => _events.OnNext(new DoorOpenedEvent()));

		public IDisposable Subscribe(IObserver<DoorOpenedEvent> observer) 
			=> _events.Subscribe(observer);

		public Result<Unit, DoorError> Unlock()
			=> Result.Create(_locked, Unit.Value, DoorError.DoorAlreadyUnlocked);
	}
}