using System;
using System.Collections.Generic;
using System.Text;

namespace SRCTech.ECS
{
    public interface IEntityStore : IReadOnlyCollection<EntityId>
    {
        bool HasComponent(Type componentType);

        bool HasComponents(IEnumerable<Type> componentTypes);

        bool HasEntity(EntityId entityId);

        bool AddEntity(EntityId entityId);

        bool RemoveEntity(EntityId entityId);

        bool TryGetComponent<T>(EntityId entityId, out T component);

        bool TrySetComponent<T>(EntityId entityId, T component);
    }
}
