using System.Collections.Generic;

namespace SRCTech.ParserCombinators.TextParsers
{
    public interface ITextParserError
    {
        int Position { get; }

        string Message { get; }

        IEnumerable<string> Expectations { get; }

        IReadOnlyCollection<ITextParserError> InnerErrors { get; }
    }
}
