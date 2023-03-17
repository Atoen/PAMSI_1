namespace PAMSI_1.DataStructures;

public class PriorityQueue2<TItem, TPriority>
{
    public PriorityQueue2()
    {
        _entries = Array.Empty<(TItem item, TPriority priority)>();
        _comparer = Comparer<TPriority>.Default;
    }

    public PriorityQueue2(IComparer<TPriority> comparer)
    {
        _entries = Array.Empty<(TItem item, TPriority priority)>();
        _comparer = comparer;
    }

    private const int DefaultCapacity = 4;

    public int Count { get; private set; }
    public int Capacity => _entries.Length;
    public bool IsEmpty => Count == 0;

    private (TItem item, TPriority priority)[] _entries;
    private readonly IComparer<TPriority> _comparer;

    public void Enqueue(TItem item, TPriority priority)
    {
        if (Capacity == Count)
        {
            Grow(Count + 1);
        }

        MoveEntryUp((item, priority), Count);

        Count++;
    }

    public TItem Top()
    {
        if (Count < 1)
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        return _entries[0].item;
    }

    public TItem Dequeue()
    {
        if (Count < 1)
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        var item = _entries[0].item;

        RemoveRootEntry();

        return item;
    }

    public void Clear()
    {
        Array.Clear(_entries, 0, Count);

        Count = 0;
    }

    private void RemoveRootEntry()
    {
        var lastEntryIndex = --Count;

        if (lastEntryIndex > 0)
        {
            var lastEntry = _entries[lastEntryIndex];

            MoveEntryDown(lastEntry, 0);
        }

        _entries[lastEntryIndex] = default;
    }

    private void MoveEntryUp((TItem item, TPriority priority) entry, int index)
    {
        while (index > 0)
        {
            var parentIndex = GetParentIndex(index);
            var parent = _entries[parentIndex];

            if (_comparer.Compare(entry.priority, parent.priority) < 0)
            {
                _entries[index] = parent;
                index = parentIndex;
            }
            else
            {
                break;
            }
        }

        _entries[index] = entry;
    }

    private void MoveEntryDown((TItem item, TPriority priority) entry, int index)
    {
        int i;
        while ((i = GetFirstChildIndex(index)) < Count)
        {
            var minChild = _entries[i];
            var minChildIndex = i;

            var childIndexUpperBound = Math.Min(i + 4, Count);

            while (++i < childIndexUpperBound)
            {
                var nextChild = _entries[i];
                if (_comparer.Compare(nextChild.priority, minChild.priority) < 0)
                {
                    minChild = nextChild;
                    minChildIndex = i;
                }
            }

            if (_comparer.Compare(entry.priority, minChild.priority) <= 0)
            {
                break;
            }

            _entries[index] = minChild;
            index = minChildIndex;
        }

        _entries[index] = entry;
    }

    private static int GetParentIndex(int index) => (index - 1) >> 2;

    private static int GetFirstChildIndex(int index) => (index << 2) + 1;

    private void Grow(int requestedCapacity)
    {
        var newCapacity = Capacity == 0 ? DefaultCapacity : Capacity * 2;

        if (newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

        if (newCapacity < requestedCapacity) newCapacity = requestedCapacity;

        Array.Resize(ref _entries, newCapacity);
    }
}

