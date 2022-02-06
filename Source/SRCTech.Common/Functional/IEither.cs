namespace SRCTech.Common.Functional
{
    public interface IEither<out TLeft, out TRight>
    {
        EitherSide Side { get; }

        TLeft Left { get; }

        TRight Right { get; }
    }
}
