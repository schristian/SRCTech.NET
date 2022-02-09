using System;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public static class DisposableTests_Empty
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Disposable_Empty_MultipleDisposes_DoesNotThrow(int disposalCount)
        {
            var disposable = Disposable.Empty;

            for (int i = 0; i < disposalCount; i++)
            {
                disposable.Dispose();
            }
        }

        [Fact]
        public static void Disposable_Empty_MultipleGets_ReturnsSameInstance()
        {
            var disposable1 = Disposable.Empty;
            var disposable2 = Disposable.Empty;

            Assert.Same(disposable1, disposable2);
        }
    }
}
