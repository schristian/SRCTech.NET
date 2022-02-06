using System;
using Moq;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public sealed class SharedDisposableTests
    {
        [Theory]
        [InlineData("disposable")]
        public static void SharedDisposable_Constructor_NullDisposable_ThrowsArgumentNullException(string parameterName)
        {
            var disposable = Disposable.Empty;

            NullTestingUtilities.TestNullParameters(
               () => new SharedDisposable(disposable),
               parameterName);
        }

        [Fact]
        public static void SharedDisposable_Dispose_NoDisposes_DisposeNeverCalled()
        {
            var disposable = new Mock<IDisposable>();

            var sharedDisposable = new SharedDisposable(disposable.Object);

            disposable.Verify(it => it.Dispose(), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void SharedDisposable_Dispose_MultipleDisposes_DisposeCalledOnce(int disposalCount)
        {
            var disposable = new Mock<IDisposable>();

            var sharedDisposable = new SharedDisposable(disposable.Object);

            for (int i = 0; i < disposalCount; i++)
            {
                sharedDisposable.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Once);
        }

        [Fact]
        public static void SharedDisposable_Clone_AlreadyDisposed_ThrowsObjectDisposedException()
        {
            var disposable = new Mock<IDisposable>();

            var sharedDisposable = new SharedDisposable(disposable.Object);
            sharedDisposable.Dispose();

            Assert.Throws<ObjectDisposedException>(() => sharedDisposable.Clone());
        }

        [Theory]
        [InlineData(false, false, 0)]
        [InlineData(false, true, 0)]
        [InlineData(true, false, 0)]
        [InlineData(true, true, 1)]
        public static void SharedDisposable_Clone_SingleClone_DisposeCalledAppropriately(
            bool disposeFirst,
            bool disposeSecond,
            int expectedDisposals)
        {
            var disposable = new Mock<IDisposable>();

            var sharedDisposable1 = new SharedDisposable(disposable.Object);
            var sharedDisposable2 = sharedDisposable1.Clone();

            if (disposeFirst)
            {
                sharedDisposable1.Dispose();
            }

            if (disposeSecond)
            {
                sharedDisposable2.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Exactly(expectedDisposals));
        }

        [Theory]
        [InlineData(false, false, false, 0)]
        [InlineData(false, false, true, 0)]
        [InlineData(false, true, false, 0)]
        [InlineData(false, true, true, 0)]
        [InlineData(true, false, false, 0)]
        [InlineData(true, false, true, 0)]
        [InlineData(true, true, false, 0)]
        [InlineData(true, true, true, 1)]
        public static void SharedDisposable_Clone_MultipleClones_DisposeCalledAppropriately(
            bool disposeFirst,
            bool disposeSecond,
            bool disposeThird,
            int expectedDisposals)
        {
            var disposable = new Mock<IDisposable>();

            var sharedDisposable1 = new SharedDisposable(disposable.Object);
            var sharedDisposable2 = sharedDisposable1.Clone();
            var sharedDisposable3 = sharedDisposable2.Clone();

            if (disposeFirst)
            {
                sharedDisposable1.Dispose();
            }

            if (disposeSecond)
            {
                sharedDisposable2.Dispose();
            }

            if (disposeThird)
            {
                sharedDisposable3.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Exactly(expectedDisposals));
        }
    }
}
