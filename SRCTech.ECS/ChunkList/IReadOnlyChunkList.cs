using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRCTech.ECS.ChunkList
{
    public interface IReadOnlyChunkList<T> : IReadOnlyCollection<T>
    {
        ref readonly T this[int index] { get; }

        IEnumerator<ReadOnlyMemory<T>> GetReadOnlyBulkEnumerator();
    }
}
