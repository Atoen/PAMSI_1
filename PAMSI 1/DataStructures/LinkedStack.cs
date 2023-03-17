namespace PAMSI_1.DataStructures;

public class LinkedStack<T> : IStack<T>
{
    private LinkedStackNode<T>? _head;

    public bool IsEmpty => _head == null;

    public T Pop()
    {
        if (_head == null)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        var data = _head.Data;
        _head = _head.Next;

        return data;
    }

    public T Top()
    {
        if (_head == null)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        return _head.Data;
    }

    public void Push(T item)
    {
        var newNode = new LinkedStackNode<T>(item)
        {
            Next = _head
        };

        _head = newNode;
    }

    public void Clear()
    {
        _head = null;
    }
}

public class LinkedStackNode<T>
{
    public LinkedStackNode(T value)
    {
        Data = value;
    }

    public T Data { get; set; }
    public LinkedStackNode<T>? Next { get; set; }

    public void Clear()
    {
        Data = default!;
        Next = null;
    }
}