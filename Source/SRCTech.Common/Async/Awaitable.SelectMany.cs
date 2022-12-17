using System;
using System.Threading.Tasks;

namespace SRCTech.Common.Async
{
    public static partial class Awaitable
    {
        public static IAwaitable<TResult> SelectMany<TSource, TResult>(
            this IAwaitable<TSource> source,
            Func<TSource, ValueTask<TResult>> selector)
        {
            return Create(async () => await selector(await source));
        }

        public static IAwaitable<TResult> SelectMany<TSource, TResult>(
            this IAwaitable<TSource> source,
            Func<TSource, Task<TResult>> selector)
        {
            return Create(async () => await selector(await source));
        }

        public static IAwaitable<TResult> SelectMany<TSource, TResult>(
            this IAwaitable<TSource> source,
            Func<TSource, IAwaitable<TResult>> selector)
        {
            return Create(async () => await selector(await source));
        }

        public static IAwaitable<TResult> SelectMany<TSource, TIntermediate, TResult>(
            this IAwaitable<TSource> source,
            Func<TSource, ValueTask<TIntermediate>> intermediateSelector,
            Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return Create(
                async () =>
                {
                    var sourceItem = await source;
                    var intermediateItem = await intermediateSelector(sourceItem);
                    return resultSelector(sourceItem, intermediateItem);
                });
        }

        public static IAwaitable<TResult> SelectMany<TSource, TIntermediate, TResult>(
            this IAwaitable<TSource> source,
            Func<TSource, Task<TIntermediate>> intermediateSelector,
            Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return Create(
                async () =>
                {
                    var sourceItem = await source;
                    var intermediateItem = await intermediateSelector(sourceItem);
                    return resultSelector(sourceItem, intermediateItem);
                });
        }

        public static IAwaitable<TResult> SelectMany<TSource, TIntermediate, TResult>(
            this IAwaitable<TSource> source,
            Func<TSource, IAwaitable<TIntermediate>> intermediateSelector,
            Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return Create(
                async () =>
                {
                    var sourceItem = await source;
                    var intermediateItem = await intermediateSelector(sourceItem);
                    return resultSelector(sourceItem, intermediateItem);
                });
        }
    }
}
