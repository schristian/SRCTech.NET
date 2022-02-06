using System;
using Moq;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public sealed class DisposableTests_CreateShared
    {
        [Theory]
        [InlineData("disposable")]
        public static void Disposable_CreateShared_NullDisposable_ThrowsArgumentNullException(string parameterName)
        {
            var disposable = Disposable.Empty;

            NullTestingUtilities.TestNullParameters(
               () => Disposable.CreateShared(disposable),
               parameterName);
        }

        [Fact]
        public static void Disposable_CreateShared_NoDisposesWithDisposable_DisposeNeverCalled()
        {
            var disposable = new Mock<IDisposable>();

            var sharedDisposable = Disposable.CreateShared(disposable.Object);

            disposable.Verify(it => it.Dispose(), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Disposable_CreateShared_MultipleDisposesWithDisposable_DisposeCalledOnce(int disposalCount)
        {
            var disposable = new Mock<IDisposable>();

            var sharedDisposable = Disposable.CreateShared(disposable.Object);

            for (int i = 0; i < disposalCount; i++)
            {
                sharedDisposable.Dispose();
            }

            disposable.Verify(it => it.Dispose(), Times.Once);
        }
    }
}
