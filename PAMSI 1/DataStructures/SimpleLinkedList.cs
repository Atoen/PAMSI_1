using System.Collections;

namespace PAMSI_1.DataStructures;

public class SimpleLinkedList<T> : IEnumerable<T>
{
    public class Node
    {
        public Node(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
        public Node? Next { get; set; }
    }

    private int _version;
    private Node? _head;
    private readonly IEqualityComparer<T> _equalityComparer = EqualityComparer<T>.Default;

    public void Add(T value)
    {
        var newNode = new Node(value) { Next = _head };

        _head = newNode;
        _version++;
    }

    public bool Remove(T value)
    {
        if (_head == null)
        {
            return false;
        }

        if (_equalityComparer.Equals(_head.Value, value))
        {
            _head = _head.Next;
            _version++;
            return true;
        }

        var current = _head;
        while (current.Next != null)
        {
            if (_equalityComparer.Equals(current.Next.Value, value))
            {
                current.Next = current.Next.Next;
                _version++;
                return true;
            }

            current = current.Next;
        }

        return false;
    }

    public bool Remove(Node? node)
    {
        if (node == null)
        {
            return false;
        }

        if (_head == node)
        {
            _head = _head.Next;
            _version++;
            return true;
        }

        var current = _head;
        while (current?.Next != null)
        {
            if (current.Next == node)
            {
                current.Next = node.Next;
                _version++;
                return true;
            }

            current = current.Next;
        }

        return false;
    }

    public void Clear()
    {
        _head = null;
        _version++;
    }

    public bool Contains(T value)
    {
        var current = _head;

        while (current != null)
        {
            if (_equalityComparer.Equals(current.Value, value))
            {
                return true;
            }

            current = current.Next;
        }
        return false;
    }

    public Node? Find(T value)
    {
        var current = _head;

        while (current != null)
        {
            if (_equalityComparer.Equals(current.Value, value))
            {
                return current;
            }

            current = current.Next;
        }

        return null;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var version = _version;

        var current = _head;

        while (current != null)
        {
            if (_version != version) throw new InvalidOperationException("Collection changed during iteration.");

            yield return current.Value;

            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}