using System;

namespace SRCTech.ParserCombinators.TextParsers
{
    public struct TextParserResult<T>
    {
        private readonly ITextParserError _error;

        private readonly T _value;

        public TextParserResult(T value)
        {
            _value = value;
            _error = default;
        }

        public TextParserResult(ITextParserError error)
        {
            _value = default;
            _error = error;
        }

        public T Value => HasValue ? _value : throw new InvalidOperationException();

        public ITextParserError Error => !HasValue ? _error : throw new InvalidOperationException();

        public bool HasValue => _error is null;

        public TextParserResult<TResult> CastError<TResult>()
        {
            if (HasValue)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return new TextParserResult<TResult>(Error);
            }
        }

        public TextParserResult<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            if (HasValue)
            {
                return new TextParserResult<TResult>(selector(Value));
            }
            else
            {
                return new TextParserResult<TResult>(Error);
            }
        }

        public TextParserPeekResult<T> ToPeekResult(
            Func<TextParserResult<T>, bool> rollBackSelector)
        {
            return new TextParserPeekResult<T>(this, rollBackSelector(this));
        }
    }
}
