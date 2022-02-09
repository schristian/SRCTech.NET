using System;
using Moq;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public static class HandleTests_CreateShared
    {
        [Theory]
        [InlineData("disposable")]
        public static void Handle_CreateShared_NullDisposable_ThrowsArgumentNullException(string parameterName)
        {
            var value = 5;
            var disposable = Disposable.Empty;

            NullTestingUtilities.TestNullParameters(
               () => Handle.CreateShared(value, disposable),
               parameterName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Handle_CreateShared_NoDisposesWithDisposable_DisposeNeverCalled(object value)
        {
            var disposable = new Mock<IDisposable>();

            var sharedHandle = Handle.CreateShared(value, disposable.Object);

            disposable.Verify(it => it.Dispose(), Times.Never);
            Assert.Equal(value, sharedHandle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_CreateShared_MultipleDisposesWithDisposable_DisposeCalledOnce(int disposalCount)
        {
            var value = 5;
            var disposable = new Mock<IDisposable>();

            var sharedHandle = Handle.CreateShared(value, disposable.Object);

            for (int i = 0; i < disposalCount; i++)
            {
                sharedHandle.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => sharedHandle.Value);
        }

        [Theory]
        [InlineData("handle")]
        public static void Handle_CreateShared_NullHandle_ThrowsArgumentNullException(string parameterName)
        {
            var handle = Handle.CreateWithoutDisposable(5);

            NullTestingUtilities.TestNullParameters(
               () => Handle.CreateShared(handle),
               parameterName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Handle_CreateShared_NoDisposesWithHandle_DisposeNeverCalled(object value)
        {
            var handle = new Mock<IHandle<object>>();
            handle.Setup(it => it.Value).Returns(value);

            var sharedHandle = Handle.CreateShared(handle.Object);

            handle.Verify(it => it.Dispose(), Times.Never);
            Assert.Equal(value, sharedHandle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_CreateShared_MultipleDisposesWithHandle_DisposeCalledOnce(int disposalCount)
        {
            var value = 5;
            var handle = new Mock<IHandle<int>>();
            handle.Setup(it => it.Value).Returns(value);

            var sharedHandle = Handle.CreateShared(handle.Object);

            for (int i = 0; i < disposalCount; i++)
            {
                sharedHandle.Dispose();
            }

            handle.Verify(it => it.Dispose(), Times.Once);
            Assert.Throws<ObjectDisposedException>(() => sharedHandle.Value);
        }
    }
}
