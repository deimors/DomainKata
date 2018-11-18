using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Kata.TicTacToe.Tests
{
	public class GameTests : GameTestFixture
	{
		[Fact]
		public void MarkFirstXInTopCorner()
		{
			Act_MarkX(0, 0);

			Assert_EventReceived(new XMarkedEvent(0, 0));
		}
	}

	public class XMarkedEvent : GameEvent
	{
		public XMarkedEvent(int x, int y)
		{
			throw new NotImplementedException();
		}
	}

	public abstract class GameTestFixture
	{
		private readonly Game _game = new Game();

		protected void Act_MarkX(int x, int y)
			=> _game.MarkX(x, y);

		protected void Assert_EventReceived(GameEvent gameEvent)
		{
			throw new NotImplementedException();
		}
	}

	public abstract class GameEvent
	{
	}

	public class Game
	{
		public void MarkX(int x, int y)
		{
			throw new NotImplementedException();
		}
	}
}
