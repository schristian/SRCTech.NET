using System.Collections.Generic;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<IReadOnlyList<TSource>> ZeroOrMore<TSource>(
            this ITextParser<TSource> parser)
        {
            return parser.SeveralImpl(0, int.MaxValue);
        }

        public static ITextParser<IReadOnlyList<TSource>> OneOrMore<TSource>(
            this ITextParser<TSource> parser)
        {
            return parser.SeveralImpl(1, int.MaxValue);
        }

        public static ITextParser<IReadOnlyList<TSource>> Several<TSource>(
            this ITextParser<TSource> parser,
            int occurrences)
        {
            return parser.SeveralImpl(occurrences, occurrences);
        }

        public static ITextParser<IReadOnlyList<TSource>> Several<TSource>(
            this ITextParser<TSource> parser,
            int minOccurrences,
            int maxOccurrences)
        {
            return parser.SeveralImpl(minOccurrences, maxOccurrences);
        }

        public static ITextParser<IReadOnlyList<TSource>> StrictZeroOrMore<TSource>(
            this ITextParser<TSource> parser)
        {
            return parser.StrictSeveralImpl(0, int.MaxValue);
        }

        public static ITextParser<IReadOnlyList<TSource>> StrictOneOrMore<TSource>(
            this ITextParser<TSource> parser)
        {
            return parser.StrictSeveralImpl(1, int.MaxValue);
        }

        public static ITextParser<IReadOnlyList<TSource>> StrictSeveral<TSource>(
            this ITextParser<TSource> parser,
            int occurrences)
        {
            return parser.StrictSeveralImpl(occurrences, occurrences);
        }

        public static ITextParser<IReadOnlyList<TSource>> StrictSeveral<TSource>(
            this ITextParser<TSource> parser,
            int minOccurrences,
            int maxOccurrences)
        {
            return parser.StrictSeveralImpl(minOccurrences, maxOccurrences);
        }

        private static ITextParser<IReadOnlyList<TSource>> SeveralImpl<TSource>(
            this ITextParser<TSource> parser,
            int minOccurrences,
            int maxOccurrences)
        {
            return Create(
               async i =>
               {
                   var items = new List<TSource>();
                   while (items.Count < minOccurrences)
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

                   while (items.Count < maxOccurrences)
                   {
                       var result = await parser.TryParse(i);
                       if (result.HasValue)
                       {
                           items.Add(result.Value);
                       }
                       else
                       {
                           return new TextParserResult<IReadOnlyList<TSource>>(items);
                       }
                   }

                   return new TextParserResult<IReadOnlyList<TSource>>(items);
               });
        }

        private static ITextParser<IReadOnlyList<TSource>> StrictSeveralImpl<TSource>(
            this ITextParser<TSource> parser,
            int minOccurrences,
            int maxOccurrences)
        {
            return Create(
               async i =>
               {
                   var items = new List<TSource>();
                   while (items.Count < minOccurrences)
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

                   while (items.Count < maxOccurrences)
                   {
                       int startPosition = i.CurrentPosition;
                       var result = await parser.Parse(i);
                       if (result.HasValue)
                       {
                           items.Add(result.Value);
                       }
                       else if (i.CurrentPosition != startPosition)
                       {
                           return result.CastError<IReadOnlyList<TSource>>();
                       }
                       else
                       {
                           return new TextParserResult<IReadOnlyList<TSource>>(items);
                       }
                   }

                   return new TextParserResult<IReadOnlyList<TSource>>(items);
               });
        }
    }
}
