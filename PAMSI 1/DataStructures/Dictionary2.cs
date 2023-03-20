namespace PAMSI_1.DataStructures;

public class Dictionary2<TKey, TValue>
{
    public Dictionary2()
    {
        _items = new HashSet3<KeyValuePair<TKey, TValue>>();
    }

    private readonly HashSet3<KeyValuePair<TKey, TValue>> _items;
    
    public void Add(TKey key, TValue value)
    {
        if (ContainsKey(key))
        {
            throw new ArgumentException("An element with the same key already exists in the dictionary.");
        }
        
        _items.Add(new KeyValuePair<TKey, TValue>(key, value));
    }

    public bool ContainsKey(TKey key)
    {
        // _items.Contains()
        
        // foreach (KeyValuePair<TKey, TValue> item in _items)
        // {
        //     if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
        //     {
        //         return true;
        //     }
        // }

        return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        // Search for the key in the set
        // foreach (KeyValuePair<TKey, TValue> item in _items)
        // {
        //     if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
        //     {
        //         value = item.Value;
        //         return true;
        //     }
        // }

        value = default(TValue);
        return false;
    }

    public TValue this[TKey key]
    {
        get
        {
            // Search for the key in the set
            // foreach (KeyValuePair<TKey, TValue> item in _items)
            // {
            //     if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
            //     {
            //         return item.Value;
            //     }
            // }

            throw new KeyNotFoundException("The given key was not present in the dictionary.");
        }
        set
        {
            // Search for the key in the set and update its value
            // foreach (KeyValuePair<TKey, TValue> item in _items)
            // {
            //     if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
            //     {
            //         _items.Remove(item);
            //         _items.Add(new KeyValuePair<TKey, TValue>(key, value));
            //         return;
            //     }
            // }
            
            _items.Add(new KeyValuePair<TKey, TValue>(key, value));
        }
    }

    public void Remove(TKey key)
    {

        // // Search for the key in the set and remove it
        // foreach (KeyValuePair<TKey, TValue> item in _items)
        // {
        //     if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
        //     {
        //         _items.Remove(item);
        //         return;
        //     }
        // }

        throw new KeyNotFoundException("The given key was not present in the dictionary.");
    }
}