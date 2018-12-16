using System;
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

			result.Should().Be(Result.Fail<Unit, DoorError>(DoorError.CantOpenLockedDoor));
		}
	}
}
