using System;
using System.Collections.Generic;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TResult> Generate<TAccumulate, TResult>(
            TAccumulate seed,
            Func<TAccumulate, ITextParser<TAccumulate>> accumulateSelector,
            Func<TAccumulate, ITextParser<TResult>> resultSelector)
        {
            return Create(
                async i =>
                {
                    var accumulate = seed;

                    while (true)
                    {
                        var endResult = await resultSelector(accumulate).TryParse(i);
                        if (endResult.HasValue)
                        {
                            return endResult;
                        }

                        var accumulateResult = await accumulateSelector(accumulate).Parse(i);
                        if (accumulateResult.HasValue)
                        {
                            accumulate = accumulateResult.Value;
                        }
                        else
                        {
                            return accumulateResult.CastError<TResult>();
                        }
                    }
                });
        }

        public static ITextParser<IReadOnlyList<TSource>> Collection<TSource, TDelim, TEnd>(
            this ITextParser<TSource> sourceParser,
            ITextParser<TDelim> delimiterParser,
            ITextParser<TEnd> endParser)
        {
            return Create(
                async i =>
                {
                    var items = new List<TSource>();

                    {
                        var result = await endParser.TryParse(i);
                        if (result.HasValue)
                        {
                            return new TextParserResult<IReadOnlyList<TSource>>(items);
                        }
                    }

                    while (true)
                    {
                        {
                            var result = await sourceParser.Parse(i);
                            if (result.HasValue)
                            {
                                items.Add(result.Value);
                            }
                            else
                            {
                                return result.CastError<IReadOnlyList<TSource>>();
                            }
                        }

                        {
                            var result = await endParser.TryParse(i);
                            if (result.HasValue)
                            {
                                return new TextParserResult<IReadOnlyList<TSource>>(items);
                            }
                        }

                        {
                            var result = await delimiterParser.Parse(i);
                            if (!result.HasValue)
                            {
                                return result.CastError<IReadOnlyList<TSource>>();
                            }
                        }
                    }
                });
        }

        public static ITextParser<IReadOnlyList<TSource>> Collection<TSource, TStart, TDelim, TEnd>(
            this ITextParser<TSource> sourceParser,
            ITextParser<TStart> startParser,
            ITextParser<TDelim> delimiterParser,
            ITextParser<TEnd> endParser)
        {
            return startParser.Then(Generate(
                new List<TSource>(),
                list => (list.Count == 0 ? sourceParser : delimiterParser.Then(sourceParser))
                    .Select(item => { list.Add(item); return list; }),
                list => endParser.Select(_ => (IReadOnlyList<TSource>)list)));
        }
    }
}
