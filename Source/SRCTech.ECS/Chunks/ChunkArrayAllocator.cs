using System.Buffers;

namespace SRCTech.ECS.Chunks;

internal class ChunkArrayAllocator : IChunkArrayAllocator
{
    private Dictionary<Type, IChunkArrayPool> _pools = new();

    public IChunkArray AcquireArray(Type type, int capacity)
    {
        if (!_pools.TryGetValue(type, out var pool))
        {
            Type poolType = typeof(ChunkArrayPool<>).MakeGenericType(type);
            pool = (IChunkArrayPool)Activator.CreateInstance(poolType)!;
            _pools.Add(type, pool);
        }

        return pool.AcquireArray(capacity);
    }

    public IChunkArray<T> AcquireArray<T>(int capacity)
    {
        Type type = typeof(T);
        if (!_pools.TryGetValue(type, out var pool))
        {
            pool = new ChunkArrayPool<T>();
            _pools.Add(type, pool);
        }

        return (IChunkArray<T>)pool.AcquireArray(capacity);
    }

    private interface IChunkArrayPool
    {
        IChunkArray AcquireArray(int capacity);
    }

    private sealed class ChunkArrayPool<T> : IChunkArrayPool
    {
        private readonly ArrayPool<T> _arrayPool = ArrayPool<T>.Create();

        public IChunkArray AcquireArray(int capacity)
        {
            T[] array = _arrayPool.Rent(capacity);
            return new ChunkArray<T>(_arrayPool, array);
        }
    }

    private sealed class ChunkArray<T> : IChunkArray<T>
    {
        private readonly ArrayPool<T> _arrayPool;

        public ChunkArray(ArrayPool<T> arrayPool, T[] array)
        {
            _arrayPool = arrayPool;
            Array = array;
        }

        public T[] Array { get; }

        Array IChunkArray.Array => Array;

        public void Dispose()
        {
            _arrayPool.Return(Array);
        }

        public void CopyTo(
            EntitySlot sourceSlot,
            IChunkArray destinationArray,
            EntitySlot destinationSlot)
        {
            CopyTo(sourceSlot, (IChunkArray<T>)destinationArray, destinationSlot);
        }

        public void CopyTo(
            EntitySlot sourceSlot,
            IChunkArray<T> destinationArray,
            EntitySlot destinationSlot)
        {
            destinationArray.Array[destinationSlot] = Array[sourceSlot];
        }
    }
}
