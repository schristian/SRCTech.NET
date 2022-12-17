using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SRCTech.Common.Functional;

namespace SRCTech.Common.Async
{
    public static partial class Awaitable
    {
        public static IAwaitable<T> Create<T>(Func<ValueTask<T>> func)
        {
            return func().ToAwaitable();
        }

        public static IAwaitable<T> FromResult<T>(T value)
        {
            return new ResultAwaitable<T>(value);
        }

        public static IAwaitable<T> FromException<T>(Exception exception)
        {
            return new ExceptionAwaitable<T>(exception);
        }

        public static IAwaitable<Unit> ToAwaitable<T>(this Task task)
        {
            return new TaskAwaitable(task);
        }

        public static IAwaitable<T> ToAwaitable<T>(this Task<T> task)
        {
            return new TaskAwaitable<T>(task);
        }

        public static IAwaitable<Unit> ToAwaitable<T>(this ValueTask task)
        {
            return new TaskAwaitable(task.AsTask());
        }

        public static IAwaitable<T> ToAwaitable<T>(this ValueTask<T> task)
        {
            return new TaskAwaitable<T>(task.AsTask());
        }

        private sealed class TaskAwaitable : IAwaitable<Unit>
        {
            private Task _task;

            public TaskAwaitable(Task task)
            {
                _task = task;
            }

            public IAwaiter<Unit> GetAwaiter() => new Awaiter(_task.GetAwaiter());

            private sealed class Awaiter : IAwaiter<Unit>
            {
                private TaskAwaiter _taskAwaiter;

                public Awaiter(TaskAwaiter taskAwaiter)
                {
                    _taskAwaiter = taskAwaiter;
                }

                public bool IsCompleted => _taskAwaiter.IsCompleted;

                public Unit GetResult() => Unit.Default;

                public void OnCompleted(Action continuation) => _taskAwaiter.OnCompleted(continuation);
            }
        }

        private sealed class TaskAwaitable<T> : IAwaitable<T>
        {
            private Task<T> _task;

            public TaskAwaitable(Task<T> task)
            {
                _task = task;
            }

            public IAwaiter<T> GetAwaiter() => new Awaiter(_task.GetAwaiter());

            private sealed class Awaiter : IAwaiter<T>
            {
                private TaskAwaiter<T> _taskAwaiter;

                public Awaiter(TaskAwaiter<T> taskAwaiter)
                {
                    _taskAwaiter = taskAwaiter;
                }

                public bool IsCompleted => _taskAwaiter.IsCompleted;

                public T GetResult() => _taskAwaiter.GetResult();

                public void OnCompleted(Action continuation) => _taskAwaiter.OnCompleted(continuation);
            }
        }

        private sealed class ResultAwaitable<T> : IAwaitable<T>, IAwaiter<T>
        {
            public ResultAwaitable(T value)
            {
                Value = value;
            }

            public T Value { get; }

            public bool IsCompleted => true;

            public IAwaiter<T> GetAwaiter() => this;

            public T GetResult() => Value;

            public void OnCompleted(Action continuation) => continuation();
        }

        private sealed class ExceptionAwaitable<T> : IAwaitable<T>, IAwaiter<T>
        {
            public ExceptionAwaitable(Exception exception)
            {
                Exception = exception;
            }

            public Exception Exception { get; }

            public bool IsCompleted => true;

            public IAwaiter<T> GetAwaiter() => this;

            public T GetResult() => throw Exception;

            public void OnCompleted(Action continuation) => continuation();
        }
    }
}
