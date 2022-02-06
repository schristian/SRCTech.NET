using System;
using System.Collections.Generic;
using SRCTech.Common.Collections;
using Xunit;

namespace SRCTech.Common.Tests.Collections
{
    public static class CollectionHelperTests_GetCollectionHashCode
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
                { new[] { "A", "B" }, new[] { "B", "A" }, false, false },
                { new[] { "a", "B" }, new[] { "A", "b" }, false, true },
                { new[] { "a", "B" }, new[] { "b", "A" }, false, false },
                { new[] { "A", "A", "B" }, new[] { "A", "B", "B" }, false, false },
            };

        public static TestCases DefaultComparerTestCases { get; } =
            AllTestCases.PickColumns(0, 1, 2);

        public static TestCases CaseInsensitiveComparerTestCases { get; } =
            AllTestCases.PickColumns(0, 1, 3);

        [Theory]
        [InlineData("values")]
        public static void CollectionHelper_GetCollectionHashCode_NoComparer_ThrowsArgumentNullException(
            string parameterName)
        {
            var first = new[] { 1, 2, 3 };

            NullTestingUtilities.TestNullParameters(
                () => CollectionHelper.GetCollectionHashCode(first),
                parameterName);
        }

        [Theory]
        [InlineData("values")]
        [InlineData("equalityComparer")]
        public static void CollectionHelper_GetCollectionHashCode_WithComparer_ThrowsArgumentNullException(
            string parameterName)
        {
            var values = new[] { 1, 2, 3 };
            var equalityComparer = EqualityComparer<int>.Default;

            NullTestingUtilities.TestNullParameters(
                () => CollectionHelper.GetCollectionHashCode(values, equalityComparer),
                parameterName);
        }

        [Theory]
        [MemberData(nameof(DefaultComparerTestCases))]
        public static void CollectionHelper_GetCollectionHashCode_NoComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            var firstHashCode = CollectionHelper.GetCollectionHashCode(first);
            var secondHashCode = CollectionHelper.GetCollectionHashCode(second);

            Assert.Equal(expectedResult, firstHashCode == secondHashCode);
        }

        [Theory]
        [MemberData(nameof(DefaultComparerTestCases))]
        public static void CollectionHelper_GetCollectionHashCode_DefaultComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            var comparer = EqualityComparer<string>.Default;
            var firstHashCode = CollectionHelper.GetCollectionHashCode(first, comparer);
            var secondHashCode = CollectionHelper.GetCollectionHashCode(second, comparer);

            Assert.Equal(expectedResult, firstHashCode == secondHashCode);
        }

        [Theory]
        [MemberData(nameof(CaseInsensitiveComparerTestCases))]
        public static void CollectionHelper_GetCollectionHashCode_CaseInsensitiveComparer_ReturnsCorrectResult(
            string[] first,
            string[] second,
            bool expectedResult)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            var firstHashCode = CollectionHelper.GetCollectionHashCode(first, comparer);
            var secondHashCode = CollectionHelper.GetCollectionHashCode(second, comparer);

            Assert.Equal(expectedResult, firstHashCode == secondHashCode);
        }
    }
}
