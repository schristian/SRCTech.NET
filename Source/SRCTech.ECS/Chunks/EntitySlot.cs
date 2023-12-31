namespace SRCTech.ECS.Chunks;

public record struct EntitySlot(int Slot)
{
    public static implicit operator int(EntitySlot slot) => slot.Slot;
}
