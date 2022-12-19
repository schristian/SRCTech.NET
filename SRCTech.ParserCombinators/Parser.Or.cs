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
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return ParseInternal(input).ToAwaitable();
            }

            private async Task<IParserOutput<TState, TResult>> ParseInternal<TState>(
                IParserInput<TState, TToken> input)
            {
                List<IParserOutput<TState, TResult>> results = null;

                foreach (var parser in InnerParsers)
                {
                    var result = await input.Try(parser);
                    results ??= new List<IParserOutput<TState, TResult>>();
                    results.Add(result);

                    if (result.HasValue)
                    {
                        break;
                    }
                }

                return await input.CombineOutputAlternatives(results);
            }
        }

        private sealed record StrictOrParser<TToken, TResult>(
            IReadOnlyCollection<IParser<TToken, TResult>> InnerParsers) : IParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return ParseInternal(input).ToAwaitable();
            }

            private async Task<IParserOutput<TState, TResult>> ParseInternal<TState>(
                IParserInput<TState, TToken> input)
            {
                var startPosition = input.CurrentPosition;
                List<IParserOutput<TState, TResult>> results = null;

                foreach (var parser in InnerParsers)
                {
                    var result = await parser.Parse(input);
                    results ??= new List<IParserOutput<TState, TResult>>();
                    results.Add(result);

                    if (result.HasValue || input.CurrentPosition != startPosition)
                    {
                        break;
                    }
                }

                return await input.CombineOutputAlternatives(results);
            }
        }
    }
}
