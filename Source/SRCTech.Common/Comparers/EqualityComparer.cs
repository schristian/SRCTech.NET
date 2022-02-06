using System;
using System.Collections.Generic;
using System.Linq;
using SRCTech.Common.Collections;

namespace SRCTech.Common.Comparers
{
    public static class EqualityComparer
    {
        public static IEqualityComparer<T> Create<T>(
            Func<T, T, bool> equalsFunc,
            Func<T, int> hashCodeFunc)
        {
            return new AnonymousEqualityComparer<T>(equalsFunc, hashCodeFunc);
        }

        public static IEqualityComparer<IEnumerable<T>> CreateCollectionComparer<T>(
            this IEqualityComparer<T> equalityComparer)
        {
            return Create<IEnumerable<T>>(
                (x, y) => Enumerable.SequenceEqual(x, y, equalityComparer),
                x => x.GetCollectionHashCode(equalityComparer));
        }

        public static IEqualityComparer<IEnumerable<T>> CreateUnorderedCollectionComparer<T>(
            this IEqualityComparer<T> equalityComparer)
        {
            return Create<IEnumerable<T>>(
                (x, y) => CollectionHelper.EqualsUnorderedCollection(x, y, equalityComparer),
                x => x.GetUnorderedCollectionHashCode(equalityComparer));
        }

        public static IEqualityComparer<TSource> Extend<TSource, TResult>(
            this IEqualityComparer<TResult> equalityComparer,
            Func<TSource, TResult> selector)
        {
            return Create<TSource>(
                (x, y) => equalityComparer.Equals(selector(x), selector(y)),
                x => equalityComparer.GetHashCode(selector(x)));
        }

        public static IEqualityComparer<T> Combine<T>(
            this IEnumerable<IEqualityComparer<T>> equalityComparers)
        {
            return Create<T>(
                (x, y) => equalityComparers.All(c => c.Equals(x, y)),
                x => equalityComparers.GetHashCode(x));
        }

        public static IEqualityComparer<T> Combine<T>(
            params IEqualityComparer<T>[] equalityComparers)
        {
            return Combine((IEnumerable<IEqualityComparer<T>>)equalityComparers);
        }

        private static int GetHashCode<T>(
            this IEnumerable<IEqualityComparer<T>> equalityComparers,
            T value)
        {
            return equalityComparers
                .Select(c => c.GetHashCode(value))
                .CombineHashCodes();
        }

        private sealed class AnonymousEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equalsFunc;
            private readonly Func<T, int> _hashCodeFunc;

            public AnonymousEqualityComparer(
                Func<T, T, bool> equalsFunc, 
                Func<T, int> hashCodeFunc)
            {
                _equalsFunc = equalsFunc;
                _hashCodeFunc = hashCodeFunc;
            }

            public bool Equals(T x, T y) => _equalsFunc(x, y);

            public int GetHashCode(T obj) => _hashCodeFunc(obj);
        }
    }
}
