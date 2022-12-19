using System.Collections.Generic;

namespace SRCTech.ParserCombinators
{
    public interface IEnumerableParser<in TToken, out TResult> : IParser<TToken, IReadOnlyCollection<TResult>>
    {
        IAsyncEnumerable<IParserOutput<TState, TResult>> ParseMany<TState>(
            IParserInput<TState, TToken> input);
    }
}
