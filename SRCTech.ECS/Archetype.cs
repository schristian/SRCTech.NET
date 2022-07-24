using System;
using System.Collections.Generic;
using SRCTech.Common.Collections;

namespace SRCTech.ECS
{
    public struct Archetype : IEquatable<Archetype>
    {
        public Archetype(IReadOnlyList<Type> components)
        {
            this.Components = components;
        }

        public IReadOnlyList<Type> Components { get; }

        public bool Equals(Archetype other)
        {
            return CollectionHelper.EqualsUnorderedCollection(Components, other.Components);
        }

        public override bool Equals(object obj)
        {
            if (obj is Archetype other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return CollectionHelper.GetUnorderedCollectionHashCode(Components);
        }
    }
}
