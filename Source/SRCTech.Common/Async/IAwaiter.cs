using System.Runtime.CompilerServices;

namespace SRCTech.Common.Async
{
    public interface IAwaiter<out T> : INotifyCompletion
    {
        bool IsCompleted { get; }

        T GetResult();
    }
}
