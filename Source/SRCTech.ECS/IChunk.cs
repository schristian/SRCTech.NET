namespace SRCTech.ECS;

public interface IChunk
{
    int Count { get; }

    bool HasComponent<TComponent>() where TComponent : IComponent<TComponent>;

    Span<TComponent> GetComponentBuffer<TComponent>() where TComponent : IComponent<TComponent>;
}
