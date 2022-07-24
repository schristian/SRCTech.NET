using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SRCTech.ECS
{
    public class EntityStore : IEntityStore
    {
        private readonly IReadOnlyDictionary<Type, IComponentStore> _componentStores;

        public EntityStore(IReadOnlyDictionary<Type, IComponentStore> componentStores)
        {
            _componentStores = componentStores;
        }

        public int Count => _componentStores.Values.First().Count;

        public static EntityStore Create(Archetype archetype)
        {
            Dictionary<Type, IComponentStore> componentStores = new Dictionary<Type, IComponentStore>();

            foreach (var componentType in archetype.Components)
            {
                // Nasty reflection hackiness
                var componentStoreType = typeof(ComponentStore<>).MakeGenericType(componentType);
                var componentStore = (IComponentStore)Activator.CreateInstance(componentStoreType);

                componentStores.Add(componentType, componentStore);
            }

            return new EntityStore(componentStores);
        }

        public bool HasComponent(Type componentType)
        {
            return _componentStores.ContainsKey(componentType);
        }

        public bool HasComponents(IEnumerable<Type> componentTypes)
        {
            return componentTypes.All(HasComponent);
        }

        public bool HasEntity(EntityId entityId)
        {
            return _componentStores.Values.First().HasEntity(entityId);
        }

        public bool AddEntity(EntityId entityId)
        {
            bool wasAdded = false;
            foreach (var componentStore in _componentStores.Values)
            {
                if (componentStore.AddEntity(entityId))
                {
                    wasAdded = true;
                }
            }

            return wasAdded;
        }

        public bool RemoveEntity(EntityId entityId)
        {
            bool wasRemoved = false;
            foreach (var componentStore in _componentStores.Values)
            {
                if (componentStore.RemoveEntity(entityId))
                {
                    wasRemoved = true;
                }
            }

            return wasRemoved;
        }

        public bool TryGetComponent<T>(EntityId entityId, out T component)
        {
            if (!TryGetComponentStore<T>(out var componentStore))
            {
                component = default;
                return false;
            }

            return componentStore.TryGetComponent(entityId, out component);
        }

        public bool TrySetComponent<T>(EntityId entityId, T component)
        {
            if (!TryGetComponentStore<T>(out var componentStore))
            {
                return false;
            }

            return componentStore.TrySetComponent(entityId, component);
        }

        private bool TryGetComponentStore<T>(out IComponentStore<T> componentStore)
        {
            if (!_componentStores.TryGetValue(typeof(T), out IComponentStore untypedComponentStore))
            {
                componentStore = default;
                return false;
            }

            if (!(untypedComponentStore is IComponentStore<T> typedComponentStore))
            {
                componentStore = default;
                return false;
            }

            componentStore = typedComponentStore;
            return true;
        }

        public IEnumerator<EntityId> GetEnumerator()
        {
            return _componentStores.Values.First().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
