using System;
using SRCTech.Common.Lifetimes;
using Xunit;

namespace SRCTech.Common.Tests.Lifetimes
{
    public sealed class HandleTests_CreateWithoutDisposable
    {
        [Theory]
        [InlineData(null)]
        [InlineData(5)]
        [InlineData("String Value")]
        public static void Handle_CreateWithoutDisposable_NoDisposes_ValueAccessible(object value)
        {
            var handle = Handle.CreateWithoutDisposable(value);

            Assert.Equal(value, handle.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Handle_Create_MultipleDisposes_ValueThrowsObjectDisposedException(int disposalCount)
        {
            var value = 5;
            var handle = Handle.CreateWithoutDisposable(value);

            for (int i = 0; i < disposalCount; i++)
            {
                handle.Dispose();
            }

            Assert.Throws<ObjectDisposedException>(() => handle.Value);
        }
    }
}
