using System;
using System.Collections.Generic;
using Moq;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    public sealed class CounterTests_Constructor
    {
        [Fact]
        public static void Counter_Constructor_WithoutEqualityComparer_CreatesEmptyInstance()
        {
            var counter = new Counter<string>();

            Assert.Empty(counter);
            Assert.True(counter.IsEmpty);
            Assert.Equal(0, counter.TotalCount);
            Assert.False(counter.IsReadOnly);
            Assert.Same(EqualityComparer<string>.Default, counter.Comparer);
        }

        [Theory]
        [InlineData("equalityComparer")]
        public static void Counter_Constructor_NullEqualityComparer_ThrowsArgumentNullException(string parameterName)
        {
            var equalityComparer = StringComparer.Ordinal;

            NullTestingUtilities.TestNullParameters(
                    () => new Counter<string>(equalityComparer),
                    parameterName);
        }

        [Fact]
        public static void Counter_Constructor_WithEqualityComparer_CreatesEmptyInstance()
        {
            var equalityComparer = new Mock<IEqualityComparer<string>>();
            var counter = new Counter<string>(equalityComparer.Object);

            Assert.Empty(counter);
            Assert.True(counter.IsEmpty);
            Assert.Equal(0, counter.TotalCount);
            Assert.False(counter.IsReadOnly);
            Assert.Same(equalityComparer.Object, counter.Comparer);
        }
    }
}
