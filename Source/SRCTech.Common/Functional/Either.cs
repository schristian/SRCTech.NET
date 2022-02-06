using System;

namespace SRCTech.Common.Functional
{
    public static class Either
    {
        public static IEither<TLeft, TRight> Left<TLeft, TRight>(TLeft left)
        {
            return new LeftEither<TLeft, TRight>(left);
        }

        public static IEither<TLeft, TRight> Right<TLeft, TRight>(TRight right)
        {
            return new RightEither<TLeft, TRight>(right);
        }

        public static bool TryGetLeft<TLeft, TRight>(
            this IEither<TLeft, TRight> either,
            out TLeft value)
        {
            Guard.ThrowIfNull(either, nameof(either));

            if (either.Side == EitherSide.Left)
            {
                value = either.Left;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public static bool TryGetRight<TLeft, TRight>(
            this IEither<TLeft, TRight> either,
            out TRight value)
        {
            Guard.ThrowIfNull(either, nameof(either));

            if (either.Side == EitherSide.Right)
            {
                value = either.Right;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        private sealed class LeftEither<TLeft, TRight> : IEither<TLeft, TRight>
        {
            public LeftEither(TLeft left)
            {
                Left = left;
            }

            public EitherSide Side => EitherSide.Left;

            public TLeft Left { get; }

            public TRight Right => throw new InvalidOperationException($"{nameof(IEither<TLeft, TRight>)} does not have a right value.");
        }

        private sealed class RightEither<TLeft, TRight> : IEither<TLeft, TRight>
        {
            public RightEither(TRight right)
            {
                Right = right;
            }

            public EitherSide Side => EitherSide.Right;

            public TLeft Left => throw new InvalidOperationException($"{nameof(IEither<TLeft, TRight>)} does not have a left value.");

            public TRight Right { get; }
        }
    }
}
