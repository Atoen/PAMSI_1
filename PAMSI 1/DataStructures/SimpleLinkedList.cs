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

    private Node? _head;
    private readonly IEqualityComparer<T> _equalityComparer = EqualityComparer<T>.Default;

    public void Add(T value)
    {
        var newNode = new Node(value) { Next = _head };

        _head = newNode;

        // if (_head == null)
        // {
        //     _head = newNode;
        // }
        // else
        // {
        //     var current = _head;
        //     
        //     while (current.Next != null)
        //     {
        //         current = current.Next;
        //     }
        //     
        //     current.Next = newNode;
        // }
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
            return true;
        }

        var current = _head;
        while (current.Next != null)
        {
            if (_equalityComparer.Equals(current.Next.Value, value))
            {
                current.Next = current.Next.Next;
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
            return true;
        }

        var current = _head;
        while (current?.Next != null)
        {
            if (current.Next == node)
            {
                current.Next = node.Next;
                return true;
            }
            
            current = current.Next;
        }

        return false;
    }

    public void Clear()
    {
        _head = null;
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
        return new Enumerator(_head);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private struct Enumerator : IEnumerator<T>
    {
        private Node? _currentNode;
        private Node? _head;

        public Enumerator(Node head)
        {
            _currentNode = null;
            _head = head;
        }

        public T Current => _currentNode.Value;

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            if (_currentNode == null)
            {
                _currentNode = _head;
            }
            else
            {
                _currentNode = _currentNode.Next;
            }
            
            return _currentNode != null;
        }

        public void Reset()
        {
            _currentNode = null;
        }
    }

    // public IEnumerator<T> GetEnumerator()
    // {
    //     var current = _head;
    //     
    //     while (current != null)
    //     {
    //         yield return current.Value;
    //         current = current.Next;
    //     }
    // }
    //
    // IEnumerator IEnumerable.GetEnumerator()
    // {
    //     return GetEnumerator();
    // }
}