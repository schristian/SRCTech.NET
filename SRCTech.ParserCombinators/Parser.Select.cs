using System;
using System.Threading.Tasks;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public static partial class Parser
    {
        public static IParser<TToken, TResult> Select<TToken, TSource, TResult>(
            this IParser<TToken, TSource> source,
            Func<TSource, TResult> selector)
        {
            return new SelectParser<TToken, TSource, TResult>(source, selector);
        }

        private sealed record SelectParser<TToken, TSource, TResult>(
            IParser<TToken, TSource> SourceParser,
            Func<TSource, TResult> Selector) : IParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return ParseInternal(input).ToAwaitable();
            }

            private async Task<IParserOutput<TState, TResult>> ParseInternal<TState>(
                IParserInput<TState, TToken> input)
            {
                var sourceOutput = await SourceParser.Parse(input);
                if (sourceOutput.TryGetValue(out var sourceValue))
                {
                    return ParserOutput.FromValue(sourceOutput.State, Selector(sourceValue));
                }

                return sourceOutput.CastError<TState, TSource, TResult>();
            }
        }
    }
}
