namespace SRCTech.ECS.Archetypes;

internal class ArchetypeGraph
{
    private readonly Dictionary<Archetype, ArchetypeId> _archetypeIds = new();
    private readonly Dictionary<ArchetypeId, Archetype> _archetypes = new();

    private readonly Dictionary<(ArchetypeId, ComponentId), ArchetypeId> _addComponentEdges = new();
    private readonly Dictionary<(ArchetypeId, ComponentId), ArchetypeId> _removeComponentEdges = new();

    private int _prevArchetypeId = 0;

    public IReadOnlyDictionary<Archetype, ArchetypeId> ArchetypeIds => _archetypeIds;

    public IReadOnlyDictionary<ArchetypeId, Archetype> Archetypes => _archetypes;

    public ArchetypeId AddComponent(ArchetypeId archetypeId, ComponentId componentId)
    {
        if (!_addComponentEdges.TryGetValue((archetypeId, componentId), out var newArchetypeId))
        {
            var newArchetype = _archetypes[archetypeId].AddComponent(componentId);
            if (!_archetypeIds.TryGetValue(newArchetype, out newArchetypeId))
            {
                newArchetypeId = new ArchetypeId(Interlocked.Increment(ref _prevArchetypeId));

                _archetypeIds[newArchetype] = newArchetypeId;
                _archetypes[newArchetypeId] = newArchetype;
            }

            _addComponentEdges[(archetypeId, componentId)] = newArchetypeId;
            _removeComponentEdges[(newArchetypeId, componentId)] = archetypeId;
        }

        return newArchetypeId;
    }

    public ArchetypeId RemoveComponent(ArchetypeId archetypeId, ComponentId componentId)
    {
        if (!_removeComponentEdges.TryGetValue((archetypeId, componentId), out var newArchetypeId))
        {
            var newArchetype = _archetypes[archetypeId].RemoveComponent(componentId);
            if (!_archetypeIds.TryGetValue(newArchetype, out newArchetypeId))
            {
                newArchetypeId = new ArchetypeId(Interlocked.Increment(ref _prevArchetypeId));

                _archetypeIds[newArchetype] = newArchetypeId;
                _archetypes[newArchetypeId] = newArchetype;
            }

            _removeComponentEdges[(archetypeId, componentId)] = newArchetypeId;
            _addComponentEdges[(newArchetypeId, componentId)] = archetypeId;
        }

        return newArchetypeId;
    }
}
