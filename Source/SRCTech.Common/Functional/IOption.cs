namespace SRCTech.Common.Functional
{
    public interface IOption<out T>
    {
        bool HasValue { get; }

        T Value { get; }
    }
}
