using System;
using System.Threading;

namespace SRCTech.Common.Lifetimes
{
    public sealed class SharedHandle<T> : IHandle<T>
    {
        private RefCounter<T> _refCounter;

        public SharedHandle(T value, IDisposable disposable)
        {
            Guard.ThrowIfNull(disposable, nameof(disposable));

            _refCounter = new RefCounter<T>(value, disposable, 1);
        }

        private SharedHandle(RefCounter<T> refCounter)
        {
            _refCounter = refCounter;
        }

        public T Value => TryGetValue(_refCounter);

        public SharedHandle<T> Clone()
        {
            return TryClone(_refCounter);
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _refCounter, null)?.Release();
        }

        private static SharedHandle<T> TryClone(RefCounter<T> refCounter)
        {
            if (refCounter == null || !refCounter.TryAcquire())
            {
                throw new ObjectDisposedException(nameof(SharedHandle<T>));
            }

            return new SharedHandle<T>(refCounter);
        }

        private static T TryGetValue(RefCounter<T> refCounter)
        {
            if (refCounter == null)
            {
                throw new ObjectDisposedException(nameof(SharedHandle<T>));
            }

            return refCounter.Value;
        }
    }
}
