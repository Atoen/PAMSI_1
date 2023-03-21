namespace PAMSI_1.DataStructures;

public class SimplePriorityQueue<TItem, TPriority>
{
    public SimplePriorityQueue()
    {
        _entries = Array.Empty<Entry>();
        _comparer = Comparer<TPriority>.Default;
    }

    public SimplePriorityQueue(IComparer<TPriority> comparer)
    {
        _entries = Array.Empty<Entry>();
        _comparer = comparer;
    }

    private const int DefaultCapacity = 4;

    public int Count { get; private set; }
    public int Capacity => _entries.Length;
    public bool IsEmpty => Count == 0;

    private record struct Entry(TItem Item, TPriority Priority);
    private Entry[] _entries;

    private readonly IComparer<TPriority> _comparer;

    public void Enqueue(TItem item, TPriority priority)
    {
        if (Count == Capacity)
        {
            Grow(Count + 1);
        }

        var entry = new Entry(item, priority);
        _entries[Count] = entry;

        InsertEntry();

        Count++;
    }

    private void InsertEntry()
    {
        var childIndex = Count;
        while (childIndex > 0)
        {
            var parentIndex = (childIndex - 1) / 2;
            if (_comparer.Compare(_entries[childIndex].Priority, _entries[parentIndex].Priority) >= 0)
            {
                break;
            }

            (_entries[childIndex], _entries[parentIndex]) = (_entries[parentIndex], _entries[childIndex]);

            childIndex = parentIndex;
        }
    }

    public TItem Top()
    {
        if (Count < 1)
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        return _entries[0].Item;
    }

    public TItem Dequeue()
    {
        if (Count < 1)
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        var first = _entries[0];
        Count--;

        RemoveRootEntry();

        return first.Item;
    }

    private void RemoveRootEntry()
    {
        _entries[0] = _entries[Count];
        _entries[Count] = default;

        var parentIndex = 0;
        while (true)
        {
            var childIndex = parentIndex * 2 + 1;
            if (childIndex >= Count) break;

            var rightChildIndex = childIndex + 1;
            if (rightChildIndex < Count &&
                _comparer.Compare(_entries[rightChildIndex].Priority, _entries[childIndex].Priority) < 0)
            {
                childIndex = rightChildIndex;
            }

            if (_comparer.Compare(_entries[parentIndex].Priority, _entries[childIndex].Priority) <= 0)
            {
                break;
            }

            (_entries[parentIndex], _entries[childIndex]) = (_entries[childIndex], _entries[parentIndex]);

            parentIndex = childIndex;
        }
    }

    public void Clear()
    {
        Array.Clear(_entries, 0, Count);

        Count = 0;
    }

    private void Grow(int requestedCapacity)
    {
        var newCapacity = Capacity == 0 ? DefaultCapacity : Capacity * 2;

        if (newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

        if (newCapacity < requestedCapacity) newCapacity = requestedCapacity;

        Array.Resize(ref _entries, newCapacity);
    }
}
