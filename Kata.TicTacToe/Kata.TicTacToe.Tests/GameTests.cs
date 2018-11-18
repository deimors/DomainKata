using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using FluentAssertions;
using Functional;
using Xunit;

namespace Kata.TicTacToe.Tests
{
	public class GameTests : GameTestFixture
	{
		[Fact]
		public void MarkFirstXInTopLeftCorner()
		{
			Act_MarkX(0, 0);

			Assert_EventObserved(new XMarkedEvent(0, 0));
		}

		[Fact]
		public void MarkFirstXInBottomRightCorner()
		{
			Act_MarkX(2, 2);

			Assert_EventObserved(new XMarkedEvent(2, 2));
		}

		[Theory]
		[InlineData(-1, 0), InlineData(3, 0), InlineData(0, -1), InlineData(0, 3)]
		public void MarkFirstXOutsideBoard(int x, int y)
		{
			Act_MarkX(x, y)
				.Assert_Failure(GameError.MarkOutsideBoard);
		}
	}

	public static class GameTestExtensions
	{
		public static void Assert_Failure(this Result<Unit, GameError> result, GameError expectedError)
			=> result.Should().BeEquivalentTo(Result.Failure<Unit, GameError>(expectedError));
	}

	public abstract class GameTestFixture
	{
		private readonly Game _game = new Game();
		private readonly IObserver<GameEvent> _eventObserver = A.Fake<IObserver<GameEvent>>();

		protected GameTestFixture()
		{
			_game.Events.Subscribe(_eventObserver);
		}

		protected Result<Unit, GameError> Act_MarkX(int x, int y)
			=> _game.MarkX(x, y);

		protected void Assert_EventObserved(GameEvent gameEvent)
			=> A.CallTo(() => _eventObserver.OnNext(gameEvent)).MustHaveHappened();
	}
}
