
using System;
using System.Collections.Generic;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace YourProject1.Implementations
{
    public class LinkedMap<K, V> : IMap<K, V> where K : IComparable<K>
    {
        private class Node
        {
            public K Key { get; set; }
            public V Value { get; set; }
            public Node Next { get; set; }
        }

        private Node head;
        private int count;
     
        public int Count => count;
        public bool IsEmpty => count == 0;

        public IEnumerable<K> Keys
        {
            get
            {
                Node current = head;
                while (current != null)
                {
                    yield return current.Key;
                    current = current.Next;
                }
            }
        }

        public IEnumerable<V> Values
        {
            get
            {
                Node current = head;
                while (current != null)
                {
                    yield return current.Value;
                    current = current.Next;
                }
            }
        }

        public V this[K key]
        {
            get
            {
                Node current = head;
                while (current != null)
                {
                    if (current.Key.CompareTo(key) == 0)
                        return current.Value;
                    current = current.Next;
                }
                throw new KeyNotFoundException("Key not found");
            }
            set
            {
                Node current = head;
                while (current != null)
                {
                    if (current.Key.CompareTo(key) == 0)
                    {
                        current.Value = value;
                        return;
                    }
                    current = current.Next;
                }
                throw new KeyNotFoundException("Key not found");
            }
        }

        public void Put(K key, V value)
        {
            Node newNode = new Node { Key = key, Value = value, Next = head };
            head = newNode;
            count++;
        }
        public void Clear()
        {
            head = null;
            count = 0;
        }

        public void Remove(K key)
        {
            Node current = head, previous = null;
            while (current != null)
            {
                if (current.Key.CompareTo(key) == 0)
                {
                    if (previous == null)
                        head = current.Next;
                    else
                        previous.Next = current.Next;

                    count--;
                    return;
                }
                previous = current;
                current = current.Next;
            }
            throw new KeyNotFoundException("Key not found");
        }

        public bool ContainsKey(K key)
        {
            Node current = head;
            while (current != null)
            {
                if (current.Key.CompareTo(key) == 0)
                    return true;
                current = current.Next;
            }
            return false;
        }

        public bool ContainsValue(V value)
        {
            Node current = head;
            while (current != null)
            {
                if (EqualityComparer<V>.Default.Equals(current.Value, value))
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public IEnumerator<IMap<K, V>.IEntry> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return new MapEntry<K, V>(current.Key, current.Value);  // Correctly returns entries
                current = current.Next;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
