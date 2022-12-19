using System.Collections.Generic;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public static partial class Parser
    {
        public static IEnumerableParser<TToken, TResult> ZeroOrMore<TToken, TResult>(
            this IParser<TToken, TResult> parser)
        {
            return new SeveralParser<TToken, TResult>(parser, 0, int.MaxValue);
        }

        public static IEnumerableParser<TToken, TResult> OneOrMore<TToken, TResult>(
            this IParser<TToken, TResult> parser)
        {
            return new SeveralParser<TToken, TResult>(parser, 1, int.MaxValue);
        }

        public static IEnumerableParser<TToken, TResult> Several<TToken, TResult>(
            this IParser<TToken, TResult> parser,
            int occurrences)
        {
            return new SeveralParser<TToken, TResult>(parser, occurrences, occurrences);
        }

        public static IEnumerableParser<TToken, TResult> Several<TToken, TResult>(
            this IParser<TToken, TResult> parser,
            int minOccurrences,
            int maxOccurrences)
        {
            return new SeveralParser<TToken, TResult>(parser, minOccurrences, maxOccurrences);
        }

        public static IEnumerableParser<TToken, TResult> StrictZeroOrMore<TToken, TResult>(
            this IParser<TToken, TResult> parser)
        {
            return new StrictSeveralParser<TToken, TResult>(parser, 0, int.MaxValue);
        }

        public static IEnumerableParser<TToken, TResult> StrictOneOrMore<TToken, TResult>(
            this IParser<TToken, TResult> parser)
        {
            return new StrictSeveralParser<TToken, TResult>(parser, 1, int.MaxValue);
        }

        public static IEnumerableParser<TToken, TResult> StrictSeveral<TToken, TResult>(
            this IParser<TToken, TResult> parser,
            int occurrences)
        {
            return new StrictSeveralParser<TToken, TResult>(parser, occurrences, occurrences);
        }

        public static IEnumerableParser<TToken, TResult> StrictSeveral<TToken, TResult>(
            this IParser<TToken, TResult> parser,
            int minOccurrences,
            int maxOccurrences)
        {
            return new StrictSeveralParser<TToken, TResult>(parser, minOccurrences, maxOccurrences);
        }

        private sealed record SeveralParser<TToken, TResult>(
            IParser<TToken, TResult> InnerParser,
            int MinOccurrences,
            int MaxOccurrences) : IEnumerableParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, IReadOnlyCollection<TResult>>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return CombineOutputSequence(input, ParseMany(input)).ToAwaitable();
            }

            public async IAsyncEnumerable<IParserOutput<TState, TResult>> ParseMany<TState>(
                IParserInput<TState, TToken> input)
            {
                int count = 0;
                while (count < MinOccurrences)
                {
                    var result = await InnerParser.Parse(input);
                    if (result.HasValue)
                    {
                        yield return result;
                        count += 1;
                    }
                    else
                    {
                        yield return result;
                        yield break;
                    }
                }

                while (count < MaxOccurrences)
                {
                    var result = await input.Try(InnerParser);
                    if (result.HasValue)
                    {
                        yield return result;
                        count += 1;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }

        private sealed record StrictSeveralParser<TToken, TResult>(
            IParser<TToken, TResult> InnerParser,
            int MinOccurrences,
            int MaxOccurrences) : IEnumerableParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, IReadOnlyCollection<TResult>>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return CombineOutputSequence(input, ParseMany(input)).ToAwaitable();
            }

            public async IAsyncEnumerable<IParserOutput<TState, TResult>> ParseMany<TState>(
                IParserInput<TState, TToken> input)
            {
                int count = 0;
                while (count < MinOccurrences)
                {
                    var result = await InnerParser.Parse(input);
                    if (result.HasValue)
                    {
                        yield return result;
                        count += 1;
                    }
                    else
                    {
                        yield return result;
                        yield break;
                    }
                }

                while (count < MaxOccurrences)
                {
                    var startPosition = input.CurrentPosition;
                    var result = await InnerParser.Parse(input);
                    if (result.HasValue)
                    {
                        yield return result;
                        count += 1;
                    }
                    else if (input.CurrentPosition != startPosition)
                    {
                        yield return result;
                        yield break;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
