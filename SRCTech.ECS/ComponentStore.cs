using System.Collections;
using System.Collections.Generic;

namespace SRCTech.ECS
{
    public class ComponentStore<T> : IComponentStore<T>
    {
        private readonly IDictionary<EntityId, T> _components;

        public int Count => _components.Count;

        public bool HasEntity(EntityId entityId)
        {
            return _components.ContainsKey(entityId);
        }

        public bool AddEntity(EntityId entityId)
        {
            if (HasEntity(entityId))
            {
                return false;
            }

            _components.Add(entityId, default);
            return true;
        }

        public bool RemoveEntity(EntityId entityId)
        {
            return _components.Remove(entityId);
        }

        public bool TryGetComponent(EntityId entityId, out T component)
        {
            return _components.TryGetValue(entityId, out component);
        }

        public bool TrySetComponent(EntityId entityId, T component)
        {
            if (HasEntity(entityId))
            {
                return false;
            }

            _components[entityId] = component;
            return true;
        }

        public IEnumerator<EntityId> GetEnumerator()
        {
            return _components.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
