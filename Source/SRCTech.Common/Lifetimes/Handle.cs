using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SRCTech.Common.Lifetimes
{
    public static class Handle
    {
        public static IHandle<T> Create<T>(T value, Action disposeAction)
        {
            Guard.ThrowIfNull(disposeAction, nameof(disposeAction));

            return new DefaultHandle<T>(value, Disposable.Create(disposeAction));
        }

        public static IHandle<T> Create<T>(T value, Action<T> disposeAction)
        {
            Guard.ThrowIfNull(disposeAction, nameof(disposeAction));

            return new DefaultHandle<T>(value, Disposable.Create(() => disposeAction(value)));
        }

        public static IHandle<T> Create<T>(T value, IDisposable disposable)
        {
            Guard.ThrowIfNull(disposable, nameof(disposable));

            return new DefaultHandle<T>(value, disposable);
        }

        public static IHandle<T> Create<T>(T value) where T : IDisposable
        {
            Guard.ThrowIfNull(value, nameof(value));

            return new DefaultHandle<T>(value, value);
        }

        public static IHandle<T> CreateFromDisposables<T>(T value, IEnumerable<IDisposable> disposables)
        {
            var disposablesList = Guard.ThrowIfAnyItemsNull(disposables?.ToList(), nameof(disposables));

            return new DefaultHandle<T>(value, Disposable.CreateFromDisposables(disposablesList));
        }

        public static IHandle<T> CreateFromDisposables<T>(T value, params IDisposable[] disposables)
        {
            Guard.ThrowIfAnyItemsNull(disposables, nameof(disposables));

            return new DefaultHandle<T>(value, Disposable.CreateFromDisposables(disposables));
        }

        public static SharedHandle<T> CreateShared<T>(T value, IDisposable disposable)
        {
            Guard.ThrowIfNull(disposable, nameof(disposable));

            return new SharedHandle<T>(value, disposable);
        }

        public static SharedHandle<T> CreateShared<T>(this IHandle<T> handle)
        {
            Guard.ThrowIfNull(handle, nameof(handle));

            return new SharedHandle<T>(handle.Value, handle);
        }

        public static IHandle<T> CreateWithoutDisposable<T>(T value)
        {
            return new DefaultHandle<T>(value, Disposable.Empty);
        }

        private sealed class DefaultHandle<T> : IHandle<T>
        {
            private IDisposable disposable;
            private readonly T value;

            public DefaultHandle(T value, IDisposable disposable)
            {
                this.value = value;
                this.disposable = disposable;
            }

            public T Value
            {
                get
                {
                    if (IsDisposed)
                    {
                        throw new ObjectDisposedException(nameof(DefaultHandle<T>));
                    }

                    return value;
                }
            }

            public bool IsDisposed => disposable == null;

            public void Dispose()
            {
                Interlocked.Exchange(ref disposable, null)?.Dispose();
            }
        }
    }
}
