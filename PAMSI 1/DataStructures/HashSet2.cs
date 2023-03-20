using System.Collections;

namespace PAMSI_1.DataStructures;

public class HashSet2<T>
{
    private readonly T[]?[] _buckets;
    private readonly int _bucketCount;
    
    private int _version;

    public HashSet2(int size = 10)
    {
        _buckets = new T[size][];
        _bucketCount = size;
    }

    public void Add(T value)
    {
        var index = GetIndex(value);
        ref var bucket = ref _buckets[index];
        
        if (bucket == null)
        {
            bucket = Array.Empty<T>();
        }
        else if (Contains(value)) return;
        
        Array.Resize(ref bucket, bucket.Length + 1);
        bucket[^1] = value;

        _version++;
        Count++;
    }

    public bool Contains(T value)
    {
        var index = GetIndex(value);
        ref var bucket = ref _buckets[index];
        
        if (bucket == null) return false;

        return Array.IndexOf(bucket, value) != -1;
    }

    public bool Remove(T value)
    {
        var index = GetIndex(value);
        ref var bucket = ref _buckets[index];

        if (bucket == null) return false;

        var i = Array.IndexOf(bucket, value);
        if (i == -1) return false;
        
        Array.Copy(bucket, i + 1, bucket, i, bucket.Length - i - 1);
        Array.Resize(ref _buckets[index], bucket.Length - 1);

        _version++;
        Count--;
        
        return true;
    }

    public int Count { get; private set; }

    private int GetIndex(T value)
    {
        return Math.Abs(value!.GetHashCode() % _bucketCount);
    }
}