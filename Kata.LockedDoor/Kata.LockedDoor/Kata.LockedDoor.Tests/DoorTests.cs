using System;
using FakeItEasy;
using FluentAssertions;
using Functional;
using Xunit;

namespace Kata.LockedDoor.Tests
{
	public class DoorTestFixture
	{
		private readonly Door _door;
		private readonly IObserver<DoorOpenedEvent> _observer = A.Fake<IObserver<DoorOpenedEvent>>();

		protected DoorTestFixture(Door door)
		{
			_door = door;
			_door.Subscribe(_observer);
		}

		protected void Assert_EventNotObserved<T>() where T : DoorOpenedEvent 
			=> A.CallTo(() => _observer.OnNext(A<T>._))
				.MustNotHaveHappened();

		protected void Assert_EventObserved(DoorOpenedEvent expectedEvent) 
			=> A.CallTo(() => _observer.OnNext(expectedEvent))
				.MustHaveHappened();

		protected Result<Unit, DoorError> Act_Open() 
			=> _door.Open();

		protected Result<Unit, DoorError> Act_Unlock() 
			=> _door.Unlock();
	}

	public class LockedDoorTests : DoorTestFixture
	{
		public LockedDoorTests() : base(new Door(locked: true)) {}

		[Fact]
		public void TryToOpenLockedDoor()
		{
			Act_Open()
				.Assert_Failure(DoorError.CantOpenLockedDoor);
			
			Assert_EventNotObserved<DoorOpenedEvent>();
		}

		[Fact]
		public void UnlockLockedDoor()
		{
			Act_Unlock()
				.Assert_Success();
		}
	}

	public class UnlockedDoorTests : DoorTestFixture
	{
		public UnlockedDoorTests() : base(new Door(locked: false)) {}

		[Fact]
		public void TryToOpenUnlockedDoor()
		{
			Act_Open()
				.Assert_Success();

			Assert_EventObserved(new DoorOpenedEvent());
		}

		[Fact]
		public void UnlockUnlockedDoor()
		{
			Act_Unlock()
				.Assert_Failure(DoorError.DoorAlreadyUnlocked);
		}
	}

	public static class DoorTestExtensions
	{
		public static void Assert_Success(this Result<Unit, DoorError> result)
			=> result.Should().Be(Result.Success<Unit, DoorError>(Unit.Value));

		public static void Assert_Failure(this Result<Unit, DoorError> result, DoorError expected)
			=> result.Should().Be(Result.Failure<Unit, DoorError>(expected));
	}
}
