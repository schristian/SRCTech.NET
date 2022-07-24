namespace SRCTech.ECS
{
    public interface IComponentStore<T> : IComponentStore
    {
        bool TryGetComponent(EntityId entityId, out T component);

        bool TrySetComponent(EntityId entityId, T component);
    }
}
