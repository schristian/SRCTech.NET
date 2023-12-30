namespace SRCTech.ECS.Chunks;

public interface IChunk
{
    int Capacity { get; }

    int Count { get; }

    Span<EntityId> GetEntityIds();

    EntityId GetEntityId(int slot);

    bool HasComponent<T>();

    Span<T> GetComponents<T>();

    ref T GetComponent<T>(int slot);
}
