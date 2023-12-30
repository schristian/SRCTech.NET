namespace SRCTech.ECS.Archetypes;

internal interface IComponentVisitor<out TResult>
{
    TResult Visit<TComponent>(ComponentId componentId);
}
