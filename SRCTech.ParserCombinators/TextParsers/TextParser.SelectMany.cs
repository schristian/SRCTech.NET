using System;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TResult> SelectMany<TSource, TResult>(
            this ITextParser<TSource> source,
            Func<TSource, ITextParser<TResult>> selector)
        {
            return Create(
                async i =>
                {
                    var sourceResult = await source.Parse(i);
                    if (sourceResult.HasValue)
                    {
                        return await selector(sourceResult.Value).Parse(i);
                    }

                    return sourceResult.CastError<TResult>();
                });
        }

        public static ITextParser<TResult> SelectMany<TSource, TIntermediate, TResult>(
            this ITextParser<TSource> source,
            Func<TSource, ITextParser<TIntermediate>> intermediateSelector,
            Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return Create(
                async i =>
                {
                    var sourceResult = await source.Parse(i);
                    if (sourceResult.HasValue)
                    {
                        var intermediateResult = await intermediateSelector(sourceResult.Value).Parse(i);
                        if (intermediateResult.HasValue)
                        {
                            return new TextParserResult<TResult>(resultSelector(sourceResult.Value, intermediateResult.Value));
                        }

                        return intermediateResult.CastError<TResult>();
                    }

                    return sourceResult.CastError<TResult>();
                });
        }
    }
}
