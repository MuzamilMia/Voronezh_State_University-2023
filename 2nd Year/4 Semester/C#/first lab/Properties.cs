
using YourProject1.Implementations;

namespace YourProject.Implementations
{
    public class Properties<K, V> : IMap<K, V> where K : IComparable<K>
    {
        private readonly HashMap<K, V> internalMap = new HashMap<K, V>();

        public int Count => internalMap.Count;
        public bool IsEmpty => internalMap.IsEmpty;
        public IEnumerable<K> Keys => internalMap.Keys;
        public IEnumerable<V> Values => internalMap.Values;

        public V this[K key]
        {
            get => internalMap[key];
            set => internalMap.Put(key, value);
        }

        public void Put(K key, V value) => internalMap.Put(key, value);
        public void Clear() => internalMap.Clear();
        public void Remove(K key) => internalMap.Remove(key);

        public bool ContainsKey(K key)
        {
            foreach (var existingKey in internalMap.Keys)
            {
                if (existingKey.CompareTo(key) == 0)
                    return true;
            }
            return false;
        }

        public bool ContainsValue(V value)
        {
            foreach (var existingValue in internalMap.Values)
            {
                if (value is IComparable<V> comparableValue)
                {
                    if (comparableValue.CompareTo(existingValue) == 0)
                        return true;
                }
                else if (existingValue.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<IMap<K, V>.IEntry> GetEnumerator() => internalMap.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

