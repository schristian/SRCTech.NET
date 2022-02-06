using System;
using Moq;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public sealed class SharedHandleTests
    {
        [Theory]
        [InlineData("disposable")]
        public static void SharedHandle_Constructor_NullDisposable_ThrowsArgumentNullException(string parameterName)
        {
            var value = 5;
            var disposable = Disposable.Empty;

            NullTestingUtilities.TestNullParameters(
               () => new SharedHandle<int>(value, disposable),
               parameterName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void SharedHandle_Dispose_NoDisposes_DisposeNeverCalled(object value)
        {
            var disposable = new Mock<IDisposable>();

            var sharedHandle = new SharedHandle<object>(value, disposable.Object);

            disposable.Verify(it => it.Dispose(), Times.Never);
            Assert.Equal(value, sharedHandle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void SharedHandle_Dispose_MultipleDisposes_DisposeCalledOnce(int disposalCount)
        {
            var value = 5;
            var disposable = new Mock<IDisposable>();

            var sharedHandle = new SharedHandle<int>(value, disposable.Object);

            for (int i = 0; i < disposalCount; i++)
            {
                sharedHandle.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => sharedHandle.Value);
        }

        [Fact]
        public static void SharedHandle_Clone_AlreadyDisposed_ThrowsObjectDisposedException()
        {
            var value = 5;
            var disposable = new Mock<IDisposable>();

            var sharedHandle = new SharedHandle<int>(value, disposable.Object);
            sharedHandle.Dispose();

            Assert.Throws<ObjectDisposedException>(() => sharedHandle.Clone());
        }

        [Theory]
        [InlineData(false, false, 0)]
        [InlineData(false, true, 0)]
        [InlineData(true, false, 0)]
        [InlineData(true, true, 1)]
        public static void SharedHandle_Clone_SingleClone_DisposeCalledAppropriately(
            bool disposeFirst,
            bool disposeSecond,
            int expectedDisposals)
        {
            var value = 5;
            var disposable = new Mock<IDisposable>();

            var sharedHandle1 = new SharedHandle<int>(value, disposable.Object);
            var sharedHandle2 = sharedHandle1.Clone();

            if (disposeFirst)
            {
                sharedHandle1.Dispose();
            }

            if (disposeSecond)
            {
                sharedHandle2.Dispose();
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
        public static void SharedHandle_Clone_MultipleClones_DisposeCalledAppropriately(
            bool disposeFirst,
            bool disposeSecond,
            bool disposeThird,
            int expectedDisposals)
        {
            var value = 5;
            var disposable = new Mock<IDisposable>();

            var sharedHandle1 = new SharedHandle<int>(value, disposable.Object);
            var sharedHandle2 = sharedHandle1.Clone();
            var sharedHandle3 = sharedHandle2.Clone();

            if (disposeFirst)
            {
                sharedHandle1.Dispose();
            }

            if (disposeSecond)
            {
                sharedHandle2.Dispose();
            }

            if (disposeThird)
            {
                sharedHandle3.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Exactly(expectedDisposals));
        }
    }
}
