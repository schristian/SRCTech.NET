using System;
using System.Threading.Tasks;
using SRCTech.Common.Functional;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TSource> Try<TSource>(
            this ITextParser<TSource> parser)
        {
            return Create(parser.TryParse);
        }

        public static ValueTask<TextParserResult<TSource>> TryParse<TSource>(
            this ITextParser<TSource> parser,
            ITextInput input)
        {
            return input.Peek(async i => (await parser.Parse(i)).ToPeekResult(r => !r.HasValue));
        }
    }
}
