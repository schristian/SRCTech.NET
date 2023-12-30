namespace SRCTech.ECS.Chunks;

public record struct EntityChunkRef(ArchetypeId ArchetypeId, ChunkId ChunkId, EntitySlot EntitySlot)
{
}
