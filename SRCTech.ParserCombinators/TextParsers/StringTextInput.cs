using System;
using System.Threading.Tasks;

namespace SRCTech.ParserCombinators.TextParsers
{
    public sealed class StringTextInput : ITextInput
    {
        public StringTextInput(string text) : this(text, -1)
        {
        }

        private StringTextInput(string text, int position)
        {
            Text = text;
            CurrentPosition = position;
        }

        public string Text { get; }

        public int CurrentPosition { get; private set; }

        public char CurrentCharacter => Text[CurrentPosition];

        public ValueTask<bool> TryAdvance()
        {
            CurrentPosition += 1;
            return new ValueTask<bool>(CurrentPosition < Text.Length);
        }

        public async ValueTask<TextParserResult<T>> Peek<T>(
            Func<ITextInput, ValueTask<TextParserPeekResult<T>>> peekFunc)
        {
            var newInput = new StringTextInput(Text, CurrentPosition);
            var peekResult = await peekFunc(newInput);

            if (!peekResult.ShouldRollBack)
            {
                CurrentPosition = newInput.CurrentPosition;
            }

            return peekResult.Result;
        }
    }
}
