using System;

namespace SRCTech.Common.Lifetimes
{
    public interface IHandle<out T> : IDisposable
    {
        T Value { get; }
    }
}
