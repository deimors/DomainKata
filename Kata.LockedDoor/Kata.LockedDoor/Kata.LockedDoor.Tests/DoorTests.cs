using System;
using FakeItEasy;
using FluentAssertions;
using Functional;
using Xunit;

namespace Kata.LockedDoor.Tests
{
	public class DoorTests
	{
		private Door _door;
		private readonly IObserver<DoorOpenedEvent> _observer = A.Fake<IObserver<DoorOpenedEvent>>();

		[Fact]
		public void TryToOpenLockedDoor()
		{
			_door = new Door(locked: true);

			var result = _door.Open();

			result.Should().Be(Result.Failure<Unit, DoorError>(DoorError.CantOpenLockedDoor));
		}

		[Fact]
		public void TryToOpenUnlockedDoor()
		{
			_door = new Door(locked: false);

			_door.Subscribe(_observer);

			var result = _door.Open();

			result.Should().Be(Result.Success<Unit, DoorError>(Unit.Value));

			Assert_EventObserved(new DoorOpenedEvent());
		}

		private void Assert_EventObserved(DoorOpenedEvent expectedEvent)
		{
			A.CallTo(() => _observer.OnNext(expectedEvent)).MustHaveHappened();
		}
	}
}
