namespace PAMSI_1.DataStructures;

public interface IStack<T>
{
    bool IsEmpty { get; }

    T Pop();
    T Top();
    void Push(T item);
    void Clear();
}
