
namespace YourProject1.Implementations
{
    public class UnmutableMap<K, V> : IMap<K, V> where K : IComparable<K>
    {
        private readonly IMap<K, V> internalMap;
        public UnmutableMap(IMap<K, V> map)
        {
            internalMap = map ?? throw new ArgumentNullException(nameof(map));
        }
        public int Count => internalMap.Count;
        public bool IsEmpty => internalMap.IsEmpty;
        public IEnumerable<K> Keys => internalMap.Keys;
        public IEnumerable<V> Values => internalMap.Values;
        V IMap<K, V>.this[K key]
        {
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }
        public V this[K key] => internalMap[key];
        public void Put(K key, V value) => throw new InvalidOperationException("Cannot modify an UnmutableMap.");
        public void Clear() => throw new InvalidOperationException("Cannot modify an UnmutableMap.");
        public void Remove(K key) => throw new InvalidOperationException("Cannot modify an UnmutableMap.");
        public bool ContainsKey(K key) => internalMap.ContainsKey(key);
        public bool ContainsValue(V value) => internalMap.ContainsValue(value);
        public IEnumerator<IMap<K, V>.IEntry> GetEnumerator() => internalMap.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
