using SRCTech.Common.Lifetimes;

namespace SRCTech.ECS.Chunks;

internal sealed class Chunk : IDisposable
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

    public bool IsEmpty => Count == 0;

    public EntitySlot LastSlot => new EntitySlot(Count - 1);

    public void Dispose()
    {
        _componentArrays.Values.DisposeAll();
    }

    public void CopyEntity(
        EntitySlot sourceSlot,
        Chunk destinationChunk,
        EntitySlot destinationSlot)
    {
        _entityIds.CopyTo(sourceSlot, destinationChunk._entityIds, destinationSlot);

        foreach (var kvp in destinationChunk._componentArrays)
        {
            _componentArrays[kvp.Key].CopyTo(sourceSlot, kvp.Value, destinationSlot);
        }
    }

    public EntitySlot PushEntity(EntityId entityId)
    {
        Count += 1;
        _entityIds.Array[LastSlot] = entityId;
        return new EntitySlot(LastSlot);
    }

    public EntityId PopEntity()
    {
        var entityId = _entityIds.Array[LastSlot];
        Count -= 1;
        return entityId;
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
        return new Span<T>(typedArray.Array, 0, Count);
    }

    public ref T GetComponentRef<T>(EntitySlot entitySlot)
    {
        return ref GetComponentSpan<T>()[entitySlot.Slot];
    }
}
