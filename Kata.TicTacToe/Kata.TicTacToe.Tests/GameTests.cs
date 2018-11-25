﻿using FakeItEasy;
using FluentAssertions;
using Functional;
using System;
using Xunit;

namespace Kata.TicTacToe.Tests
{
	public class GameTests : GameTestFixture
	{
		[Theory]
		[InlineData(-1, 0), InlineData(3, 0), InlineData(0, -1), InlineData(0, 3)]
		public void MarkFirstXOutsideBoard(int x, int y)
		{
			Act_MarkX(x, y)
				.Assert_Failure(GameError.MarkOutsideBoard);

			Assert_EventNotObserved<XMarkedEvent>();
		}

		[Theory]
		[InlineData(0, 0), InlineData(1, 1), InlineData(2, 2), InlineData(0, 2), InlineData(2, 0)]
		public void MarkFirstXInsideBoard(int x, int y)
		{
			Act_MarkX(x, y)
				.Assert_Success();

			Assert_EventObserved(new XMarkedEvent(x, y));
		}
	}

	public static class GameTestExtensions
	{
		public static void Assert_Failure(this Result<Unit, GameError> result, GameError expectedError)
			=> result.Should().BeEquivalentTo(Result.Failure<Unit, GameError>(expectedError));

		public static void Assert_Success(this Result<Unit, GameError> result)
			=> result.Should().BeEquivalentTo(Result.Success<Unit, GameError>(Unit.Value));
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

		protected void Assert_EventNotObserved<TEvent>() where TEvent : GameEvent
			=> A.CallTo(() => _eventObserver.OnNext(A<GameEvent>.That.Matches(e => e is TEvent))).MustNotHaveHappened();
	}
}
