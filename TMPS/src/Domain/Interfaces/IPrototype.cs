namespace TMPS.Domain.Interfaces;

public interface IPrototype<T>
{
    public T Clone();
}