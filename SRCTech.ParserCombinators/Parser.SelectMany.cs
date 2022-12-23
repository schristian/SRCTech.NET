using System;
using System.Threading.Tasks;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public static partial class Parser
    {
        public static IParser<TToken, TResult> SelectMany<TToken, TSource, TResult>(
            this IParser<TToken, TSource> source,
            Func<TSource, IParser<TToken, TResult>> selector)
        {
            Func<TSource, TResult, TResult> resultSelector = static (_, result) => result;

            return new SelectManyParser<TToken, TSource, TResult, TResult>(source, selector, resultSelector);
        }

        public static IParser<TToken, TResult> SelectMany<TToken, TSource, TIntermediate, TResult>(
            this IParser<TToken, TSource> source,
            Func<TSource, IParser<TToken, TIntermediate>> intermediateSelector,
            Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return new SelectManyParser<TToken, TSource, TIntermediate, TResult>(source, intermediateSelector, resultSelector);
        }

        private sealed record SelectManyParser<TToken, TSource, TIntermediate, TResult>(
            IParser<TToken, TSource> SourceParser,
            Func<TSource, IParser<TToken, TIntermediate>> IntermediateSelector,
            Func<TSource, TIntermediate, TResult> ResultSelector) : IParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return input.Then(SourceParser, IntermediateSelector, ResultSelector);
            }
        }
    }
}
