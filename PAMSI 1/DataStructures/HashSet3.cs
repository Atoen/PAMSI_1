using System.Collections;
using System.Runtime.InteropServices.JavaScript;

namespace PAMSI_1.DataStructures;

public class HashSet3<T> : IEnumerable<T>
{
    private const int DefaultCapacity = 4;

    private SimpleLinkedList<T>?[] _buckets;

    public HashSet3() : this(DefaultCapacity)
    {
    }

    public HashSet3(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }
        
        _buckets = new SimpleLinkedList<T>[capacity];
    }
    
    public int Count { get; private set; }
    private int _version;

    public void Add(T item)
    {
        if (Contains(item)) return;

        if (Count >= _buckets.Length)
        {
            Resize();
        }

        ref var bucket = ref GetBucket(item!.GetHashCode());
        
        bucket ??= new SimpleLinkedList<T>();

        bucket.Add(item);

        _version++;
        Count++;
    }

    public bool Remove(T item)
    {
        ref var bucket = ref GetBucket(item!.GetHashCode());
        
        if (bucket == null) return false;
        
        var node = bucket.Find(item);
        if (node == null) return false;

        bucket.Remove(node);

        _version++;
        Count--;
        return true;
    }

    public bool Contains(T item)
    {
        ref var bucket = ref GetBucket(item!.GetHashCode());

        if (bucket == null) return false;

        return bucket.Contains(item);
    }
    
    public void Clear()
    {
        Array.Clear(_buckets);
        Count = 0;
    }

    private int GetBucketIndex(int hashCode)
    {
        return (hashCode & 0x7FFFFFFF) % _buckets.Length;
    }

    private ref SimpleLinkedList<T>? GetBucket(int hashCode)
    {
        var index = (hashCode & 0x7FFFFFFF) % _buckets.Length;
        return ref _buckets[index];
    }

    private void Resize()
    {
        var newCapacity = _buckets.Length * 2;
        var newBuckets = new SimpleLinkedList<T>?[newCapacity];

        foreach (var bucket in _buckets)
        {
            if (bucket == null) continue;
            
            foreach (var item in bucket)
            {
                var bucketIndex = GetBucketIndex(item!.GetHashCode());
                
                ref var newBucket = ref newBuckets[bucketIndex];

                newBucket ??= new SimpleLinkedList<T>();
                newBucket.Add(item);
            }
        }

        _version++;
        _buckets = newBuckets;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var version = _version;
        
        foreach (var bucket in _buckets)
        {
            if (bucket == null) continue;
            
            foreach (var item in bucket)
            {
                if (_version != version)
                {
                    throw new Exception("fr");
                }
                
                yield return item;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}