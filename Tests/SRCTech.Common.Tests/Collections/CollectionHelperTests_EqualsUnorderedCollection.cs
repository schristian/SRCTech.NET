using System;
using System.Collections.Generic;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    public static class CollectionHelperTests_EqualsUnorderedCollection
    {
        public static TestCases AllTestCases { get; } =
            new TestCases()
            {
                { new string[] { }, new string[] { }, true, true },
                { new string[] { }, new string[] { "A" }, false, false },
                { new[] { "A" }, new[] { "A" }, true, true },
                { new[] { "a" }, new[] { "A" }, false, true },
                { new[] { "A" }, new[] { "B" }, false, false },
                { new[] { "A" }, new[] { "A", "A" }, false, false },
                { new[] { "A" }, new[] { "A", "B" }, false, false },
                { new[] { "A" }, new[] { "B", "A" }, false, false },
                { new[] { "A", "B" }, new[] { "A", "B" }, true, true },
                { new[] { "A", "B" }, new[] { "B", "A" }, true, true },
                { new[] { "a", "B" }, new[] { "A", "b" }, false, true },
                { new[] { "a", "B" }, new[] { "b", "A" }, false, true },
                { new[] { "A", "A", "B" }, new[] { "A", "B", "B" }, false, false },
            };

        public static TestCases DefaultComparerTestCases { get; } =
            AllTestCases.PickColumns(0, 1, 2);

        public static TestCases CaseInsensitiveComparerTestCases { get; } =
            AllTestCases.PickColumns(0, 1, 3);

        [Theory]
        [InlineData("first")]
        [InlineData("second")]
        public static void CollectionHelper_EqualsUnorderedCollection_NoComparer_ThrowsArgumentNullException(
            string parameterName)
        {
            var first = new[] { 1, 2, 3 };
            var second = new[] { 1, 2, 3 };

            NullTestingUtilities.TestNullParameters(
                () => CollectionHelper.EqualsUnorderedCollection(first, second),
                parameterName);
        }

        [Theory]
        [InlineData("first")]
        [InlineData("second")]
        [InlineData("equalityComparer")]
        public static void CollectionHelper_EqualsUnorderedCollection_WithComparer_ThrowsArgumentNullException(
            string parameterName)
        {
            var first = new[] { 1, 2, 3 };
            var second = new[] { 1, 2, 3 };
            var comparer = EqualityComparer<int>.Default;

            NullTestingUtilities.TestNullParameters(
                () => CollectionHelper.EqualsUnorderedCollection(first, second, comparer),
                parameterName);
        }

        [Theory]
        [MemberData(nameof(DefaultComparerTestCases))]
        public static void CollectionHelper_CompareToCollection_NoComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            Assert.Equal(
                expectedResult,
                CollectionHelper.EqualsUnorderedCollection(first, second));

            Assert.Equal(
                expectedResult,
                CollectionHelper.EqualsUnorderedCollection(second, first));
        }

        [Theory]
        [MemberData(nameof(DefaultComparerTestCases))]
        public static void CollectionHelper_CompareToCollection_DefaultComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            var comparer = EqualityComparer<string>.Default;

            Assert.Equal(
                expectedResult,
                CollectionHelper.EqualsUnorderedCollection(first, second, comparer));

            Assert.Equal(
                expectedResult,
                CollectionHelper.EqualsUnorderedCollection(second, first, comparer));
        }

        [Theory]
        [MemberData(nameof(CaseInsensitiveComparerTestCases))]
        public static void CollectionHelper_CompareToCollection_CaseInsensitiveComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            Assert.Equal(
                expectedResult,
                CollectionHelper.EqualsUnorderedCollection(first, second, comparer));

            Assert.Equal(
                expectedResult,
                CollectionHelper.EqualsUnorderedCollection(second, first, comparer));
        }
    }
}
