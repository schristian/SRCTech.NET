using System.Threading.Tasks;

namespace SRCTech.ParserCombinators.TextParsers
{
    public interface ITextParser<T>
    {
        ValueTask<TextParserResult<T>> Parse(ITextInput textInput);
    }
}
