using System;
using System.Collections.Generic;
using System.Linq;

namespace SRCTech.ECS
{
    public class World
    {
        private readonly IReadOnlyDictionary<Archetype, IEntityStore> _entityStores;
        private long _nextEntityId;

        public World(IReadOnlyDictionary<Archetype, IEntityStore> entityStores)
        {
            _entityStores = entityStores;
            _nextEntityId = 1;
        }

        public bool HasEntity(Archetype archetype, EntityId entityId)
        {
            if (!_entityStores.TryGetValue(archetype, out var entityStore))
            {
                return false;
            }

            return entityStore.HasEntity(entityId);
        }

        public EntityId AddEntity(Archetype archetype)
        {
            EntityId entityId = new EntityId(_nextEntityId++);

            if (!_entityStores.TryGetValue(archetype, out var entityStore))
            {
                throw new ArgumentException();
            }

            entityStore.AddEntity(entityId);
            return entityId;
        }

        public bool RemoveEntity(Archetype archetype, EntityId entityId)
        {
            if (!_entityStores.TryGetValue(archetype, out var entityStore))
            {
                return false;
            }

            return entityStore.RemoveEntity(entityId);
        }

        public bool TryGetComponent<T>(Archetype archetype, EntityId entityId, out T component)
        {
            if (!_entityStores.TryGetValue(archetype, out var entityStore))
            {
                component = default;
                return false;
            }

            return entityStore.TryGetComponent(entityId, out component);
        }

        public bool TrySetComponent<T>(Archetype archetype, EntityId entityId, T component)
        {
            if (!_entityStores.TryGetValue(archetype, out var entityStore))
            {
                return false;
            }

            return entityStore.TrySetComponent(entityId, component);
        }

        public IEnumerable<KeyValuePair<EntityId, IEntityStore>> QueryEntitiesByComponents(IReadOnlyCollection<Type> componentTypes)
        {
            return _entityStores.Values
                .Where(entityStore => entityStore.HasComponents(componentTypes))
                .SelectMany(
                    entityStore => entityStore,
                    (entityStore, entityId) => KeyValuePair.Create(entityId, entityStore));
        }
    }
}
