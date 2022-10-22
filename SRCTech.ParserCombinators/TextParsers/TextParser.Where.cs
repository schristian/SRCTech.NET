using System;
using System.Collections.Generic;
using System.Linq;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TSource> Where<TSource>(
            this ITextParser<TSource> source,
            Func<TSource, bool> predicate)
        {
            return source.Where(predicate, "input that satisfies the predicate");
        }

        public static ITextParser<TSource> Where<TSource>(
            this ITextParser<TSource> source,
            Func<TSource, bool> predicate,
            string expectation)
        {
            return Create(
                async i =>
                {
                    var sourceResult = await source.Parse(i);

                    if (sourceResult.HasValue && !predicate(sourceResult.Value))
                    {
                        return new TextParserResult<TSource>(TextParserError.Create(i, expectation));
                    }

                    return sourceResult;
                });
        }

        public static ITextParser<TSource> WhereOneOf<TSource>(
            this ITextParser<TSource> source,
            IEnumerable<TSource> sourceItems)
        {
            return source.WhereOneOfImpl(sourceItems.ToHashSet());
        }

        public static ITextParser<TSource> WhereOneOf<TSource>(
            this ITextParser<TSource> source,
            IEnumerable<TSource> sourceItems,
            IEqualityComparer<TSource> equalityComparer)
        {
            return source.WhereOneOfImpl(sourceItems.ToHashSet(equalityComparer));
        }

        public static ITextParser<TSource> WhereOneOf<TSource>(
            this ITextParser<TSource> source,
            params TSource[] sourceItems)
        {
            return source.WhereOneOfImpl(sourceItems.ToHashSet());
        }

        public static ITextParser<TSource> WhereOneOf<TSource>(
            this ITextParser<TSource> source,
            IEqualityComparer<TSource> equalityComparer,
            params TSource[] sourceItems)
        {
            return source.WhereOneOfImpl(sourceItems.ToHashSet(equalityComparer));
        }

        public static ITextParser<TSource> WhereNotOneOf<TSource>(
            this ITextParser<TSource> source,
            IEnumerable<TSource> sourceItems)
        {
            return source.WhereNotOneOfImpl(sourceItems.ToHashSet());
        }

        public static ITextParser<TSource> WhereNotOneOf<TSource>(
            this ITextParser<TSource> source,
            IEnumerable<TSource> sourceItems,
            IEqualityComparer<TSource> equalityComparer)
        {
            return source.WhereNotOneOfImpl(sourceItems.ToHashSet(equalityComparer));
        }

        public static ITextParser<TSource> WhereNotOneOf<TSource>(
            this ITextParser<TSource> source,
            params TSource[] sourceItems)
        {
            return source.WhereNotOneOfImpl(sourceItems.ToHashSet());
        }

        public static ITextParser<TSource> WhereNotOneOf<TSource>(
            this ITextParser<TSource> source,
            IEqualityComparer<TSource> equalityComparer,
            params TSource[] sourceItems)
        {
            return source.WhereNotOneOfImpl(sourceItems.ToHashSet(equalityComparer));
        }

        private static ITextParser<TSource> WhereOneOfImpl<TSource>(
            this ITextParser<TSource> source,
            HashSet<TSource> sourceItems)
        {
            return source.Where(x => sourceItems.Contains(x));
        }

        private static ITextParser<TSource> WhereNotOneOfImpl<TSource>(
            this ITextParser<TSource> source,
            HashSet<TSource> sourceItems)
        {
            return source.Where(x => !sourceItems.Contains(x));
        }
    }
}
