using System.Threading.Tasks;
using SRCTech.Common.Async;

namespace SRCTech.ParserCombinators
{
    public interface IParser<in TToken, out TResult>
    {
        IAwaitable<IParserOutput<TState, TResult>> Parse<TState>(
            IParserInput<TState, TToken> input);
    }
}
