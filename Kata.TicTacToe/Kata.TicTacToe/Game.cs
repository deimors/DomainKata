using System;
using System.Reactive.Subjects;
using Functional;
using Unit = Functional.Unit;

namespace Kata.TicTacToe
{
	public class Game
	{
		private readonly ISubject<GameEvent> _events = new Subject<GameEvent>();

		public IObservable<GameEvent> Events => _events;

		public Result<Unit, GameError> MarkX(int x, int y) 
			=> Result
				.Create(IsMarkInsideBoard(x, y), () => Unit.Value, () => GameError.MarkOutsideBoard)
				.Do(_ => _events.OnNext(new XMarkedEvent(x, y)));

		private static bool IsMarkInsideBoard(int x, int y)
			=> x >= 0 && x <= 2 && y >= 0 && y <= 2;
	}
}