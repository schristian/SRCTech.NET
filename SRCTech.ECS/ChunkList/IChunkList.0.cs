using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRCTech.ECS.ChunkList
{
    public interface IChunkList
    {
        void Swap(int firstIndex, int secondIndex);

        void Push();

        bool Pop();

        void Clear();
    }
}
