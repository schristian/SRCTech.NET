using System;
using System.Collections.Generic;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    public static class CollectionHelperTests_GetUnorderedCollectionHashCode
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
                { new[] { "A", "A", "B", "B" }, new[] { "A", "B", "A", "B" }, true, true },
            };

        public static TestCases DefaultComparerTestCases { get; } =
            AllTestCases.PickColumns(0, 1, 2);

        public static TestCases CaseInsensitiveComparerTestCases { get; } =
            AllTestCases.PickColumns(0, 1, 3);

        [Theory]
        [InlineData("values")]
        public static void CollectionHelper_GetUnorderedCollectionHashCode_NoComparer_ThrowsArgumentNullException(
            string parameterName)
        {
            var first = new[] { 1, 2, 3 };

            NullTestingUtilities.TestNullParameters(
                () => CollectionHelper.GetUnorderedCollectionHashCode(first),
                parameterName);
        }

        [Theory]
        [InlineData("values")]
        [InlineData("equalityComparer")]
        public static void CollectionHelper_GetUnorderedCollectionHashCode_WithComparer_ThrowsArgumentNullException(
            string parameterName)
        {
            var values = new[] { 1, 2, 3 };
            var equalityComparer = EqualityComparer<int>.Default;

            NullTestingUtilities.TestNullParameters(
                () => CollectionHelper.GetUnorderedCollectionHashCode(values, equalityComparer),
                parameterName);
        }

        [Theory]
        [MemberData(nameof(DefaultComparerTestCases))]
        public static void CollectionHelper_GetUnorderedCollectionHashCode_NoComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            var firstHashCode = CollectionHelper.GetUnorderedCollectionHashCode(first);
            var secondHashCode = CollectionHelper.GetUnorderedCollectionHashCode(second);

            Assert.Equal(expectedResult, firstHashCode == secondHashCode);
        }

        [Theory]
        [MemberData(nameof(DefaultComparerTestCases))]
        public static void CollectionHelper_GetUnorderedCollectionHashCode_DefaultComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            var comparer = EqualityComparer<string>.Default;
            var firstHashCode = CollectionHelper.GetUnorderedCollectionHashCode(first, comparer);
            var secondHashCode = CollectionHelper.GetUnorderedCollectionHashCode(second, comparer);

            Assert.Equal(expectedResult, firstHashCode == secondHashCode);
        }

        [Theory]
        [MemberData(nameof(CaseInsensitiveComparerTestCases))]
        public static void CollectionHelper_GetUnorderedCollectionHashCode_CaseInsensitiveComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            var firstHashCode = CollectionHelper.GetUnorderedCollectionHashCode(first, comparer);
            var secondHashCode = CollectionHelper.GetUnorderedCollectionHashCode(second, comparer);

            Assert.Equal(expectedResult, firstHashCode == secondHashCode);
        }
    }
}
