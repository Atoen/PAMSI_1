using System.Collections;

namespace PAMSI_1.DataStructures;

public class SimpleArrayList<T> : IEnumerable<T>
{
    public SimpleArrayList()
    {
        _items = Array.Empty<T>();
    }

    public SimpleArrayList(int initialCapacity)
    {
        _items = initialCapacity switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Capacity cannot be less than 0"),
            0 => Array.Empty<T>(),
            _ => new T[initialCapacity]
        };
    }

    private const int DefaultCapacity = 4;

    public int Count { get; private set; }
    public int Capacity => _items.Length;

    private T[] _items;
    private int _version;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index),
                    "Index must be non-negative and less than list count.");
            }

            return _items[index];
        }
        set
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index),
                    "Index must be non-negative and less than list count.");
            }

            _items[index] = value;
            _version++;
        }
    }

    public void Add(T item)
    {
        _version++;

        if (Count < Capacity)
        {
            _items[Count] = item;
        }
        else
        {
            Grow(Count + 1);
            _items[Count] = item;
        }

        Count++;
    }

    public bool Remove(T item)
    {
        var index = IndexOf(item);

        if (index == -1) return false;

        Count--;
        _version++;

        Array.Copy(_items, index + 1, _items, index, Count - index);
        _items[Count] = default!;

        return true;
    }

    public void Clear()
    {
        _version++;
        Array.Clear(_items, 0, Count);

        Count = 0;
    }

    public bool Contains(T item) => Count != 0 && IndexOf(item) != -1;

    public bool Exists(Predicate<T> predicate) => FindIndex(predicate) != -1;

    public int FindIndex(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (Count == 0) return -1;

        for (var i = 0; i < Count; i++)
        {
            if (predicate(_items[i])) return i;
        }

        return -1;
    }

    public int IndexOf(T item) => Array.IndexOf(_items, item, 0, Count);

    public T? Find(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (Count == 0) return default;

        foreach (var item in _items)
        {
            if (predicate(item)) return item;
        }

        return default;
    }

    private void Grow(int requestedCapacity)
    {
        var newCapacity = Capacity == 0 ? DefaultCapacity : Capacity * 2;

        if (newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

        if (newCapacity < requestedCapacity) newCapacity = requestedCapacity;

        Array.Resize(ref _items, newCapacity);
    }

    public IEnumerator<T> GetEnumerator()
    {
        var version = _version;

        foreach (var item in _items)
        {
            if (_version != version) throw new InvalidOperationException("Collection changed during iteration.");

            yield return item;
        }

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}