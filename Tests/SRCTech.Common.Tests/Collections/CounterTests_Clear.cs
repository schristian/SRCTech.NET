using System;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
       "Assertions",
       "xUnit2013:Do not use equality check to check for collection size.",
       Justification = "Testing 'Count' property of the Counter class.")]
    public static class CounterTests_Clear
    {
        [Fact]
        public static void Counter_Clear_AlreadyEmptyCounter_DoesNotThrow()
        {
            var counter = new Counter<string>();
            counter.Clear();

            Assert.Empty(counter);
            Assert.True(counter.IsEmpty);
            Assert.Equal(0, counter.Count);
            Assert.Equal(0, counter.TotalCount);
            Assert.False(counter.IsReadOnly);
        }

        [Fact]
        public static void Counter_Clear_CounterWithItems_DoesNotThrow()
        {
            var counter = new Counter<string>();
            counter["A"] = 1;
            counter["B"] = 2;
            counter.Clear();

            Assert.Empty(counter);
            Assert.True(counter.IsEmpty);
            Assert.Equal(0, counter.Count);
            Assert.Equal(0, counter.TotalCount);
            Assert.False(counter.IsReadOnly);
        }
    }
}
