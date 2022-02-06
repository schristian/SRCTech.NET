using System;
using Moq;
using SRCTech.Common.Functional;
using Xunit;

namespace SRCTech.Common.Tests.Functional
{
    public sealed class EitherTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Either_Left_ReturnsEitherWithLeftValue(object value)
        {
            var either = Either.Left<object, object>(value);

            Assert.Equal(EitherSide.Left, either.Side);
            Assert.Equal(value, either.Left);
            Assert.Throws<InvalidOperationException>(() => either.Right);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Either_Right_ReturnsEitherWithRightValue(object value)
        {
            var either = Either.Right<object, object>(value);

            Assert.Equal(EitherSide.Right, either.Side);
            Assert.Throws<InvalidOperationException>(() => either.Left);
            Assert.Equal(value, either.Right);
        }

        [Theory]
        [InlineData("either")]
        public static void Either_TryGetLeft_NullEither_ThrowsArgumentNullException(string parameterName)
        {
            var either = Either.Left<int, int>(5);
            int value;

            NullTestingUtilities.TestNullParameters(
               () => Either.TryGetLeft(either, out value),
               parameterName);
        }

        [Fact]
        public static void Either_TryGetLeft_RightEither_ReturnsFalse()
        {
            var either = new Mock<IEither<int, int>>(MockBehavior.Strict);
            either.Setup(it => it.Side).Returns(EitherSide.Right);

            Assert.False(Either.TryGetLeft(either.Object, out int value));
        }

        [Fact]
        public static void Either_TryGetLeft_LeftEither_ReturnsTrue()
        {
            var expectedValue = 5;
            var either = new Mock<IEither<int, int>>(MockBehavior.Strict);
            either.Setup(it => it.Side).Returns(EitherSide.Left);
            either.Setup(it => it.Left).Returns(expectedValue);

            Assert.True(Either.TryGetLeft(either.Object, out int actualValue));
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("either")]
        public static void Either_TryGetRight_NullEither_ThrowsArgumentNullException(string parameterName)
        {
            var either = Either.Right<int, int>(5);
            int value;

            NullTestingUtilities.TestNullParameters(
               () => Either.TryGetRight(either, out value),
               parameterName);
        }

        [Fact]
        public static void Either_TryGetRight_LeftEither_ReturnsFalse()
        {
            var either = new Mock<IEither<int, int>>(MockBehavior.Strict);
            either.Setup(it => it.Side).Returns(EitherSide.Left);

            Assert.False(Either.TryGetRight(either.Object, out int value));
        }

        [Fact]
        public static void Either_TryGetRight_RightEither_ReturnsTrue()
        {
            var expectedValue = 5;
            var either = new Mock<IEither<int, int>>(MockBehavior.Strict);
            either.Setup(it => it.Side).Returns(EitherSide.Right);
            either.Setup(it => it.Right).Returns(expectedValue);

            Assert.True(Either.TryGetRight(either.Object, out int actualValue));
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
