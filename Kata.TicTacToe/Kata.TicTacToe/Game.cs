using System;
using System.Reactive.Subjects;
using Functional;
using Unit = Functional.Unit;

namespace Kata.TicTacToe
{
	public class Game
	{
		private readonly ISubject<GameEvent> _events = new Subject<GameEvent>();

		private bool _isXTurn = true;

		public IObservable<GameEvent> Events => _events;

		public Result<Unit, GameError> MarkX(int x, int y)
			=> Mark(x, y, true, () => new XMarkedEvent(x, y));

		public Result<Unit, GameError> MarkO(int x, int y)
			=> Mark(x, y, false, () => new OMarkedEvent(x, y));

		private Result<Unit, GameError> Mark(int x, int y, bool isXMark, Func<GameEvent> markedEventFactory)
			=> Result
				.Create(IsMarkInsideBoard(x, y), () => Unit.Value, () => GameError.MarkOutsideBoard)
				.Bind(_ => Result.Create(_isXTurn == isXMark, () => Unit.Value, () => GameError.OutOfOrderMark))
				.Do(_ => _isXTurn = !_isXTurn)
				.Do(_ => _events.OnNext(markedEventFactory()));

		private static bool IsMarkInsideBoard(int x, int y)
			=> x >= 0 && x <= 2 && y >= 0 && y <= 2;
	}
}