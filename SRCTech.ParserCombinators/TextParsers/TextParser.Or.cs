using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SRCTech.Common.Functional;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TSource> Or<TSource>(
            IEnumerable<ITextParser<TSource>> parsers)
        {
            return OrImpl(parsers);
        }

        public static ITextParser<TSource> Or<TSource>(
            params ITextParser<TSource>[] parsers)
        {
            return OrImpl(parsers);
        }

        public static ITextParser<TSource> StrictOr<TSource>(
            IEnumerable<ITextParser<TSource>> parsers)
        {
            return StrictOrImpl(parsers);
        }

        public static ITextParser<TSource> StrictOr<TSource>(
            params ITextParser<TSource>[] parsers)
        {
            return StrictOrImpl(parsers);
        }

        private static ITextParser<TSource> OrImpl<TSource>(
            IEnumerable<ITextParser<TSource>> parsers)
        {
            return Create(async i =>
            {
                List<ITextParserError> errors = null;

                foreach (var parser in parsers)
                {
                    var result = await parser.TryParse(i);
                    if (result.HasValue)
                    {
                        return result;
                    }

                    errors ??= new List<ITextParserError>();
                    errors.Add(result.Error);
                }

                return new TextParserResult<TSource>(OrTextParserError.Create(i, errors));
            });
        }

        private static ITextParser<TSource> StrictOrImpl<TSource>(
            IEnumerable<ITextParser<TSource>> parsers)
        {
            return Create(async i =>
            {
                int startPosition = i.CurrentPosition;
                List<ITextParserError> errors = null;

                foreach (var parser in parsers)
                {
                    var result = await parser.Parse(i);
                    if (result.HasValue || i.CurrentPosition != startPosition)
                    {
                        return result;
                    }

                    errors ??= new List<ITextParserError>();
                    errors.Add(result.Error);
                }

                return new TextParserResult<TSource>(OrTextParserError.Create(i, errors));
            });
        }

        private sealed class OrTextParserError : ITextParserError
        {
            public OrTextParserError(IReadOnlyCollection<ITextParserError> innerErrors, int position)
            {
                Position = position;
                InnerErrors = innerErrors;
            }

            public int Position { get; }

            public string Message => $"Expected one of the following: {string.Join(", ", Expectations)}.";

            public IEnumerable<string> Expectations => InnerErrors.SelectMany(error => error.Expectations);

            public IReadOnlyCollection<ITextParserError> InnerErrors { get; }

            public static OrTextParserError Create(ITextInput input, IReadOnlyCollection<ITextParserError> errors)
            {
                return new OrTextParserError(errors, input.CurrentPosition);
            }
        }
    }
}
