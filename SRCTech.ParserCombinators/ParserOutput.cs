using System;

namespace SRCTech.ParserCombinators
{
    public static class ParserOutput
    {
        public static IParserOutput<TState, TResult> FromValue<TState, TResult>(
            TState state,
            TResult value)
        {
            return new ValueParserOutput<TState, TResult>(state, value);
        }

        public static IParserOutput<TState, TResult> FromError<TState, TResult>(
            TState state,
            IParserError error)
        {
            return new ErrorParserOutput<TState, TResult>(state, error);
        }

        public static IParserOutput<TState, TResult> Select<TState, TSource, TResult>(
            this IParserOutput<TState, TSource> source,
            Func<TSource, TResult> selector)
        {
            if (source.HasValue)
            {
                return FromValue(source.State, selector(source.Value));
            }

            return CastError<TState, TSource, TResult>(source);
        }

        public static IParserOutput<TState, TResult> CastError<TState, TSource, TResult>(
            this IParserOutput<TState, TSource> source)
        {
            return FromError<TState, TResult>(source.State, source.Error);
        }

        public static bool TryGetValue<TState, TResult>(
            this IParserOutput<TState, TResult> source,
            out TResult result)
        {
            if (source.HasValue)
            {
                result = source.Value;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }    

        private sealed class ValueParserOutput<TState, TResult> : IParserOutput<TState, TResult>
        {
            public ValueParserOutput(TState state, TResult value)
            {
                State = state;
                Value = value;
            }

            public TState State { get; }

            public bool HasValue => true;

            public TResult Value { get; }

            public IParserError Error => throw new InvalidOperationException();
        }

        private sealed class ErrorParserOutput<TState, TResult> : IParserOutput<TState, TResult>
        {
            public ErrorParserOutput(TState state, IParserError error)
            {
                State = state;
                Error = error;
            }

            public TState State { get; }

            public bool HasValue => false;

            public TResult Value => throw new InvalidOperationException();

            public IParserError Error { get; }
        }
    }
}
