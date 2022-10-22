using System;
using System.Collections.Generic;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TResult> Select<TSource, TResult>(
            this ITextParser<TSource> source,
            Func<TSource, TResult> selector)
        {
            return Create(async i => (await source.Parse(i)).Select(selector));
        }
    }
}
