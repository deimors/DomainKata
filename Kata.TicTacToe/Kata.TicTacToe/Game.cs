using System;
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
			=> Result
				.Create(IsMarkInsideBoard(x, y), () => Unit.Value, () => GameError.MarkOutsideBoard)
				.Bind(_ => _board[x, y].Match(some => Result.Failure<Unit, GameError>(GameError.SpaceAlreadyFilled), () => Result.Success<Unit, GameError>(Unit.Value)))
				.Bind(_ => Result.Create(_nextMark == mark, () => Unit.Value, () => GameError.OutOfOrderMark))
				.Do(_ => _board[x, y] = Option.Some(mark))
				.Do(_ => _nextMark = Successor(_nextMark))
				.Do(_ => _events.OnNext(markedEventFactory()));

		private static Mark Successor(Mark current)
			=> current == Mark.X ? Mark.O : Mark.X;

		private static bool IsMarkInsideBoard(int x, int y)
			=> x >= 0 && x <= 2 && y >= 0 && y <= 2;
	}
}