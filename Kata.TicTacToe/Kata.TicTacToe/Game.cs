using System;
using System.Reactive.Subjects;

namespace Kata.TicTacToe
{
	public class Game
	{
		private readonly ISubject<GameEvent> _events = new Subject<GameEvent>();

		public IObservable<GameEvent> Events => _events;

		public void MarkX(int x, int y)
		{
			_events.OnNext(new XMarkedEvent(x, y));
		}
	}
}