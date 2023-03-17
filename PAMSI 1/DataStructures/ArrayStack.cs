namespace PAMSI_1.DataStructures;

public class ArrayStack<T> : IStack<T>
{
    public ArrayStack()
    {
        _data = Array.Empty<T>();
    }

    private const int DefaultCapacity = 4;

    private T[] _data;

    public int Count { get; private set; }
    public int Capacity => _data.Length;
    public bool IsEmpty => Count == 0;

    public T Pop()
    {
        if (Count < 1)
        {
            throw new InvalidOperationException("The stack is empty.");
        }

        Count--;

        var item = _data[Count];
        _data[Count] = default!;

        return item;
    }

    public T Top()
    {
        if (Count < 1)
        {
            throw new InvalidOperationException("The stack is empty.");
        }

        return _data[Count - 1];
    }

    public void Push(T item)
    {
        if (Count < Capacity)
        {
            _data[Count] = item;
        }
        else
        {
            Grow(Count + 1);
            _data[Count] = item;
        }

        Count++;
    }

    public void Clear()
    {
        Array.Clear(_data, 0, Count);

        Count = 0;
    }

    private void Grow(int requestedCapacity)
    {
        var newCapacity = Capacity == 0 ? DefaultCapacity : Capacity * 2;

        if (newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

        if (newCapacity < requestedCapacity) newCapacity = requestedCapacity;

        Array.Resize(ref _data, newCapacity);
    }
}
