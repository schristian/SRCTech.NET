using System;
using System.Threading.Tasks;

namespace SRCTech.Common.Async
{
    public static partial class Awaitable
    {
        public static IAwaitable<TResult> Select<TSource, TResult>(
            this IAwaitable<TSource> source,
            Func<TSource, TResult> selector)
        {
            return Create(async () => selector(await source));
        }
    }
}
