using System;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    public static class CounterTests_TryGetValue
    {
        [Theory]
        [InlineData("item")]
        public static void Counter_TryGetValue_NullItem_ThrowsArgumentNullException(string parameterName)
        {
            var item = "A";
            var counter = new Counter<string>();

            NullTestingUtilities.TestNullParameters(
                    () => counter.ContainsKey(item),
                    parameterName);
        }

        [Fact]
        public static void Counter_TryGetValue_NotContainedItem_ReturnsFalse()
        {
            var item = "A";
            var counter = new Counter<string>();

            Assert.False(counter.TryGetValue(item, out var actualCount));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Counter_TryGetValue_ContainedItem_ReturnsTrue(int itemCount)
        {
            var item = "A";
            var counter = new Counter<string>();
            counter[item] = itemCount;

            Assert.True(counter.TryGetValue(item, out var actualCount));
            Assert.Equal(itemCount, actualCount);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Counter_ContainsKey_ItemRemoved_ReturnsFalse(int itemCount)
        {
            var item = "A";
            var counter = new Counter<string>();
            counter[item] = itemCount;
            counter[item] = 0;

            Assert.False(counter.TryGetValue(item, out var actualCount));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Counter_ContainsKey_NonDefaultComparer_ReturnsCorrectResult(int itemCount)
        {
            var item1 = "A";
            var item2 = "a";
            var counter = new Counter<string>(StringComparer.OrdinalIgnoreCase);
            counter[item1] = itemCount;

            Assert.True(counter.TryGetValue(item2, out var actualCount));
            Assert.Equal(itemCount, actualCount);
        }
    }
}
