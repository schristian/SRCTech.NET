using System;
using System.Collections.Generic;
using System.Linq;

namespace SRCTech.Common.Collections
{
    public static class CollectionHelper
    {
        public static int CompareToCollection<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second)
        {
            return CompareToCollection(first, second, Comparer<T>.Default);
        }

        public static int CompareToCollection<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            IComparer<T> comparer)
        {
            Guard.ThrowIfNull(first, nameof(first));
            Guard.ThrowIfNull(second, nameof(second));
            Guard.ThrowIfNull(comparer, nameof(comparer));

            using (var firstEnumerator = first.GetEnumerator())
            using (var secondEnumerator = second.GetEnumerator())
            {
                while (true)
                {
                    bool firstHasNext = firstEnumerator.MoveNext();
                    bool secondHasNext = secondEnumerator.MoveNext();

                    if (firstHasNext)
                    {
                        if (secondHasNext)
                        {
                            int comparison = comparer.Compare(firstEnumerator.Current, secondEnumerator.Current);
                            if (comparison != 0)
                            {
                                return comparison;
                            }
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        if (secondHasNext)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
        }

        public static bool EqualsUnorderedCollection<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second)
        {
            return EqualsUnorderedCollection(first, second, EqualityComparer<T>.Default);
        }

        public static bool EqualsUnorderedCollection<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            IEqualityComparer<T> equalityComparer)
        {
            Guard.ThrowIfNull(first, nameof(first));
            Guard.ThrowIfNull(second, nameof(second));
            Guard.ThrowIfNull(equalityComparer, nameof(equalityComparer));

            var counter = first.ToCounter(equalityComparer);
            foreach (var secondItem in second)
            {
                if (counter.Remove(secondItem) < 0)
                {
                    return false;
                }
            }
            
            return counter.IsEmpty;
        }

        public static int GetCollectionHashCode<T>(
            this IEnumerable<T> values)
        {
            return GetCollectionHashCode(values, EqualityComparer<T>.Default);
        }

        public static int GetCollectionHashCode<T>(
            this IEnumerable<T> values,
            IEqualityComparer<T> equalityComparer)
        {
            Guard.ThrowIfNull(values, nameof(values));
            Guard.ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return values
                .Select(x => equalityComparer.GetHashCode(x))
                .CombineHashCodes();
        }

        public static int GetUnorderedCollectionHashCode<T>(
            this IEnumerable<T> values)
        {
            return GetUnorderedCollectionHashCode(values, EqualityComparer<T>.Default);
        }

        public static int GetUnorderedCollectionHashCode<T>(
            this IEnumerable<T> values,
            IEqualityComparer<T> equalityComparer)
        {
            Guard.ThrowIfNull(values, nameof(values));
            Guard.ThrowIfNull(equalityComparer, nameof(equalityComparer));

            return values
                .Select(x => equalityComparer.GetHashCode(x))
                .OrderBy(x => x)
                .CombineHashCodes();
        }

        public static int CombineHashCodes(this IEnumerable<int> hashCodes)
        {
            Guard.ThrowIfNull(hashCodes, nameof(hashCodes));

            int resultHashCode = 17;
            foreach (var hashCode in hashCodes)
            {
                resultHashCode = resultHashCode * 31 + hashCode;
            }

            return resultHashCode;
        }

        public static Counter<T> ToCounter<T>(this IEnumerable<T> values)
        {
            var counter = new Counter<T>();
            counter.AddRange(values);
            return counter;
        }

        public static Counter<T> ToCounter<T>(this IEnumerable<T> values, IEqualityComparer<T> equalityComparer)
        {
            var counter = new Counter<T>(equalityComparer);
            counter.AddRange(values);
            return counter;
        }
    }
}
