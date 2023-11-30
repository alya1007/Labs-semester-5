namespace TMPS.Domain.Interfaces;

public interface IIterator<T>
{
    bool HasNext();
    T Next();
}
