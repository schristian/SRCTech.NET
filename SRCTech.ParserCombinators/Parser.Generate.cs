using System;
using System.Collections.Generic;
using SRCTech.Common.Async;
using SRCTech.Common.Functional;

namespace SRCTech.ParserCombinators
{
    public static partial class Parser
    {
        public static IEnumerableParser<TToken, TResult> Generate<TToken, TSource, TResult>(
            TResult seed,
            Func<TResult, IOption<IParser<TToken, TSource>>> parserSelector,
            Func<TResult, TSource, TResult> resultSelector)
        {
            return new GenerateParser<TToken, TSource, TResult>(seed, parserSelector, resultSelector);
        }

        private sealed record GenerateParser<TToken, TSource, TResult>(
            TResult Seed,
            Func<TResult, IOption<IParser<TToken, TSource>>> ParserSelector,
            Func<TResult, TSource, TResult> ResultSelector) : IEnumerableParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, IReadOnlyCollection<TResult>>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return CombineOutputSequence(input, ParseMany(input)).ToAwaitable();
            }

            public async IAsyncEnumerable<IParserOutput<TState, TResult>> ParseMany<TState>(
                IParserInput<TState, TToken> input)
            {
                var currentValue = Seed;
                while (true)
                {
                    var parserOption = ParserSelector(currentValue);
                    if (!parserOption.TryGetValue(out var parser))
                    {
                        yield break;
                    }

                    var output = await parser.Parse(input);
                    if (!output.TryGetValue(out var sourceValue))
                    {
                        yield return output.CastError<TState, TSource, TResult>();
                        yield break;
                    }

                    currentValue = ResultSelector(currentValue, sourceValue);
                    yield return ParserOutput.FromValue(output.State, currentValue);
                }
            }
        }
    }
}
