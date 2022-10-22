namespace SRCTech.Common.Async
{
    public interface IAwaitable<out T>
    {
        IAwaiter<T> GetAwaiter();
    }
}
