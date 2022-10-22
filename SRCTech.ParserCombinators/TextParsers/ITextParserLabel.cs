using System.Collections.Generic;

namespace SRCTech.ParserCombinators.TextParsers
{
    public interface ITextParserLabel
    {
        string Label { get; }

        IReadOnlyList<string> Expectations { get; }

        ITextParserError ToError();
    }
}
