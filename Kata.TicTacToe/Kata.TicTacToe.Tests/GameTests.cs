using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using Xunit;

namespace Kata.TicTacToe.Tests
{
	public class GameTests : GameTestFixture
	{
		[Fact]
		public void MarkFirstXInTopCorner()
		{
			Act_MarkX(0, 0);

			Assert_EventObserved(new XMarkedEvent(0, 0));
		}
	}

	public abstract class GameTestFixture
	{
		private readonly Game _game = new Game();
		private readonly IObserver<GameEvent> _eventObserver = A.Fake<IObserver<GameEvent>>();

		protected GameTestFixture()
		{
			_game.Events.Subscribe(_eventObserver);
		}

		protected void Act_MarkX(int x, int y)
			=> _game.MarkX(x, y);

		protected void Assert_EventObserved(GameEvent gameEvent)
			=> A.CallTo(() => _eventObserver.OnNext(gameEvent)).MustHaveHappened();
	}
}
