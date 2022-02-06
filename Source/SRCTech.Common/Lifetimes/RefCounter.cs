using System;
using System.Threading;

namespace SRCTech.Common.Lifetimes
{
    internal sealed class RefCounter<T>
    {
        private const int ExpiredRefCount = -2000000000;

        private IDisposable _disposable;
        private int _refCount;

        public RefCounter(T value, IDisposable disposable, int initialRefCount)
        {
            Value = value;
            _disposable = disposable;
            _refCount = initialRefCount;
        }

        public T Value { get; }

        public int RefCount => Math.Max(0, _refCount);

        public bool IsExpired => _disposable == null;

        public bool TryAcquire()
        {
            if (Interlocked.Increment(ref _refCount) <= 0)
            {
                Interlocked.Exchange(ref _refCount, ExpiredRefCount);

                return false;
            }

            return true;
        }

        public void Release()
        {
            if (Interlocked.Decrement(ref _refCount) == 0)
            {
                if (Interlocked.CompareExchange(ref _refCount, ExpiredRefCount, 0) == 0)
                {
                    Interlocked.Exchange(ref _disposable, null)?.Dispose();
                }
            }
        }
    }
}
