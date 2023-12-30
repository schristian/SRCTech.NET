namespace SRCTech.ECS.Archetypes;

internal interface IComponentDescriptor
{
    TResult Accept<TVisitor, TResult>(TVisitor visitor)
        where TVisitor : IComponentVisitor<TResult>;
}
