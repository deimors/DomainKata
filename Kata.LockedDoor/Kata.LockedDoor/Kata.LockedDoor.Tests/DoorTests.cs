using System;
using FakeItEasy;
using FluentAssertions;
using Functional;
using Xunit;

namespace Kata.LockedDoor.Tests
{
	public class DoorTestFixture
	{
		protected readonly Door _door;
		protected readonly IObserver<DoorOpenedEvent> _observer = A.Fake<IObserver<DoorOpenedEvent>>();

		protected DoorTestFixture(Door door)
		{
			_door = door;
			_door.Subscribe(_observer);
		}

		protected void Assert_EventNotObserved<T>() where T : DoorOpenedEvent
		{
			A.CallTo(() => _observer.OnNext(A<T>._)).MustNotHaveHappened();
		}

		protected void Assert_EventObserved(DoorOpenedEvent expectedEvent)
		{
			A.CallTo(() => _observer.OnNext(expectedEvent)).MustHaveHappened();
		}
	}

	public class LockedDoorTests : DoorTestFixture
	{
		public LockedDoorTests() : base(new Door(locked: true)) {}

		[Fact]
		public void TryToOpenLockedDoor()
		{
			var result = _door.Open();

			result.Should().Be(Result.Failure<Unit, DoorError>(DoorError.CantOpenLockedDoor));

			Assert_EventNotObserved<DoorOpenedEvent>();
		}

		[Fact]
		public void UnlockLockedDoor()
		{
			var result = _door.Unlock();

			result.Should().Be(Result.Success<Unit, DoorError>(Unit.Value));
		}
	}

	public class UnlockedDoorTests : DoorTestFixture
	{
		public UnlockedDoorTests() : base(new Door(locked: false)) {}

		[Fact]
		public void TryToOpenUnlockedDoor()
		{
			var result = _door.Open();

			result.Should().Be(Result.Success<Unit, DoorError>(Unit.Value));

			Assert_EventObserved(new DoorOpenedEvent());
		}

		[Fact]
		public void UnlockUnlockedDoor()
		{
			var result = _door.Unlock();

			result.Should().Be(Result.Failure<Unit, DoorError>(DoorError.DoorAlreadyUnlocked));
		}
	}
}
