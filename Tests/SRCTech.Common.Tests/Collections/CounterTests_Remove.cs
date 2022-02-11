using System;
using System.Linq;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
       "Assertions",
       "xUnit2013:Do not use equality check to check for collection size.",
       Justification = "Testing 'Count' property of the Counter class.")]
    public static class CounterTests_Remove
    {
        [Theory]
        [InlineData("item")]
        public static void Counter_Remove_NullItem_ThrowsArgumentNullException(string parameterName)
        {
            var item = "A";
            var counter = new Counter<string>();

            NullTestingUtilities.TestNullParameters(
                    () => counter.Remove(item),
                    parameterName);
        }

        [Fact]
        public static void Counter_Remove_NotContainedItem_ReturnsCorrectCount()
        {
            var item = "A";
            var counter = new Counter<string>();

            Assert.Equal(-1, counter.Remove(item));

            Assert.False(counter.IsEmpty);
            Assert.Single(counter);
            Assert.Equal(1, counter.Count);
            Assert.Equal(-1, counter.TotalCount);
            Assert.Equal(-1, counter[item]);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(-5)]
        public static void Counter_Remove_ContainedItem_ReturnsCorrectCount(int previousCount)
        {
            var item = "A";
            var expectedCount = previousCount - 1;
            var counter = new Counter<string>();
            counter[item] = previousCount;

            Assert.Equal(expectedCount, counter.Remove(item));

            Assert.False(counter.IsEmpty);
            Assert.Single(counter);
            Assert.Equal(1, counter.Count);
            Assert.Equal(expectedCount, counter.TotalCount);
            Assert.Equal(expectedCount, counter[item]);
        }

        [Fact]
        public static void Counter_Remove_ItemWouldBeRemoved_RemovesItem()
        {
            var item = "A";
            var counter = new Counter<string>();
            counter[item] = 1;

            Assert.Equal(0, counter.Remove(item));

            Assert.True(counter.IsEmpty);
            Assert.Empty(counter);
            Assert.Equal(0, counter.Count);
            Assert.Equal(0, counter.TotalCount);
            Assert.Equal(0, counter[item]);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(-5)]
        public static void Counter_Remove_NonDefaultComparer_ReturnsCorrectCount(int previousCount)
        {
            var item1 = "A";
            var item2 = "a";
            var expectedCount = previousCount - 1;
            var counter = new Counter<string>(StringComparer.OrdinalIgnoreCase);
            counter[item1] = previousCount;

            Assert.Equal(expectedCount, counter.Remove(item2));

            Assert.False(counter.IsEmpty);
            Assert.Single(counter);
            Assert.Equal(1, counter.Count);
            Assert.Equal(expectedCount, counter.TotalCount);
            Assert.Equal(expectedCount, counter[item1]);
            Assert.Equal(expectedCount, counter[item2]);
        }

        [Fact]
        public static void Counter_Remove_MultipleItems_ReturnsCorrectCounts()
        {
            var item1 = "A";
            var item2 = "B";

            var counter = new Counter<string>();
            Assert.Equal(-1, counter.Remove(item1));
            Assert.Equal(-1, counter.Remove(item2));

            Assert.False(counter.IsEmpty);
            Assert.Equal(2, counter.Count);
            Assert.Equal(-2, counter.TotalCount);
            Assert.Equal(-1, counter[item1]);
            Assert.Equal(-1, counter[item2]);
        }

        [Theory]
        [InlineData("item")]
        public static void Counter_Remove_WithAmount_NullItem_ThrowsArgumentNullException(string parameterName)
        {
            var item = "A";
            var amount = 1;
            var counter = new Counter<string>();

            NullTestingUtilities.TestNullParameters(
                    () => counter.Remove(item, amount),
                    parameterName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(-5)]
        public static void Counter_Remove_WithAmount_NotContainedItem_ReturnsCorrectCount(int amount)
        {
            var item = "A";
            var expectedCount = -amount;
            var counter = new Counter<string>();

            Assert.Equal(expectedCount, counter.Remove(item, amount));

            Assert.False(counter.IsEmpty);
            Assert.Single(counter);
            Assert.Equal(1, counter.Count);
            Assert.Equal(expectedCount, counter.TotalCount);
            Assert.Equal(expectedCount, counter[item]);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(-5)]
        public static void Counter_Remove_WithAmount_ContainedItem_ReturnsCorrectCount(int amount)
        {
            var item = "A";
            var expectedCount = 1 - amount;
            var counter = new Counter<string>();
            counter[item] = 1;

            Assert.Equal(expectedCount, counter.Remove(item, amount));

            Assert.False(counter.IsEmpty);
            Assert.Single(counter);
            Assert.Equal(1, counter.Count);
            Assert.Equal(expectedCount, counter.TotalCount);
            Assert.Equal(expectedCount, counter[item]);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(-5)]
        public static void Counter_Remove_WithAmount_ItemWouldBeRemoved_RemovesItem(int amount)
        {
            var item = "A";
            var counter = new Counter<string>();
            counter[item] = amount;

            Assert.Equal(0, counter.Remove(item, amount));

            Assert.True(counter.IsEmpty);
            Assert.Empty(counter);
            Assert.Equal(0, counter.Count);
            Assert.Equal(0, counter.TotalCount);
            Assert.Equal(0, counter[item]);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(-5)]
        public static void Counter_Remove_WithAmount_NonDefaultComparer_ReturnsCorrectCount(int amount)
        {
            var item1 = "A";
            var item2 = "a";
            var expectedCount = 1 - amount;
            var counter = new Counter<string>(StringComparer.OrdinalIgnoreCase);
            counter[item1] = 1;

            Assert.Equal(expectedCount, counter.Remove(item2, amount));

            Assert.False(counter.IsEmpty);
            Assert.Single(counter);
            Assert.Equal(1, counter.Count);
            Assert.Equal(expectedCount, counter.TotalCount);
            Assert.Equal(expectedCount, counter[item1]);
            Assert.Equal(expectedCount, counter[item2]);
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
        public static void Counter_Remove_WithAmount_MultipleItems_ReturnsCorrectCounts(
            int item1Count,
            int item2Count)
        {
            var item1 = "A";
            var item2 = "B";
            var expectedCount = (new[] { item1Count, item2Count }).Count(it => it != 0);
            var expectedIsEmpty = expectedCount == 0;
            var counter = new Counter<string>();

            Assert.Equal(-item1Count, counter.Remove(item1, item1Count));
            Assert.Equal(-item2Count, counter.Remove(item2, item2Count));

            Assert.Equal(expectedIsEmpty, counter.IsEmpty);
            Assert.Equal(expectedCount, counter.Count);
            Assert.Equal(-item1Count + -item2Count, counter.TotalCount);
            Assert.Equal(-item1Count, counter[item1]);
            Assert.Equal(-item2Count, counter[item2]);
        }
    }
}
