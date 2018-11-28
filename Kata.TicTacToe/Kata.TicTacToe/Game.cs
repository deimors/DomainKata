using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Functional;
using Unit = Functional.Unit;

namespace Kata.TicTacToe
{
	public class Game
	{
		private const int Size = 3;

		private readonly ISubject<GameEvent> _events = new Subject<GameEvent>();
		
		private Mark _nextMark = Mark.X;

		private readonly Option<Mark>[,] _board = new Option<Mark>[Size, Size];

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
				.Do(_ => _nextMark = Successor(mark))
				.Do(_ => _events.OnNext(markedEventFactory()))
				.Do(_ => EventOnWin(mark))
				.Do(_ => EventOnDraw());

		private Result<Unit, GameError> FailIfOutOfTurnOrder(Mark mark) 
			=> Result.Create(_nextMark == mark, () => Unit.Value, () => GameError.OutOfOrderMark);

		private Result<Unit, GameError> FailIfSpaceAlreadyFilled(int x, int y) 
			=> _board[x, y].Match(some => Result.Failure<Unit, GameError>(GameError.SpaceAlreadyFilled), () => Result.Success<Unit, GameError>(Unit.Value));

		private static Result<Unit, GameError> FailIfOutsideBoard(int x, int y) 
			=> Result.Create(IsMarkInsideBoard(x, y), () => Unit.Value, () => GameError.MarkOutsideBoard);

		private static Mark Successor(Mark current)
			=> current == Mark.X ? Mark.O : Mark.X;

		private static bool IsMarkInsideBoard(int x, int y)
			=> x >= 0 && x < Size && y >= 0 && y < Size;

		private void EventOnWin(Mark mark)
		{
			if (GameWonBy(mark))
				_events.OnNext(GetWinEvent(mark));
		}

		private void EventOnDraw()
		{
			if (HorizontalSequences.All(sequence => sequence.All(space => space.Match(someMark => true, () => false))))
				_events.OnNext(new DrawEvent());
		}

		private bool GameWonBy(Mark mark) 
			=> Sequences.Any(sequence => SequenceFilledByMark(mark, sequence));

		private static bool SequenceFilledByMark(Mark mark, IEnumerable<Option<Mark>> sequence) 
			=> sequence.All(markOption => markOption == Option.Some(mark));

		private IEnumerable<IEnumerable<Option<Mark>>> Sequences 
			=> HorizontalSequences.Concat(VerticalSequences).Append(FirstDiagonal).Append(SecondDiagonal);

		private IEnumerable<IEnumerable<Option<Mark>>> HorizontalSequences
			=> SequenceRange.Select(x => SequenceRange.Select(y => _board[x, y]));

		private IEnumerable<IEnumerable<Option<Mark>>> VerticalSequences
			=> SequenceRange.Select(y => SequenceRange.Select(x => _board[x, y]));

		private IEnumerable<Option<Mark>> FirstDiagonal
			=> SequenceRange.Select(x =>  _board[x, x]);

		private IEnumerable<Option<Mark>> SecondDiagonal
			=> SequenceRange.Select(x => _board[Size - 1 - x, x]);

		private static IEnumerable<int> SequenceRange => Enumerable.Range(0, Size);

		private static GameEvent GetWinEvent(Mark mark)
			=> mark == Mark.X ? (GameEvent)new XWinsEvent() : new OWinsEvent();
	}
}