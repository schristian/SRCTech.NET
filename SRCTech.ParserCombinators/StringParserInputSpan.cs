using System;

namespace SRCTech.ParserCombinators
{
    public record struct StringParserInputSpan(
        string String,
        int StartPosition,
        int EndPosition)
    {
        public int Length => EndPosition - StartPosition;

        public override string ToString()
        {
            return String.Substring(StartPosition, Length);
        }
    }
}
