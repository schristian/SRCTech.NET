using System.Collections.Generic;

namespace SRCTech.ECS
{
    public interface IComponentStore : IReadOnlyCollection<EntityId>
    {
        bool HasEntity(EntityId entityId);

        bool AddEntity(EntityId entityId);

        bool RemoveEntity(EntityId entityId);
    }
}
