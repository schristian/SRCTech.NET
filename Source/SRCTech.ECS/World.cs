using SRCTech.ECS.Archetypes;
using SRCTech.ECS.Chunks;

namespace SRCTech.ECS;

public sealed class World
{
    private readonly ArchetypeGraph _archetypeGraph;
    private readonly Dictionary<EntityId, EntityChunkRef> _entityChunkRefs;

    private readonly Dictionary<IndexId, >
    private readonly Lookup<ComponentId, IndexId> _indexesByComponent;
}
