namespace SRCTech.ECS.Chunks;

internal interface IChunkArray : IDisposable
{
    void Copy(int sourceSlot, int destinationSlot);
}

internal interface IChunkArray<T> : IChunkArray
{
    Span<T> GetSpan(int count);
}
