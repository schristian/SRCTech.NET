namespace SRCTech.ECS.Archetypes;

public sealed record Archetype(ArchetypeComponents Components) : IEquatable<Archetype>
{
    public bool HasComponent(ComponentId componentId)
    {
        return Components.HasComponent(componentId);
    }

    public Archetype AddComponent(ComponentId componentId)
    {
        ArchetypeComponents newComponents = Components.AddComponent(componentId);
        if (ReferenceEquals(Components, newComponents))
        {
            return this;
        }

        return new Archetype(newComponents);
    }

    public Archetype RemoveComponent(ComponentId componentId)
    {
        ArchetypeComponents newComponents = Components.RemoveComponent(componentId);
        if (ReferenceEquals(Components, newComponents))
        {
            return this;
        }

        return new Archetype(newComponents);
    }
}
