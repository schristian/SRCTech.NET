using System;

namespace SRCTech.Common.Functional
{
    public static partial class Option
    {
        public static IOption<TResult> Select<TSource, TResult>(
            this IOption<TSource> source,
            Func<TSource, TResult> selector)
        {
            return source.TryGetValue(out var sourceValue) ? 
                Some(selector(sourceValue)) : 
                None<TResult>();
        }

        public static IOption<TResult> SelectOption<TSource, TResult>(
            this IOption<TSource> source,
            Func<TSource, IOption<TResult>> selector)
        {
            return source.TryGetValue(out var sourceValue) ? 
                selector(sourceValue) : 
                None<TResult>();
        }

        public static IOption<TResult> SelectOption<TSource, TIntermediate, TResult>(
            this IOption<TSource> source,
            Func<TSource, IOption<TIntermediate>> intermediateSelector,
            Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return source.TryGetValue(out var sourceValue) &&
                intermediateSelector(sourceValue).TryGetValue(out var intermediateValue) ?
                Some(resultSelector(sourceValue, intermediateValue)) :
                None<TResult>();
        }

        public static IOption<TSource> Where<TSource>(
            this IOption<TSource> source,
            Func<TSource, bool> predicate)
        {
            return source.TryGetValue(out var sourceValue) && 
                predicate(sourceValue) ? 
                Some(sourceValue) :
                None<TSource>();
        }
    }
}
