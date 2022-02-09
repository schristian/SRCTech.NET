using System.Collections;
using System.Collections.Generic;

namespace SRCTech.Common.Collections
{
    public sealed class Counter<T> : IDictionary<T, int>, IReadOnlyDictionary<T, int>
    {
        private Dictionary<T, int> _itemCounts;
        private int _totalCount;

        public Counter() 
        {
            _itemCounts = new Dictionary<T, int>();
            _totalCount = 0;
        }

        public Counter(IEqualityComparer<T> equalityComparer)
        {
            Guard.ThrowIfNull(equalityComparer, nameof(equalityComparer));

            _itemCounts = new Dictionary<T, int>(equalityComparer);
            _totalCount = 0;
        }

        public int Count => _itemCounts.Count;

        public int TotalCount => _totalCount;

        public bool IsEmpty => _itemCounts.Count == 0;

        public bool IsReadOnly => false;

        public IEqualityComparer<T> Comparer => _itemCounts.Comparer;

        public ICollection<T> Keys => _itemCounts.Keys;

        public ICollection<int> Values => _itemCounts.Values;

        IEnumerable<T> IReadOnlyDictionary<T, int>.Keys => _itemCounts.Keys;

        IEnumerable<int> IReadOnlyDictionary<T, int>.Values => _itemCounts.Values;

        public int this[T item]
        {
            get
            {
                Guard.ThrowIfNull(item, nameof(item));

                if (_itemCounts.TryGetValue(item, out var itemCount))
                {
                    return itemCount;
                }

                return 0;
            }

            set
            {
                Guard.ThrowIfNull(item, nameof(item));

                bool hasValue = _itemCounts.TryGetValue(item, out var oldItemCount);
                oldItemCount = hasValue ? oldItemCount : 0;

                _totalCount += value - oldItemCount;

                if (value != 0)
                {
                    _itemCounts[item] = value;
                }
                else if (hasValue)
                {
                    _itemCounts.Remove(item);
                }
            }
        }

        public bool ContainsKey(T item)
        {
            Guard.ThrowIfNull(item, nameof(item));

            return _itemCounts.ContainsKey(item);
        }

        public bool TryGetValue(T item, out int itemCount)
        {
            return _itemCounts.TryGetValue(item, out itemCount);
        }

        public int Add(T item)
        {
            return Add(item, 1);
        }
            
        public int Add(T item, int amount)
        {
            bool hasValue = _itemCounts.TryGetValue(item, out var oldItemCount);
            oldItemCount = hasValue ? oldItemCount : 0;

            var newItemCount = oldItemCount + amount;
            _totalCount += newItemCount - oldItemCount;

            if (newItemCount != 0)
            {
                _itemCounts[item] = newItemCount;
            }
            else if (hasValue)
            {
                _itemCounts.Remove(item);
            }

            return newItemCount;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public int Remove(T item)
        {
            return Add(item, -1);
        }

        public int Remove(T item, int amount)
        {
            return Add(item, -amount);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        public void Clear()
        {
            _itemCounts.Clear();
            _totalCount = 0;
        }

        public bool Contains(KeyValuePair<T, int> item)
        {
            return ((ICollection<KeyValuePair<T, int>>)_itemCounts).Contains(item);
        }

        public void CopyTo(KeyValuePair<T, int>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<T, int>>)_itemCounts).CopyTo(array, arrayIndex);
        }

        void ICollection<KeyValuePair<T, int>>.Add(KeyValuePair<T, int> item)
        {
            ((ICollection<KeyValuePair<T, int>>)_itemCounts).Add(item);
        }

        bool ICollection<KeyValuePair<T, int>>.Remove(KeyValuePair<T, int> item)
        {
            return ((ICollection<KeyValuePair<T, int>>)_itemCounts).Remove(item);
        }

        void IDictionary<T, int>.Add(T key, int value)
        {
            _itemCounts.Add(key, value);
        }

        bool IDictionary<T, int>.Remove(T key)
        {
            return _itemCounts.Remove(key);
        }

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
        {
            return _itemCounts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _itemCounts.GetEnumerator();
        }
    }
}
