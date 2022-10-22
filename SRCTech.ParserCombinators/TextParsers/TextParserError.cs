using System;
using System.Collections.Generic;

namespace SRCTech.ParserCombinators.TextParsers
{
    public sealed class TextParserError : ITextParserError
    {
        public TextParserError(string expectation, int position)
        {
            Position = position;
            Expectation = expectation;
        }

        public int Position { get; }

        public string Message => $"Expected: {Expectation}.";

        public string Expectation { get; }

        public IEnumerable<string> Expectations => new[] { Expectation };

        public IReadOnlyCollection<ITextParserError> InnerErrors => Array.Empty<ITextParserError>();

        public static TextParserError Create(ITextInput input, string expectation)
        {
            return new TextParserError(expectation, input.CurrentPosition);
        }
    }
}
