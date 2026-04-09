
public interface IMap<K, V> : IEnumerable<IMap<K, V>.IEntry>
{
    public interface IEntry
    {
        K Key { get; }
        V Value { get; set; }
    }

    int Count { get; }
    bool IsEmpty { get; }
    IEnumerable<K> Keys { get; }
    IEnumerable<V> Values { get; }
    V this[K key] { get; set; }

    void Put(K key, V value);
    void Clear();
    void Remove(K key);
    bool ContainsKey(K key);
    bool ContainsValue(V value);
}
