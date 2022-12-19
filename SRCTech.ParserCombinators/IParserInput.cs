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

        IAwaitable<IParserOutput<TState, TResult>> CombineOutputSequence<T1, T2, TResult>(
            IParserOutput<TState, T1> first,
            IParserOutput<TState, T2> second,
            Func<T1, T2, TResult> resultSelector);

        IAwaitable<IParserOutput<TState, TResult>> CombineOutputAlternatives<TResult>(
            IReadOnlyCollection<IParserOutput<TState, TResult>> alternatives);
        
        IAwaitable<IParserOutput<TState, TToken>> Advance();

        IAwaitable<IParserOutput<TState, TResult>> Peek<TResult>(
            IParser<TToken, TResult> parser);

        IAwaitable<IParserOutput<TState, TResult>> Try<TResult>(
            IParser<TToken, TResult> parser);
    }
}
