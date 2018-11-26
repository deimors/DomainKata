﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Functional;
using Unit = Functional.Unit;

namespace Kata.TicTacToe
{
	public class Game
	{
		private readonly ISubject<GameEvent> _events = new Subject<GameEvent>();
		
		private Mark _nextMark = Mark.X;

		private readonly Option<Mark>[,] _board = new Option<Mark>[3,3];

		public IObservable<GameEvent> Events => _events;

		public Result<Unit, GameError> MarkX(int x, int y)
			=> ApplyMark(x, y, Mark.X, () => new XMarkedEvent(x, y));

		public Result<Unit, GameError> MarkO(int x, int y)
			=> ApplyMark(x, y, Mark.O, () => new OMarkedEvent(x, y));

		private Result<Unit, GameError> ApplyMark(int x, int y, Mark mark, Func<GameEvent> markedEventFactory)
			=> FailIfOutsideBoard(x, y)
				.Bind(_ => FailIfSpaceAlreadyFilled(x, y))
				.Bind(_ => FailIfOutOfTurnOrder(mark))
				.Do(_ => _board[x, y] = Option.Some(mark))
				.Do(_ => _nextMark = Successor(_nextMark))
				.Do(_ => _events.OnNext(markedEventFactory()))
				.Do(_ => EventOnWin(mark));

		private Result<Unit, GameError> FailIfOutOfTurnOrder(Mark mark) 
			=> Result.Create(_nextMark == mark, () => Unit.Value, () => GameError.OutOfOrderMark);

		private Result<Unit, GameError> FailIfSpaceAlreadyFilled(int x, int y) 
			=> _board[x, y].Match(some => Result.Failure<Unit, GameError>(GameError.SpaceAlreadyFilled), () => Result.Success<Unit, GameError>(Unit.Value));

		private static Result<Unit, GameError> FailIfOutsideBoard(int x, int y) 
			=> Result.Create(IsMarkInsideBoard(x, y), () => Unit.Value, () => GameError.MarkOutsideBoard);

		private static Mark Successor(Mark current)
			=> current == Mark.X ? Mark.O : Mark.X;

		private static bool IsMarkInsideBoard(int x, int y)
			=> x >= 0 && x <= 2 && y >= 0 && y <= 2;

		private void EventOnWin(Mark mark)
		{
			if (TestForWin(mark))
				_events.OnNext(new XWinsEvent());
		}

		private bool TestForWin(Mark mark) 
			=> MarksToTest().All(markOption => markOption == Option.Some(mark));

		private IEnumerable<Option<Mark>> MarksToTest()
		{
			yield return _board[0, 0];
			yield return _board[0, 1];
			yield return _board[0, 2];
		}
	}
}