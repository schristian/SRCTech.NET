using System;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    public static class CounterTests_ContainsKey
    {
        [Theory]
        [InlineData("item")]
        public static void Counter_ContainsKey_NullItem_ThrowsArgumentNullException(string parameterName)
        {
            var item = "A";
            var counter = new Counter<string>();

            NullTestingUtilities.TestNullParameters(
                    () => counter.ContainsKey(item),
                    parameterName);
        }

        [Fact]
        public static void Counter_ContainsKey_NotContainedItem_ReturnsFalse()
        {
            var item = "A";
            var counter = new Counter<string>();

            Assert.False(counter.ContainsKey(item));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Counter_ContainsKey_ContainedItem_ReturnsTrue(int itemCount)
        {
            var item = "A";
            var counter = new Counter<string>();
            counter[item] = itemCount;

            Assert.True(counter.ContainsKey(item));
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

            Assert.False(counter.ContainsKey(item));
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

            Assert.True(counter.ContainsKey(item2));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, 5)]
        [InlineData(1, 0)]
        [InlineData(5, 0)]
        [InlineData(1, 1)]
        [InlineData(1, 5)]
        [InlineData(5, 1)]
        [InlineData(5, 5)]
        public static void Counter_ContainsKey_MultipleItems_ReturnsCorrectResults(
            int item1Count,
            int item2Count)
        {
            var item1 = "A";
            var item2 = "B";
            var counter = new Counter<string>();
            counter[item1] = item1Count;
            counter[item2] = item2Count;

            Assert.Equal(item1Count != 0, counter.ContainsKey(item1));
            Assert.Equal(item2Count != 0, counter.ContainsKey(item2));
        }
    }
}
