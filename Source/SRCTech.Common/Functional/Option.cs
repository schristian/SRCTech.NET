using System;

namespace SRCTech.Common.Functional
{
    public static partial class Option
    {
        public static IOption<T> None<T>()
        {
            return NoneOption<T>.Instance;
        }

        public static IOption<T> Some<T>(T value)
        {
            return new SomeOption<T>(value);
        }

        public static IOption<T> FromNullable<T>(T? value)
            where T : struct
        {
            return value.HasValue ? Some(value.Value) : None<T>();
        }

        public static IOption<T> FromNullable<T>(T value)
        {
            return value != null ? Some(value) : None<T>();
        }

        public static bool TryGetValue<T>(
            this IOption<T> option,
            out T value)
        {
            Guard.ThrowIfNull(option, nameof(option));

            if (option.HasValue)
            {
                value = option.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        private sealed class NoneOption<T> : IOption<T>
        {
            public static NoneOption<T> Instance { get; } = new NoneOption<T>();

            public bool HasValue => false;

            public T Value => throw new InvalidOperationException($"{nameof(IOption<T>)} does not have a value.");
        }

        private sealed class SomeOption<T> : IOption<T>
        {
            public SomeOption(T value)
            {
                Value = value;
            }

            public bool HasValue => true;

            public T Value { get; }
        }
    }
}
