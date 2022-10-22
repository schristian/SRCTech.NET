using System;
using System.Threading.Tasks;

namespace SRCTech.ParserCombinators.TextParsers
{
    public interface ITextInput
    {
        int CurrentPosition { get; }

        char CurrentCharacter { get; }

        ValueTask<bool> TryAdvance();

        ValueTask<TextParserResult<T>> Peek<T>(
            Func<ITextInput, ValueTask<TextParserPeekResult<T>>> peekFunc);
    }
}
