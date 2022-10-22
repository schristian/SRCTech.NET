using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<char> Char(char targetChar)
        {
            var errorString = $"Expected '{targetChar}'";
            return Advance().Where(x => x == targetChar).WithExpectation(errorString);
        }

        public static ITextParser<char> NotChar(char targetChar)
        {
            var errorString = $"Expected any character but '{targetChar}'";
            return Advance().Where(x => x != targetChar).WithExpectation(errorString);
        }

        public static ITextParser<char> InRange(char startChar, char endChar)
        {
            var errorString = $"Expected character in '{startChar}'-'{endChar}'";
            return Advance().Where(x => x >= startChar && x <= endChar).WithExpectation(errorString);
        }

        public static ITextParser<char> NotInRange(char startChar, char endChar)
        {
            var errorString = $"Expected character not in '{startChar}'-'{endChar}'";
            return Advance().Where(x => x < startChar || x > endChar).WithExpectation(errorString);
        }

        public static ITextParser<char> OneOf(params char[] chars)
        {
            var errorString = $"Expected one of: ${string.Join(", ", chars.Select(c => $"'{c}'"))}";
            return Advance().WhereOneOf(chars);
        }

        public static ITextParser<char> NotOneOf(params char[] chars)
        {
            var errorString = $"Expected any character but one of: ${string.Join(", ", chars.Select(c => $"'{c}'"))}";
            return Advance().WhereNotOneOf(chars);
        }

        public static ITextParser<char> InCategory(UnicodeCategory category)
        {
            var errorString = $"Expected character in category: {category}";
            return Advance().Where(x => CharUnicodeInfo.GetUnicodeCategory(x) == category).WithExpectation(errorString);
        }

        public static ITextParser<string> LiteralString(string literalString)
        {
            return Sequence(literalString.Select(Char)).Select(_ => literalString);
        }

        public static ITextParser<string> Whitespace()
        {
            return Advance().Where(char.IsWhiteSpace).ZeroOrMore().AsString();
        }

        public static ITextParser<TSource> IgnoreLeadingWhitespace<TSource>(
            this ITextParser<TSource> parser)
        {
            return Whitespace().Then(parser);
        }

        public static ITextParser<TSource> IgnoreTrailingWhitespace<TSource>(
            this ITextParser<TSource> parser)
        {
            return parser.ThenIgnore(Whitespace());
        }

        public static ITextParser<TSource> IgnoreWhitespace<TSource>(
            this ITextParser<TSource> parser)
        {
            return parser.IgnoreLeadingWhitespace().IgnoreTrailingWhitespace();
        }

        public static ITextParser<string> AsString(
            this ITextParser<char> parser)
        {
            return parser.Select(x => x.ToString());
        }

        public static ITextParser<string> AsString<TSource>(
            this ITextParser<TSource> parser)
            where TSource : IEnumerable<char>
        {
            return parser.Select(
                xs =>
                {
                    var stringBuilder = new StringBuilder();
                    foreach (var x in xs)
                    {
                        stringBuilder.Append(x);
                    }

                    return stringBuilder.ToString();
                });
        }

        public static ITextParser<string> ConcatStrings<TSource>(
            this ITextParser<TSource> parser)
            where TSource : IEnumerable<string>
        {
            return parser.Select(
                xs =>
                {
                    var stringBuilder = new StringBuilder();
                    foreach (var x in xs)
                    {
                        stringBuilder.Append(x);
                    }

                    return stringBuilder.ToString();
                });
        }
    }
}
