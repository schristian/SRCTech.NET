using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public sealed class StringParserInput : IParserInput<StringParserInputSpan, char>
    {
        private static readonly IParserError EndOfStringError = null;

        public StringParserInput(string str)
        {
            String = str;
            CurrentPosition = 0;
            CurrentItem = default;
        }

        public string String { get; }

        public int CurrentPosition { get; private set; }

        public char CurrentItem { get; private set; }

        public IAwaitable<IParserOutput<StringParserInputSpan, TResult>> CreateValueOutput<TResult>(
            TResult value)
        {
            var span = new StringParserInputSpan(String, CurrentPosition, CurrentPosition);
            return CreateValueOutput(span, value);
        }

        public IAwaitable<IParserOutput<StringParserInputSpan, TResult>> CreateErrorOutput<TResult>(
            IParserError error)
        {
            var span = new StringParserInputSpan(String, CurrentPosition, CurrentPosition);
            return CreateErrorOutput<TResult>(span, error);
        }

        public IAwaitable<IParserOutput<StringParserInputSpan, TResult>> CombineOutputSequence<T1, T2, TResult>(
            IParserOutput<StringParserInputSpan, T1> first,
            IParserOutput<StringParserInputSpan, T2> second,
            Func<T1, T2, TResult> resultSelector)
        {
            var span = new StringParserInputSpan(String, first.State.StartPosition, second.State.EndPosition);

            if (!first.TryGetValue(out var firstValue))
            {
                return CreateErrorOutput<TResult>(span, first.Error);
            }

            if (!second.TryGetValue(out var secondValue))
            {
                return CreateErrorOutput<TResult>(span, second.Error);
            }

            return CreateValueOutput(span, resultSelector(firstValue, secondValue));
        }

        public IAwaitable<IParserOutput<StringParserInputSpan, TResult>> CombineOutputAlternatives<TResult>(
            IReadOnlyCollection<IParserOutput<StringParserInputSpan, TResult>> alternatives)
        {
            throw new NotImplementedException();
        }

        public IAwaitable<IParserOutput<StringParserInputSpan, char>> Advance()
        {
            var startPosition = CurrentPosition;
            if (startPosition >= String.Length)
            {
                var errorSpan = new StringParserInputSpan(String, startPosition, startPosition);
                return CreateErrorOutput<char>(errorSpan, EndOfStringError);
            }

            CurrentPosition += 1;
            CurrentItem = String[startPosition];

            var span = new StringParserInputSpan(String, startPosition, CurrentPosition);
            return CreateValueOutput(span, CurrentItem);
        }

        public IAwaitable<IParserOutput<StringParserInputSpan, TResult>> Peek<TResult>(IParser<char, TResult> parser)
        {
            throw new NotImplementedException();
        }

        public IAwaitable<IParserOutput<StringParserInputSpan, TResult>> Try<TResult>(IParser<char, TResult> parser)
        {
            throw new NotImplementedException();
        }

        private static IAwaitable<IParserOutput<StringParserInputSpan, TResult>> CreateValueOutput<TResult>(
            StringParserInputSpan span,
            TResult value)
        {
            return Awaitable.FromResult(ParserOutput.FromValue(span, value));
        }

        private static IAwaitable<IParserOutput<StringParserInputSpan, TResult>> CreateErrorOutput<TResult>(
            StringParserInputSpan span,
            IParserError error)
        {
            return Awaitable.FromResult(ParserOutput.FromError<StringParserInputSpan, TResult>(span, error));
        }
    }
}
