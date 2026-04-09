using System;
using System.Collections;
using System.Collections.Generic;
using KeyNotFoundException = myproject.IMap.KeyNotFoundException;

namespace YourProject1.Implementations
{
    public class HashMap<K, V> : IMap<K, V> where K : IComparable<K>
    {
        private const int DefaultCapacity = 16;
        private List<Entry>[] buckets;
        private int count;

        private class Entry
        {
            public K Key { get; set; }
            public V Value { get; set; }
        }
        public HashMap() : this(DefaultCapacity) { }

        public HashMap(int capacity = DefaultCapacity)
        {
            buckets = new List<Entry>[capacity];
            for (int i = 0; i < capacity; i++)
            {
                buckets[i] = new List<Entry>();
            }
        }

        public int Count => count;
        public bool IsEmpty => count == 0;

        public IEnumerable<K> Keys
        {
            get
            {
                foreach (var bucket in buckets)
                {
                    foreach (var entry in bucket)
                    {
                        yield return entry.Key;
                    }
                }
            }
        }

        public IEnumerable<V> Values
        {
            get
            {
                foreach (var bucket in buckets)
                {
                    foreach (var entry in bucket)
                    {
                        yield return entry.Value;
                    }
                }
            }
        }

        private int GetHashIndex(K key) => Math.Abs(key.GetHashCode()) % buckets.Length;

        public V this[K key]
        {
            get
            {
                int index = GetHashIndex(key);
                foreach (var entry in buckets[index])
                {
                    if (entry.Key.CompareTo(key) == 0)
                        return entry.Value;
                }
                throw new KeyNotFoundException("Key not found");
            }
            set => Put(key, value);
        }

        public void Put(K key, V value)
        {
            int index = GetHashIndex(key);
            foreach (var entry in buckets[index])
            {
                if (entry.Key.CompareTo(key) == 0)
                {
                    entry.Value = value;
                    return;
                }
            }
            buckets[index].Add(new Entry { Key = key, Value = value });
            count++;
        }

        public void Clear()
        {
            foreach (var bucket in buckets)
            {
                bucket.Clear();
            }
            count = 0;
        }

        public void Remove(K key)
        {
            int index = GetHashIndex(key);
            for (int i = 0; i < buckets[index].Count; i++)
            {
                if (buckets[index][i].Key.CompareTo(key) == 0)
                {
                    buckets[index].RemoveAt(i);
                    count--;
                    return;
                }
            }
            throw new KeyNotFoundException("Key not found");
        }

        public bool ContainsKey(K key)
        {
            int index = GetHashIndex(key);
            foreach (var entry in buckets[index])
            {
                if (entry.Key.CompareTo(key) == 0)
                    return true;
            }
            return false;
        }

        public bool ContainsValue(V value)
        {
            foreach (var bucket in buckets)
            {
                foreach (var entry in bucket)
                {
                    if (EqualityComparer<V>.Default.Equals(entry.Value, value))
                        return true;
                }
            }
            return false;
        }

        public IEnumerator<IMap<K, V>.IEntry> GetEnumerator()
        {
            foreach (var bucket in buckets)
            {
                foreach (var entry in bucket)
                {
                    yield return new MapEntry<K, V>(entry.Key, entry.Value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
