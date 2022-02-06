using System;
using System.Collections.Generic;
using System.Linq;
using SRCTech.Common.Collections;

namespace SRCTech.Common.Comparers
{
    public static class Comparer
    {
        public static IComparer<T> Create<T>(Func<T, T, int> compareFunc)
        {
            return new AnonymousComparer<T>(compareFunc);
        }

        public static IComparer<IEnumerable<T>> CreateCollectionComparer<T>(
            this IComparer<T> comparer)
        {
            return Create<IEnumerable<T>>(
                (x, y) => CollectionHelper.CompareToCollection(x, y, comparer));
        }

        public static IComparer<TSource> Extend<TSource, TResult>(
            this IComparer<TResult> comparer,
            Func<TSource, TResult> selector)
        {
            return Create<TSource>(
                (x, y) => comparer.Compare(selector(x), selector(y)));
        }

        public static IComparer<T> Combine<T>(
            this IEnumerable<IComparer<T>> comparers)
        {
            return Create<T>(
                (x, y) => comparers.Select(c => c.Compare(x, y)).FirstOrDefault(r => r != 0));
        }

        public static IComparer<T> Combine<T>(
            params IComparer<T>[] comparers)
        {
            return Combine((IEnumerable<IComparer<T>>)comparers);
        }

        public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
        {
            return Create<T>((x, y) => -comparer.Compare(x, y));
        }

        private class AnonymousComparer<T> : IComparer<T>
        {
            private readonly Func<T, T, int> _compareFunc;

            public AnonymousComparer(Func<T, T, int> compareFunc)
            {
                _compareFunc = compareFunc;
            }

            public int Compare(T x, T y) => _compareFunc(x, y);
        }
    }
}
