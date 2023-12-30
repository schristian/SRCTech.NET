namespace SRCTech.ECS.Chunks;

internal interface IChunkArrayAllocator
{
    IChunkArray AcquireArray(Type type, int capacity);

    IChunkArray<T> AcquireArray<T>(int capacity);
}
