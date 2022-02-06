using SRCTech.Common.Functional;
using System;
using System.Threading;

namespace SRCTech.Common.Lifetimes
{
    public sealed class SharedDisposable : IDisposable
    {
        private RefCounter<Unit> _refCounter;

        public SharedDisposable(IDisposable disposable)
        {
            Guard.ThrowIfNull(disposable, nameof(disposable));

            _refCounter = new RefCounter<Unit>(Unit.Default, disposable, 1);
        }

        private SharedDisposable(RefCounter<Unit> refCounter)
        {
            _refCounter = refCounter;
        }

        public SharedDisposable Clone()
        {
            return TryClone(_refCounter);
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _refCounter, null)?.Release();
        }

        private static SharedDisposable TryClone(RefCounter<Unit> refCounter)
        {
            if (refCounter == null || !refCounter.TryAcquire())
            {
                throw new ObjectDisposedException(nameof(SharedDisposable));
            }

            return new SharedDisposable(refCounter);
        }
    }
}
