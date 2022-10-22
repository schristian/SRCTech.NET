using System;
using System.Collections.Generic;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<IReadOnlyList<TSource>> Sequence<TSource>(
            IEnumerable<ITextParser<TSource>> parsers)
        {
            return Create(
                async i =>
                {
                    var items = new List<TSource>();
                    foreach (var parser in parsers)
                    {
                        var result = await parser.Parse(i);
                        if (result.HasValue)
                        {
                            items.Add(result.Value);
                        }
                        else
                        {
                            return result.CastError<IReadOnlyList<TSource>>();
                        }
                    }

                    return new TextParserResult<IReadOnlyList<TSource>>(items);
                });
        }

        public static ITextParser<IReadOnlyList<TSource>> Sequence<TSource>(
            params ITextParser<TSource>[] parsers)
        {
            return Sequence((IEnumerable<ITextParser<TSource>>)parsers);
        }
    }
}
