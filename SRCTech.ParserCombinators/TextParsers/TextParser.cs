using System;
using System.Threading.Tasks;
using SRCTech.Common.Functional;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TSource> Create<TSource>(
            Func<ITextInput, ValueTask<TextParserResult<TSource>>> parseFunc)
        {
            return new AnonymousParser<TSource>(parseFunc);
        }

        public static ITextParser<TSource> Lazy<TSource>(
            Func<ITextParser<TSource>> parserFactory)
        {
            var lazy = new Lazy<ITextParser<TSource>>(parserFactory);
            return Create(i => lazy.Value.Parse(i));
        }

        public static ITextParser<TSource> FromResult<TSource>(Func<ITextInput, TextParserResult<TSource>> resultSelector)
        {
            return Create(i => new ValueTask<TextParserResult<TSource>>(resultSelector(i)));
        }

        public static ITextParser<TSource> FromValue<TSource>(TSource value)
        {
            return FromResult(_ => new TextParserResult<TSource>(value));
        }

        public static ITextParser<TSource> FromExpectation<TSource>(string expectation)
        {
            return FromResult(i => i.CreateErrorResult<TSource>(expectation));
        }

        public static ITextParser<char> Advance()
        {
            return Create(
                async i =>
                {
                    if (await i.TryAdvance())
                    {
                        return new TextParserResult<char>(i.CurrentCharacter);
                    }

                    return i.CreateErrorResult<char>("not end of input");
                });
        }

        public static ITextParser<Unit> EndOfInput()
        {
            return Create(
                async i =>
                {
                    if (await i.TryAdvance())
                    {
                        return i.CreateErrorResult<Unit>("end of input");
                    }

                    return new TextParserResult<Unit>(Unit.Default);
                });
        }

        public static ITextParser<TResult> WithValue<TSource, TResult>(
            this ITextParser<TSource> parser,
            TResult value)
        {
            return parser.Select(_ => value);
        }

        public static ITextParser<TSource> WithExpectation<TSource>(
            this ITextParser<TSource> parser,
            string expectation)
        {
            return Create(
                async i =>
                {
                    var result = await parser.Parse(i);
                    if (result.HasValue)
                    {
                        return result;
                    }

                    return i.CreateErrorResult<TSource>(expectation);
                });
        }

        public static ITextParser<TSource> PredictWith<TSource, TPredict>(
            this ITextParser<TSource> sourceParser,
            ITextParser<TPredict> predictionParser)
        {
            return predictionParser.Peek().Then(sourceParser);
        }

        private static TextParserResult<TSource> CreateErrorResult<TSource>(
            this ITextInput input,
            string expectation)
        {
            return new TextParserResult<TSource>(new TextParserError(expectation, input.CurrentPosition));
        }

        private sealed class AnonymousParser<TSource> : ITextParser<TSource>
        {
            private readonly Func<ITextInput, ValueTask<TextParserResult<TSource>>> _parseFunc;

            public AnonymousParser(Func<ITextInput, ValueTask<TextParserResult<TSource>>> parseFunc)
            {
                _parseFunc = parseFunc;
            }

            public ValueTask<TextParserResult<TSource>> Parse(ITextInput input)
            {
                return _parseFunc(input);
            }
        }
    }
}
