using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using SRCTech.Common.Async;
using SRCTech.Common.Functional;

namespace SRCTech.ParserCombinators
{
    public static partial class Parser
    {
        public static IParser<TToken, TResult> FromValue<TToken, TResult>(
            TResult value)
        {
            return new FromValueParser<TToken, TResult>(value);
        }

        public static IParser<TToken, TResult> FromError<TToken, TResult>(
            IParserError error)
        {
            return new FromErrorParser<TToken, TResult>(error);
        }

        public static IParser<TToken, TToken> Advance<TToken>()
        {
            return AdvanceParser<TToken>.Instance;
        }

        public static IParser<TToken, TResult> Peek<TToken, TResult>(
            this IParser<TToken, TResult> parser)
        {
            return new PeekParser<TToken, TResult>(parser);
        }

        public static IParser<TToken, TResult> Try<TToken, TResult>(
            this IParser<TToken, TResult> source)
        {
            return new TryParser<TToken, TResult>(source);
        }

        public static async Task<IParserOutput<TState, IReadOnlyCollection<TResult>>> CombineOutputSequence<TState, TToken, TResult>(
            this IParserInput<TState, TToken> input,
            IAsyncEnumerable<IParserOutput<TState, TResult>> outputs)
        {
            var listOutput = await input.CreateValueOutput(new List<TResult>());
            await foreach (var output in outputs)
            {
                listOutput = await input.CombineOutputSequence(
                    listOutput,
                    output,
                    static (list, value) => { list.Add(value); return list; });
            }

            return listOutput;
        }

        private sealed record FromValueParser<TToken, TResult>(
            TResult Value) : IParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return input.CreateValueOutput(Value);
            }
        }

        private sealed record FromErrorParser<TToken, TResult>(
            IParserError Error) : IParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return input.CreateErrorOutput<TResult>(Error);
            }
        }

        private sealed class AdvanceParser<TToken> : IParser<TToken, TToken>
        {
            public static AdvanceParser<TToken> Instance { get; } = new AdvanceParser<TToken>();

            public IAwaitable<IParserOutput<TState, TToken>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return input.Advance();
            }
        }

        private sealed record PeekParser<TToken, TResult>(
            IParser<TToken, TResult> InnerParser) : IParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return input.Peek(InnerParser);
            }
        }

        private sealed record TryParser<TToken, TResult>(
            IParser<TToken, TResult> InnerParser) : IParser<TToken, TResult>
        {
            public IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
                IParserInput<TState, TToken> input)
            {
                return input.Try(InnerParser);
            }
        }
    }
}
