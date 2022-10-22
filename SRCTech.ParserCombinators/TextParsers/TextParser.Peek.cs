using System;
using System.Threading.Tasks;
using SRCTech.Common.Functional;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TSource> Peek<TSource>(
            this ITextParser<TSource> parser)
        {
            return Create(parser.PeekParse);
        }

        public static ValueTask<TextParserResult<TSource>> PeekParse<TSource>(
            this ITextParser<TSource> parser,
            ITextInput input)
        {
            return input.Peek(async i => (await parser.Parse(i)).ToPeekResult(_ => true));
        }
    }
}
