using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRCTech.Common.Functional;

using MatchTextParser = SRCTech.ParserCombinators.TextParsers.ITextParser<string>;

namespace SRCTech.ParserCombinators.TextParsers
{
    public sealed class RegexTextParser : ITextParser<MatchTextParser>
    {
        private static ITextParser<char> Char { get; } = TextParser
            .Or(
                TextParser.Char('\x0009'),
                TextParser.Char('\x000A'),
                TextParser.Char('\x000D'),
                TextParser.InRange('\x0020', '\xD7FF'),
                TextParser.InRange('\xE000', '\xFFFD'))
            .WithExpectation("Expected valid regex character");

        private static MatchTextParser Letters { get; } = TextParser
            .Or(
                TextParser.InRange('a', 'z'),
                TextParser.InRange('A', 'Z'))
            .OneOrMore()
            .AsString()
            .WithExpectation("Expected sequence of letters");

        private static ITextParser<int> Integer { get; } = TextParser
            .InRange('0', '9')
            .OneOrMore()
            .AsString()
            .Select(int.Parse)
            .WithExpectation("Expected integer");

        private static ITextParser<Unit> StartAnchor { get; } = TextParser
            .Char('^')
            .WithValue(Unit.Default)
            .WithExpectation("Expected start anchor");

        private static ITextParser<Unit> EndAnchor { get; } = TextParser
            .Char('$')
            .WithValue(Unit.Default)
            .WithExpectation("Expected end anchor");

        private static ITextParser<Func<MatchTextParser, MatchTextParser>> RangeQuantifier { get; } = TextParser
            .Char('{')
            .Then(TextParser.Or(
                Integer.Zip(
                    TextParser.Char('}'),
                    (x, _) => (Func<MatchTextParser, MatchTextParser>)((parser) => parser
                        .Several(x)
                        .ConcatStrings())),
                Integer.Zip(
                    TextParser.Char(','),
                    TextParser.Char('}'),
                    (x, _, _) => (Func<MatchTextParser, MatchTextParser>)((parser) => parser
                        .Several(x, int.MaxValue)
                        .ConcatStrings())),
                Integer.Zip(
                    TextParser.Char(','),
                    Integer,
                    TextParser.Char('}'),
                    (x, _, y, _) => (Func<MatchTextParser, MatchTextParser>)((parser) => parser
                        .Several(x, y)
                        .ConcatStrings()))))
            .WithExpectation("Expected range quantifier");

        public ValueTask<TextParserResult<MatchTextParser>> Parse(ITextInput textInput)
        {
            throw new NotImplementedException();
        }
    }
}
