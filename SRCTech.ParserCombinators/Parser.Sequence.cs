using System.Collections.Generic;
using System.Linq;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public static partial class Parser
    {
        public static IEnumerableParser<TToken, TResult> Sequence<TToken, TResult>(
            IEnumerable<IParser<TToken, TResult>> parsers)
        {
            return new SequenceParser<TToken, TResult>(parsers.ToList());
        }

        public static IEnumerableParser<TToken, TResult> Sequence<TToken, TResult>(
            params IParser<TToken, TResult>[] parsers)
        {
            return new SequenceParser<TToken, TResult>(parsers);
        }

        private sealed record SequenceParser<TToken, TResult>(
            IReadOnlyList<IParser<TToken, TResult>> InnerParsers) : IEnumerableParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, IReadOnlyCollection<TResult>>> Parse<TState>(IParserInput<TState, TToken> input)
            {
                throw new System.NotImplementedException();
            }

            public IEnumerableParserOutput<TState, TResult> ParseMany<TState>(IParserInput<TState, TToken> input)
            {
                return input.Generate()
            }
        }
    }
}
