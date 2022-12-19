using System;

namespace SRCTech.ParserCombinators
{
    public interface IParserOutput<out TState, out TResult>
    {
        TState State { get; }

        bool HasValue { get; }

        TResult Value { get; }

        IParserError Error { get; }
    }
}
