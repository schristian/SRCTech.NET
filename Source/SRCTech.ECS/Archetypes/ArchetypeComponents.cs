namespace SRCTech.ECS.Archetypes;

public class ArchetypeComponents : IEquatable<ArchetypeComponents>
{
    private ComponentId[] _components;

    public ArchetypeComponents()
    {
        _components = Array.Empty<ComponentId>();
    }

    public ArchetypeComponents(IEnumerable<ComponentId> components)
    {
        _components = components.Distinct().Order().ToArray();
    }

    public bool HasComponent(ComponentId componentId)
    {
        return Array.BinarySearch(_components, componentId) >= 0;
    }

    public ArchetypeComponents AddComponent(ComponentId componentId)
    {
        int foundIndex = Array.BinarySearch(_components, componentId);
        if (foundIndex >= 0)
        {
            return this;
        }

        int nextIndex = ~foundIndex;
        ComponentId[] newArray = new ComponentId[_components.Length + 1];

        Array.Copy(_components, 0, newArray, 0, nextIndex);
        newArray[nextIndex] = componentId;
        Array.Copy(_components, nextIndex, newArray, nextIndex + 1, _components.Length - nextIndex);
        return new ArchetypeComponents(newArray);
    }

    public ArchetypeComponents RemoveComponent(ComponentId componentId)
    {
        int foundIndex = Array.BinarySearch(_components, componentId);
        if (foundIndex < 0)
        {
            return this;
        }

        ComponentId[] newArray = new ComponentId[_components.Length - 1];

        Array.Copy(_components, 0, newArray, 0, foundIndex);
        Array.Copy(_components, foundIndex + 1, newArray, foundIndex, newArray.Length - foundIndex);
        return new ArchetypeComponents(newArray);
    }

    public bool Equals(ArchetypeComponents? other)
    {
        return other is not null && other._components.SequenceEqual(_components);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ArchetypeComponents);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach (var component in _components)
        {
            hashCode.Add(component);
        }

        return hashCode.ToHashCode();
    }
}
