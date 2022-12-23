using System;
using System.Collections.Generic;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public interface IParserInput<TState, out TToken>
    {
        int CurrentPosition { get; }

        TToken CurrentItem { get; }

        IAwaitable<IParserOutput<TState, TResult>> CreateValueOutput<TResult>(TResult value);

        IAwaitable<IParserOutput<TState, TResult>> CreateErrorOutput<TResult>(IParserError error);

        IAwaitable<IParserOutput<TState, TToken>> Advance();

        IAwaitable<IParserOutput<TState, TResult>> Peek<TResult>(
            IParser<TToken, TResult> parser);

        IAwaitable<IParserOutput<TState, TResult>> Try<TResult>(
            IParser<TToken, TResult> parser);

        IAwaitable<IParserOutput<TState, TResult>> Then<TSource, TIntermediate, TResult>(
            IParser<TToken, TSource> parser,
            Func<TSource, IParser<TToken, TIntermediate>> intermediateSelector,
            Func<TSource, TIntermediate, TResult> resultSelector);

        IAwaitable<IParserOutput<TState, TResult>> Or<TResult>(
            IReadOnlyCollection<IParser<TToken, TResult>> parsers);
    }
}
