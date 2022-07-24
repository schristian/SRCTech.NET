using System;
using System.Collections;
using System.Collections.Generic;

namespace SRCTech.ECS.ChunkList
{
    public sealed class ChunkList<T> : IChunkList<T>
    {
        private readonly List<T[]> _chunks;
        private readonly int _chunkCapacity;

        private int _count;

        public ChunkList(int chunkCapacity)
        {
            _chunks = new List<T[]>();
            _chunkCapacity = chunkCapacity;
            _count = 0;
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public ref T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                int chunkIndex = Math.DivRem(index, _chunkCapacity, out int itemIndex);
                return ref _chunks[chunkIndex][itemIndex];
            }
        }

        ref readonly T IReadOnlyChunkList<T>.this[int index] => ref this[index];

        public void Swap(int firstIndex, int secondIndex)
        {
            if (firstIndex < 0 || firstIndex >= _count)
            {
                throw new ArgumentOutOfRangeException(nameof(firstIndex));
            }

            if (secondIndex < 0 || secondIndex >= _count)
            {
                throw new ArgumentOutOfRangeException(nameof(secondIndex));
            }

            if (firstIndex == secondIndex)
            {
                return;
            }

            int firstChunkIndex = Math.DivRem(firstIndex, _chunkCapacity, out int firstItemIndex);
            int secondChunkIndex = Math.DivRem(secondIndex, _chunkCapacity, out int secondItemIndex);

            var firstChunk = _chunks[firstChunkIndex];
            var secondChunk = _chunks[secondChunkIndex];

            (firstChunk[firstItemIndex], secondChunk[secondItemIndex]) = (secondChunk[secondItemIndex], firstChunk[firstItemIndex]);
        }

        public void Push(T item)
        {
            int lastChunkIndex = Math.DivRem(_count, _chunkCapacity, out int lastChunkItemIndex);
            _count += 1;

            if (lastChunkItemIndex == 0)
            {
                var chunk = new T[_chunkCapacity];
                chunk[0] = item;

                _chunks.Add(chunk);
            }
            else
            {
                _chunks[lastChunkIndex][lastChunkItemIndex] = item;
            }
        }

        public void Push() => Push(default);

        public bool Pop()
        {
            if (_count <= 0)
            {
                return false;
            }

            _count -= 1;
            int lastChunkIndex = Math.DivRem(_count, _chunkCapacity, out int lastChunkItemIndex);

            if (lastChunkItemIndex == 0)
            {
                _chunks.RemoveAt(lastChunkIndex);
            }
            else
            {
                _chunks[lastChunkIndex][lastChunkItemIndex] = default;
            }

            return true;
        }

        public void Clear()
        {
            _chunks.Clear();
            _count = 0;
        }

        public IEnumerator<Memory<T>> GetBulkEnumerator()
        {
            if (_count == 0)
            {
                yield break;
            }

            int fullChunkCount = Math.DivRem(_count, _chunkCapacity, out int lastChunkItemCount);

            // Iterate through all full chunks
            for (int chunkIndex = 0; chunkIndex < fullChunkCount; chunkIndex++)
            {
                yield return new Memory<T>(_chunks[chunkIndex]);
            }

            // Return last chunk if it is partially filled
            if (lastChunkItemCount > 0)
            {
                yield return new Memory<T>(_chunks[fullChunkCount], 0, lastChunkItemCount);
            }
        }

        public IEnumerator<ReadOnlyMemory<T>> GetReadOnlyBulkEnumerator()
        {
            if (_count == 0)
            {
                yield break;
            }

            int fullChunkCount = Math.DivRem(_count, _chunkCapacity, out int lastChunkItemCount);

            // Iterate through all full chunks
            for (int chunkIndex = 0; chunkIndex < fullChunkCount; chunkIndex++)
            {
                yield return new ReadOnlyMemory<T>(_chunks[chunkIndex]);
            }

            // Return last chunk if it is partially filled
            if (lastChunkItemCount > 0)
            {
                yield return new ReadOnlyMemory<T>(_chunks[fullChunkCount], 0, lastChunkItemCount);
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            if (_count == 0)
            {
                yield break;
            }

            int fullChunkCount = Math.DivRem(_count, _chunkCapacity, out int lastChunkItemCount);

            // Iterate through all full chunks
            for (int chunkIndex = 0; chunkIndex < fullChunkCount; chunkIndex++)
            {
                var fullChunk = _chunks[chunkIndex];
                for (int itemIndex = 0; itemIndex < _chunkCapacity; itemIndex++)
                {
                    yield return fullChunk[itemIndex];
                }
            }

            // Return last chunk if it is partially filled
            if (lastChunkItemCount > 0)
            {
                var lastChunk = _chunks[fullChunkCount];
                for (int itemIndex = 0; itemIndex < lastChunkItemCount; itemIndex++)
                {
                    yield return lastChunk[itemIndex];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
