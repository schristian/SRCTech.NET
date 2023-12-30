using SRCTech.Common.Lifetimes;

namespace SRCTech.ECS.Chunks;

public sealed class Chunk : IDisposable
{
    private readonly IChunkArrayAllocator _chunkArrayAllocator;
    private readonly IChunkArray<EntityId> _entityIds;
    private readonly Dictionary<ComponentId, IChunkArray> _componentArrays;

    internal Chunk(IChunkArrayAllocator chunkArrayAllocator, int capacity)
    {
        _chunkArrayAllocator = chunkArrayAllocator;
        _entityIds = _chunkArrayAllocator.AcquireArray<EntityId>(capacity);
        _componentArrays = new();

        Capacity = capacity;
        Count = 0;
    }
    
    public int Capacity { get; set; }

    public int Count { get; set; }

    public void Dispose()
    {
        _componentArrays.Values.DisposeAll();
    }

    public EntitySlot AddEntity(EntityId entityId)
    {
        if (Count >= Capacity)
        {
            throw new InvalidOperationException(); // TODO: Add message
        }

        int slot = Count;
        Count += 1;
        _entityIds.GetSpan(Count)[slot] = entityId;
        return new EntitySlot(slot);
    }

    public void RemoveEntity(EntitySlot entitySlot)
    {
        var slot = entitySlot.Slot;
        var lastSlot = Count - 1;
        if (slot != lastSlot)
        {
            Span<EntityId> entityIds = _entityIds.GetSpan(Count);
            var movedEntityId = entityIds[lastSlot];
            entityIds[slot] = movedEntityId;

            foreach (var array in _componentArrays.Values)
            {
                array.Copy(lastSlot, slot);
            }
        }

        Count -= 1;
    }

    public void AddComponentType(ComponentId componentId)
    {
        _componentArrays.Add(componentId, _chunkArrayAllocator.AcquireArray(componentId.Type, Capacity));
    }

    public void RemoveComponentType(ComponentId componentId)
    {
        _componentArrays.Remove(componentId);
    }

    public Span<T> GetComponentSpan<T>()
    {
        var componentId = new ComponentId(typeof(T));
        if (!_componentArrays.TryGetValue(componentId, out var array))
        {
            throw new ArgumentException(); // TODO: Add message
        }

        IChunkArray<T> typedArray = (IChunkArray<T>)array;
        return typedArray.GetSpan(Count);
    }

    public ref T GetComponentRef<T>(EntitySlot entitySlot)
    {
        return ref GetComponentSpan<T>()[entitySlot.Slot];
    }
}
