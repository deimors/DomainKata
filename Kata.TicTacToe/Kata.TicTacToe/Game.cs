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
			=> Result
				.Create(IsMarkInsideBoard(x, y), () => Unit.Value, () => GameError.MarkOutsideBoard)
				.Do(_ => _isXTurn = false)
				.Do(_ => _events.OnNext(new XMarkedEvent(x, y)));

		public Result<Unit, GameError> MarkO(int x, int y) 
			=> Result.Create(!_isXTurn, () => Unit.Value, () => GameError.OutOfOrderMark);

		private static bool IsMarkInsideBoard(int x, int y)
			=> x >= 0 && x <= 2 && y >= 0 && y <= 2;
	}
}