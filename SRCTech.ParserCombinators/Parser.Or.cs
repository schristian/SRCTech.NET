using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public static partial class Parser
    {
        public static IParser<TToken, TResult> Or<TToken, TResult>(
            IReadOnlyCollection<IParser<TToken, TResult>> parsers)
        {
            return new OrParser<TToken, TResult>(parsers.Select(Try).ToArray());
        }

        public static IParser<TToken, TResult> Or<TToken, TResult>(
            params IParser<TToken, TResult>[] parsers)
        {
            return new OrParser<TToken, TResult>(parsers.Select(Try).ToArray());
        }

        public static IParser<TToken, TResult> StrictOr<TToken, TResult>(
            IReadOnlyCollection<IParser<TToken, TResult>> parsers)
        {
            return new StrictOrParser<TToken, TResult>(parsers);
        }

        public static IParser<TToken, TResult> StrictOr<TToken, TResult>(
            params IParser<TToken, TResult>[] parsers)
        {
            return new StrictOrParser<TToken, TResult>(parsers);
        }

        private sealed record OrParser<TToken, TResult>(
            IReadOnlyCollection<IParser<TToken, TResult>> InnerParsers) : IParser<TToken, TResult>
        {
            public IReadOnlyCollection<IParser<TToken, TResult>> TryInnerParsers { get; } =
                InnerParsers.Select(innerParser => innerParser.Try()).ToList();

            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return input.Or(TryInnerParsers);
            }
        }

        private sealed record StrictOrParser<TToken, TResult>(
            IReadOnlyCollection<IParser<TToken, TResult>> InnerParsers) : IParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return input.Or(InnerParsers);
            }
        }
    }
}
