namespace SRCTech.ECS.Chunks;

internal interface IChunkArray : IDisposable
{
    Array Array { get; }

    void CopyTo(
        EntitySlot sourceSlot,
        IChunkArray destinationArray,
        EntitySlot destinationSlot);
}

internal interface IChunkArray<T> : IChunkArray
{
    new T[] Array { get; }

    void CopyTo(
        EntitySlot sourceSlot,
        IChunkArray<T> destinationArray,
        EntitySlot destinationSlot);
}
