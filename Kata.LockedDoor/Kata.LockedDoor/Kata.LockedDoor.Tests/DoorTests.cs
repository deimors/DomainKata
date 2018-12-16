using System;
using FluentAssertions;
using Functional;
using Xunit;

namespace Kata.LockedDoor.Tests
{
	public class DoorTests
	{
		[Fact]
		public void TryToOpenLockedDoor()
		{
			var door = new Door(locked: true);

			var result = door.Open();

			result.Should().Be(Result.Failure<Unit, DoorError>(DoorError.CantOpenLockedDoor));
		}

		[Fact]
		public void TryToOpenUnlockedDoor()
		{
			var door = new Door(locked: false);

			var result = door.Open();

			result.Should().Be(Result.Success<Unit, DoorError>(Unit.Value));

			Assert_EventObserved(new DoorOpenedEvent());
		}

		private void Assert_EventObserved(DoorOpenedEvent doorEvent)
		{
			throw new NotImplementedException();
		}
	}

	public class DoorOpenedEvent
	{
	}
}
