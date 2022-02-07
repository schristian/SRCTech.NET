using System;
using System.Linq;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    public sealed class CounterTests_Indexer
    {
        [Fact]
        public static void Counter_Indexer_EmptyCounter_ReturnsZero()
        {
            var item = "A";

            var counter = new Counter<string>();

            Assert.Equal(0, counter[item]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Counter_Indexer_SingleItem_ReturnsCorrectCount(int itemCount)
        {
            var item1 = "A";
            var item2 = "B";

            var counter = new Counter<string>();
            counter[item1] = itemCount;

            Assert.False(counter.IsEmpty);
            Assert.Single(counter);
            Assert.Equal(itemCount, counter.TotalCount);
            Assert.Equal(itemCount, counter[item1]);
            Assert.Equal(0, counter[item2]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public static void Counter_IndexerGet_NonDefaultComparer_ReturnsCorrectCount(int itemCount)
        {
            var item1 = "A";
            var item2 = "a";

            var counter = new Counter<string>(StringComparer.OrdinalIgnoreCase);
            counter[item1] = itemCount;

            Assert.False(counter.IsEmpty);
            Assert.Single(counter);
            Assert.Equal(itemCount, counter.TotalCount);
            Assert.Equal(itemCount, counter[item1]);
            Assert.Equal(itemCount, counter[item2]);
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
        public static void Counter_Indexer_MultipleItems_ReturnsCorrectCounts(
            int item1Count,
            int item2Count)
        {
            var item1 = "A";
            var item2 = "B";

            var expectedCount = (new[] { item1Count, item2Count }).Count(it => it != 0);
            var expectedIsEmpty = expectedCount == 0;

            var counter = new Counter<string>();
            counter[item1] = item1Count;
            counter[item2] = item2Count;

            Assert.Equal(expectedIsEmpty, counter.IsEmpty);
            Assert.Equal(expectedCount, counter.Count);
            Assert.Equal(item1Count + item2Count, counter.TotalCount);
            Assert.Equal(item1Count, counter[item1]);
            Assert.Equal(item2Count, counter[item2]);
        }
    }
}
