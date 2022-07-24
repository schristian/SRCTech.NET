using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRCTech.ECS.ChunkList
{
    public interface IChunkList<T> : IChunkList, IReadOnlyChunkList<T>
    {
        new ref T this[int index] { get; }

        IEnumerator<Memory<T>> GetBulkEnumerator();

        void Push(T item);
    }
}
