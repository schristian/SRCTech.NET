namespace SRCTech.ECS;

public record struct EntityId(long Id)
{
    public static EntityId Invalid { get; } = new EntityId(-1);
}
