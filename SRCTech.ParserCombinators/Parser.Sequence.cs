using System.Collections.Generic;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public static partial class Parser
    {
        public static IEnumerableParser<TToken, TResult> Sequence<TToken, TResult>(
            IEnumerable<IParser<TToken, TResult>> parsers)
        {
            return new SequenceParser<TToken, TResult>(parsers);
        }

        public static IEnumerableParser<TToken, TResult> Sequence<TToken, TResult>(
            params IParser<TToken, TResult>[] parsers)
        {
            return new SequenceParser<TToken, TResult>(parsers);
        }

        private sealed record SequenceParser<TToken, TResult>(
            IEnumerable<IParser<TToken, TResult>> InnerParsers) : IEnumerableParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, IReadOnlyCollection<TResult>>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return CombineOutputSequence(input, ParseMany(input)).ToAwaitable();
            }

            public async IAsyncEnumerable<IParserOutput<TState, TResult>> ParseMany<TState>(
                IParserInput<TState, TToken> input)
            {
                foreach (var parser in InnerParsers)
                {
                    var result = await parser.Parse(input);
                    yield return result;

                    if (!result.HasValue)
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
