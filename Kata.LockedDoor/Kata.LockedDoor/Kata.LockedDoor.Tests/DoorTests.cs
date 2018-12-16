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
			var door = new Door(initiallyLocked: true);

			var result = door.Open();

			result.Should().Be(Result.Failure<Unit, DoorError>(DoorError.CantOpenLockedDoor));
		}

		[Fact]
		public void TryToOpenUnlockedDoor()
		{
			var door = new Door(initiallyLocked: false);

			var result = door.Open();

			result.Should().Be(Result.Success<Unit, DoorError>(Unit.Value));
		}
	}

	public enum DoorError
	{
		CantOpenLockedDoor
	}

	public class Door
	{
		public Door(bool initiallyLocked)
		{
			
		}

		public Result<Unit, DoorError> Open()
		{
			return Result.Failure<Unit, DoorError>(DoorError.CantOpenLockedDoor);
		}
	}
}
