using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SRCTech.Common.Tests
{
    public sealed class TestCases : IReadOnlyCollection<object[]>
    {
        private readonly List<object[]> _testCases;

        public TestCases()
        {
            _testCases = new List<object[]>();
        }

        public TestCases(IEnumerable<object[]> testCases)
        {
            _testCases = testCases.ToList();
        }

        public int Count => _testCases.Count;

        public TestCases Add(params object[] arguments)
        {
            _testCases.Add(arguments);
            return this;
        }

        public TestCases PickColumns(params int[] columns)
        {
            return new TestCases(
                _testCases.Select(xs => columns.Select(c => xs[c]).ToArray()));
        }

        public IEnumerator<object[]> GetEnumerator() => _testCases.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
