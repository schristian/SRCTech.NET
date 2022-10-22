using System;
using System.Threading.Tasks;
using SRCTech.Common.Functional;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TResult> Then<TSource, TResult>(
            this ITextParser<TSource> first,
            ITextParser<TResult> second)
        {
            return first.Then(_ => second);
        }

        public static ITextParser<TResult> Then<TSource, TResult>(
            this ITextParser<TSource> first,
            Func<TSource, ITextParser<TResult>> secondSelector)
        {
            return first.SelectMany(secondSelector);
        }

        public static ITextParser<TResult> Then<TSource, TIntermediate, TResult>(
            this ITextParser<TSource> first,
            ITextParser<TIntermediate> second,
            Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return first.Then(_ => second, resultSelector);
        }

        public static ITextParser<TResult> Then<TSource, TIntermediate, TResult>(
            this ITextParser<TSource> first,
            Func<TSource, ITextParser<TIntermediate>> secondSelector,
            Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return first.SelectMany(secondSelector, resultSelector);
        }

        public static ITextParser<TSource> ThenIgnore<TSource, TResult>(
            this ITextParser<TSource> first,
            ITextParser<TResult> second)
        {
            return first.ThenIgnore(_ => second);
        }

        public static ITextParser<TSource> ThenIgnore<TSource, TResult>(
            this ITextParser<TSource> first,
            Func<TSource, ITextParser<TResult>> secondSelector)
        {
            return first.SelectMany(secondSelector, (result, _) => result);
        }
    }
}
