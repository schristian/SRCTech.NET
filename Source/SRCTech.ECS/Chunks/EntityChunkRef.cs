namespace SRCTech.ECS.Chunks;

public record struct EntityChunkRef(ChunkId ChunkId, ArchetypeId ArchetypeId, EntitySlot EntitySlot)
{
}
