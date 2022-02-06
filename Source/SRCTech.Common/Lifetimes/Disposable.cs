using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SRCTech.Common.Lifetimes
{
    public static class Disposable
    {
        public static IDisposable Empty => EmptyDisposable.Instance;

        public static IDisposable Create(Action disposeAction)
        {
            Guard.ThrowIfNull(disposeAction, nameof(disposeAction));

            return new AnonymousDisposable(disposeAction);
        }

        public static IDisposable Create<T>(Func<T> disposeAction)
        {
            Guard.ThrowIfNull(disposeAction, nameof(disposeAction));

            return new AnonymousDisposable(() => disposeAction());
        }

        public static IDisposable CreateFromDisposables(IEnumerable<IDisposable> disposables)
        {
            var disposablesList = Guard.ThrowIfAnyItemsNull(disposables?.ToList(), nameof(disposables));

            return new CompositeDisposable(disposablesList);
        }

        public static IDisposable CreateFromDisposables(params IDisposable[] disposables)
        {
            Guard.ThrowIfAnyItemsNull(disposables, nameof(disposables));

            return new CompositeDisposable(disposables);
        }

        public static SharedDisposable CreateShared(this IDisposable disposable)
        {
            Guard.ThrowIfNull(disposable, nameof(disposable));

            return new SharedDisposable(disposable);
        }

        public static void DisposeAll(this IEnumerable<IDisposable> disposables)
        {
            Guard.ThrowIfNull(disposables, nameof(disposables));

            List<Exception> exceptions = null;
            bool hasNullItems = false;
            foreach (IDisposable disposable in disposables)
            {
                try
                {
                    if (disposable == null)
                    {
                        hasNullItems = true;
                    }
                    else
                    {
                        disposable.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    exceptions = exceptions ?? new List<Exception>();
                    exceptions.Add(exception);
                }
            }

            if (hasNullItems)
            {
                exceptions = exceptions ?? new List<Exception>();
                exceptions.Add(new ArgumentException($"Parameter '{nameof(disposables)}' contains one or more null items.", nameof(disposables)));
            }

            if (exceptions != null)
            {
                throw new AggregateException(exceptions);
            }
        }

        private sealed class AnonymousDisposable : IDisposable
        {
            private Action disposeAction;

            public AnonymousDisposable(Action disposeAction)
            {
                this.disposeAction = disposeAction;
            }

            public bool IsDisposed => disposeAction == null;

            public void Dispose()
            {
                Interlocked.Exchange(ref disposeAction, null)?.Invoke();
            }
        }

        private sealed class CompositeDisposable : IDisposable
        {
            private IReadOnlyCollection<IDisposable> disposables;

            public CompositeDisposable(IReadOnlyCollection<IDisposable> disposables)
            {
                this.disposables = disposables;
            }

            public bool IsDisposed => disposables == null;

            public void Dispose()
            {
                Interlocked.Exchange(ref disposables, null)?.DisposeAll();
            }
        }

        private sealed class EmptyDisposable : IDisposable
        {
            public static EmptyDisposable Instance { get; } = new EmptyDisposable();

            public void Dispose()
            {
            }
        }
    }
}
