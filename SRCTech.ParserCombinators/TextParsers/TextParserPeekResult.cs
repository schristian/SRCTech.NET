namespace SRCTech.ParserCombinators.TextParsers
{
    public struct TextParserPeekResult<T>
    {
        public TextParserPeekResult(TextParserResult<T> result, bool shouldRollBack)
        {
            Result = result;
            ShouldRollBack = shouldRollBack;
        }

        public TextParserResult<T> Result { get; }

        public bool ShouldRollBack { get; }
    }
}
