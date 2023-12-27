namespace SRCTech.ECS;

public interface ISystem<TEvent>
{
    void Run(ICommandBuffer commandBuffer, ReadOnlySpan<TEvent> events);
}
