using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRCTech.ECS
{
    public struct EntityId : IEquatable<EntityId>
    {
        public EntityId(long id)
        {
            this.Id = id;
        }

        public long Id { get; }

        public bool Equals(EntityId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is EntityId other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
