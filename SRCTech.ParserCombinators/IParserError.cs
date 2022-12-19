using System.Collections.Generic;

namespace SRCTech.ParserCombinators
{
    public interface IParserError
    {
        string Message { get; }

        IEnumerable<string> ExpectedLabels { get; }

        IReadOnlyCollection<IParserError> InnerErrors { get; }
    }
}
