using FakeItEasy;
using FluentAssertions;
using Functional;
using System;
using System.Linq;
using FakeItEasy.Configuration;
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

		[Fact]
		public void MarkOFirst()
		{
			Act_MarkO(0, 0)
				.Assert_Failure(GameError.OutOfOrderMark);
		}

		[Fact]
		public void MarkXThenO()
		{
			Act_MarkX(0, 0);

			Act_MarkO(2, 2)
				.Assert_Success();

			Assert_EventsObservedInOrder(
				new XMarkedEvent(0, 0),
				new OMarkedEvent(2, 2)
			);
		}

		[Fact]
		public void MarkXThenX()
		{
			Act_MarkX(0, 0);

			Act_MarkX(2, 2)
				.Assert_Failure(GameError.OutOfOrderMark);
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

		protected Result<Unit, GameError> Act_MarkO(int x, int y)
			=> _game.MarkO(x, y);

		protected void Assert_EventObserved(GameEvent gameEvent)
			=> A.CallTo(() => _eventObserver.OnNext(gameEvent)).MustHaveHappened();

		protected void Assert_EventsObservedInOrder(params GameEvent[] gameEvents)
			=> gameEvents.Skip(1)
				.Aggregate(
					A.CallTo(() => _eventObserver.OnNext(gameEvents.First())).MustHaveHappened() as IOrderableCallAssertion,
					(prev, nextEvent) => prev.Then(A.CallTo(() => _eventObserver.OnNext(nextEvent)).MustHaveHappened())
				);

		protected void Assert_EventNotObserved<TEvent>() where TEvent : GameEvent
			=> A.CallTo(() => _eventObserver.OnNext(A<GameEvent>.That.Matches(e => e is TEvent))).MustNotHaveHappened();	
	}
}
