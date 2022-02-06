using System.Collections.Generic;
using SRCTech.Common.Comparers;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    public static class CollectionHelperTests_CompareToCollection
    {
        public static TestCases AllTestCases { get; } = 
            new TestCases()
            {
                { new int[] { }, new int[] { }, 0, 0 },
                { new int[] { }, new int[] { 1 }, -1, -1 },
                { new int[] { 1 }, new int[] { }, 1, 1 },
                { new[] { 1 }, new[] { 1 }, 0, 0 },
                { new[] { 1 }, new[] { 2 }, -1, 1 },
                { new[] { 2 }, new[] { 1 }, 1, -1 },
                { new[] { 1, 2 }, new[] { 2 }, -1, 1 },
                { new[] { 2, 1 }, new[] { 1 }, 1, -1 },
                { new[] { 1, 2 }, new[] { 2, 1 }, -1, 1 },
                { new[] { 2, 1 }, new[] { 1, 2 }, 1, -1 },
                { new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, 0, 0 },
                { new[] { 1, 2, 3 }, new[] { 1, 3, 3 }, -1, 1 },
                { new[] { 1, 3, 3 }, new[] { 1, 2, 3 }, 1, -1 },
                { new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4 }, -1, -1 },
                { new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3 }, 1, 1 },
            };

        public static TestCases DefaultComparerTestCases { get; } =
            AllTestCases.PickColumns(0, 1, 2);

        public static TestCases ReversedComparerTestCases { get; } =
            AllTestCases.PickColumns(0, 1, 3);

        [Theory]
        [InlineData("first")]
        [InlineData("second")]
        public static void CollectionHelper_CompareToCollection_NoComparer_ThrowsArgumentNullException(
            string parameterName)
        {
            var first = new[] { 1, 2, 3 };
            var second = new[] { 1, 2, 3 };

            NullTestingUtilities.TestNullParameters(
                () => CollectionHelper.CompareToCollection(first, second),
                parameterName);
        }

        [Theory]
        [InlineData("first")]
        [InlineData("second")]
        [InlineData("comparer")]
        public static void CollectionHelper_CompareToCollection_WithComparer_ThrowsArgumentNullException(
            string parameterName)
        {
            var first = new[] { 1, 2, 3 };
            var second = new[] { 1, 2, 3 };
            var comparer = Comparer<int>.Default;

            NullTestingUtilities.TestNullParameters(
                () => CollectionHelper.CompareToCollection(first, second, comparer),
                parameterName);
        }

        [Theory]
        [MemberData(nameof(DefaultComparerTestCases))]
        public static void CollectionHelper_CompareToCollection_NoComparer_ReturnsCorrectResult(
            int[] first,
            int[] second,
            int expectedResult)
        {
            Assert.Equal(
                expectedResult,
                CollectionHelper.CompareToCollection(first, second));
        }

        [Theory]
        [MemberData(nameof(DefaultComparerTestCases))]
        public static void CollectionHelper_CompareToCollection_DefaultComparer_ReturnsCorrectResult(
            int[] first,
            int[] second,
            int expectedResult)
        {
            var comparer = Comparer<int>.Default;

            Assert.Equal(
                expectedResult,
                CollectionHelper.CompareToCollection(first, second, comparer));
        }

        [Theory]
        [MemberData(nameof(ReversedComparerTestCases))]
        public static void CollectionHelper_CompareToCollection_ReversedComparer_ReturnsCorrectResult(
            int[] first,
            int[] second,
            int expectedResult)
        {
            var comparer = Comparer<int>.Default.Reverse();

            Assert.Equal(
                expectedResult,
                CollectionHelper.CompareToCollection(first, second, comparer));
        }
    }
}
