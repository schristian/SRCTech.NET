using System;
using SRCTech.ParserCombinators.TextParsers;

namespace SRCTech.ParserCombinators
{
    public static partial class TextParser
    {
        public static ITextParser<TResult> Zip<T1, T2, TResult>(
            this ITextParser<T1> parser1,
            ITextParser<T2> parser2,
            Func<T1, T2, TResult> resultSelector)
        {
            return
                from value1 in parser1
                from value2 in parser2
                select resultSelector(value1, value2);
        }

        public static ITextParser<TResult> Zip<T1, T2, T3, TResult>(
            this ITextParser<T1> parser1,
            ITextParser<T2> parser2,
            ITextParser<T3> parser3,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            return
                from value1 in parser1
                from value2 in parser2
                from value3 in parser3
                select resultSelector(value1, value2, value3);
        }

        public static ITextParser<TResult> Zip<T1, T2, T3, T4, TResult>(
            this ITextParser<T1> parser1,
            ITextParser<T2> parser2,
            ITextParser<T3> parser3,
            ITextParser<T4> parser4,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            return
                from value1 in parser1
                from value2 in parser2
                from value3 in parser3
                from value4 in parser4
                select resultSelector(value1, value2, value3, value4);
        }
    }
}
