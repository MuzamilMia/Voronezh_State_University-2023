//using myproject.IMap;
//using System.Collections;

//namespace YourProject.Implementations
//{
//    public class ArrayMap<K, V> : IMap<K, V> 
//    {
//        private const int InitialCapacity = 4;
//        private IMap<K, V>.IEntry[] _entries;
//        private int _count;

//        /// Внутренний класс для хранения пары ключ-значение

//        private class Entry : IMap<K, V>.IEntry 
//        {
//            public K Key { get; }
//            public V Value { get; set; }

//            public Entry(K key, V value)
//            {
//                Key = key;
//                Value = value;
//            }
//        }
//        /// Конструктор по умолчанию
//        public ArrayMap()
//        {
//            _entries = new IMap<K, V>.IEntry[InitialCapacity];
//            _count = 0;
//        }
//        /// Добавляет или обновляет элемент
//        public void Put(K key, V value)
//        {
//            // Check for existing key
//            for (int i = 0; i < _count; i++)
//            {
//                if (_entries[i].Key.Equals(key))
//                {
//                    // Если ключ найден -обновляем значение
//                    _entries[i].Value = value;
//                    return;
//                }
//            }
//            // 2. Если массив заполнен - увеличиваем его размер
//            // Resize if needed
//            if (_count == _entries.Length)
//            {
//                // Создаем новый массив в 2 раза больше
//                var newEntries = new IMap<K, V>.IEntry[_entries.Length * 2];

//                // Копируем старые элементы
//                for (int i = 0; i < _count; i++)
//                {
//                    newEntries[i] = _entries[i];
//                }
//                _entries = newEntries;
//            }
//            // 3. Добавляем новый элемент
//            // Add new entry
//            _entries[_count++] = new Entry(key, value);
//        }

//        /// Получает или устанавливает значение по ключу
//        public V this[K key]
//        {
//            get
//            {
//                // Линейный поиск ключа
//                for (int i = 0; i < _count; i++)
//                {
//                    if (_entries[i].Key.Equals(key))
//                    {
//                        return _entries[i].Value;
//                    }
//                }
//                 throw new NotImplementedException();
//            }
//            set => Put(key, value);
//        }
//        /// Проверяет наличие ключа
//        public bool ContainsKey(K key)
//        {
//            for (int i = 0; i < _count; i++)
//            {
//                if (_entries[i].Key.Equals(key))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        /// Удаляет элемент по ключу
//        public void Remove(K key)
//        {
//            for (int i = 0; i < _count; i++)
//            {
//                if (_entries[i].Key.Equals(key))
//                {
//                    // Сдвигаем все элементы после удаляемого
//                    // Shift all subsequent entries left
//                    for (int j = i; j < _count - 1; j++)
//                    {
//                        _entries[j] = _entries[j + 1];
//                    }
//                    _count--;
//                    _entries[_count] = null; // Clear last reference, // Очищаем последнюю ссылку
//                    return;
//                }
//            }
//        }

//        public int Count => _count;

//        // Проверяет, пуста ли коллекция
//        public bool IsEmpty => _count == 0;

//        // Other required IMap methods...
//        /// Очищает словарь
//        public void Clear()
//        {
//            //_entries = new IMap<K, V>.IEntry[InitialCapacity];
//            _count = 0;
//        }
//        /// Проверяет наличие значения в словаре
//        public bool ContainsValue(V value)
//        {
//            for (int i = 0; i < _count; i++)
//            {
//                if (_entries[i].Value.Equals(value))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//        /// Возвращает перечисление всех ключей
//        public IEnumerable<K> Keys
//        {
//            get
//            {
//                for (int i = 0; i < _count; i++)
//                {
//                    yield return _entries[i].Key;
//                }
//            }
//        }
//        /// Возвращает перечисление всех значений
//        public IEnumerable<V> Values
//        {
//            get
//            {
//                for (int i = 0; i < _count; i++)
//                {
//                    yield return _entries[i].Value;
//                }
//            }
//        }

//        // Implement IEnumerable
//        /// Реализация перечисления элементов
//        public IEnumerator<IMap<K, V>.IEntry> GetEnumerator()
//        {
//            for (int i = 0; i < _count; i++)
//            {
//                yield return _entries[i];
//            }
//        }
//        // Явная реализация интерфейса для IEnumerable
//        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//    }

//}


//using myproject.IMap;
//using System;
//using System.Collections.Generic;
//namespace YourProject1.Implementations
//{
//    public class ArrayMap<K, V> : IMap<K, V> where K : IComparable<K>
//    {
//        private const int DefaultCapacity = 16;
//        private Entry[] entries;
//        private int size;


//        private class Entry
//        {
//            public K Key { get; set; }
//            public V Value { get; set; }
//        }

//        public ArrayMap(int capacity = DefaultCapacity)
//        {
//            entries = new Entry[capacity];
//            size = 0;
//        }

//        public int Count => size;
//        public bool IsEmpty => size == 0;
//        public IEnumerable<K> Keys
//        {
//            get
//            {
//                for (int i = 0; i < size; i++)
//                {
//                    yield return entries[i].Key;
//                }
//            }
//        }
//        public IEnumerable<V> Values
//        {
//            get
//            {
//                for (int i = 0; i < size; i++)
//                {
//                    yield return entries[i].Value;
//                }
//            }
//        }

//        public V this[K key]
//        {
//            get
//            {
//                for (int i = 0; i < size; i++)
//                {
//                    if (entries[i].Key.CompareTo(key) == 0)
//                        return entries[i].Value;
//                }
//                throw new myproject.IMap.KeyNotFoundException("Key not found");
//            }
//            set => Put(key, value);
//        }

//        public void Put(K key, V value)
//        {
//            for (int i = 0; i < size; i++)
//            {
//                if (entries[i].Key.CompareTo(key) == 0)
//                {
//                    entries[i].Value = value;
//                    return;
//                }
//            }

//            if (size == entries.Length)
//            {
//                Array.Resize(ref entries, entries.Length * 2);
//            }

//            entries[size++] = new Entry { Key = key, Value = value };
//        }

//        public void Clear()
//        {
//            size = 0;
//        }

//        public void Remove(K key)
//        {
//            for (int i = 0; i < size; i++)
//            {
//                if (entries[i].Key.CompareTo(key) == 0)
//                {
//                    for (int j = i; j < size - 1; j++)
//                    {
//                        entries[j] = entries[j + 1];
//                    }
//                    size--;
//                    return;
//                }
//            }
//            throw new myproject.IMap.KeyNotFoundException("Key not found");
//        }

//        public bool ContainsKey(K key)
//        {
//            for (int i = 0; i < size; i++)
//            {
//                if (entries[i].Key.CompareTo(key) == 0)
//                    return true;
//            }
//            return false;
//        }

//        public bool ContainsValue(V value)
//        {
//            for (int i = 0; i < size; i++)
//            {
//                if (value is IComparable<V> comparableValue)
//                {
//                    if (comparableValue.CompareTo(entries[i].Value) == 0)
//                        return true;
//                }
//                else if (entries[i].Value.Equals(value))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        public IEnumerator<IMap<K, V>.IEntry> GetEnumerator() => throw new NotImplementedException();
//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
//    }
//}


/*the Last working one

using myproject.IMap;
using System;
using System.Collections.Generic;

namespace YourProject1.Implementations
{
    public class ArrayMap<K, V> : IMap<K, V> where K : IComparable<K>
    {
        private const int DefaultCapacity = 16;
        private Entry[] entries;
        private int size;

        private class Entry
        {
            public K Key { get; set; }
            public V Value { get; set; }
        }

        public ArrayMap(int capacity = DefaultCapacity)
        {
            entries = new Entry[capacity];
            size = 0;
        }

        public int Count => size;
        public bool IsEmpty => size == 0;

        public IEnumerable<K> Keys
        {
            get
            {
                for (int i = 0; i < size; i++)
                {
                    yield return entries[i].Key;
                }
            }
        }

        public IEnumerable<V> Values
        {
            get
            {
                for (int i = 0; i < size; i++)
                {
                    yield return entries[i].Value;
                }
            }
        }

        public V this[K key]
        {
            get
            {
                for (int i = 0; i < size; i++)
                {
                    if (entries[i].Key.CompareTo(key) == 0)
                        return entries[i].Value;
                }
                throw new System.Collections.Generic.KeyNotFoundException("Key not found");
            }
            set => Put(key, value);
        }

        public void Put(K key, V value)
        {
            for (int i = 0; i < size; i++)
            {
                if (entries[i].Key.CompareTo(key) == 0)
                {
                    entries[i].Value = value;
                    return;
                }
            }

            if (size == entries.Length)
            {
                Array.Resize(ref entries, entries.Length * 2);
            }

            entries[size++] = new Entry { Key = key, Value = value };
        }

        public void Clear()
        {
            size = 0;
        }

        public void Remove(K key)
        {
            for (int i = 0; i < size; i++)
            {
                if (entries[i].Key.CompareTo(key) == 0)
                {
                    for (int j = i; j < size - 1; j++)
                    {
                        entries[j] = entries[j + 1];
                    }
                    size--;
                    return;
                }
            }
            throw new System.Collections.Generic.KeyNotFoundException("Key not found");
        }

        public bool ContainsKey(K key)
        {
            for (int i = 0; i < size; i++)
            {
                if (entries[i].Key.CompareTo(key) == 0)
                    return true;
            }
            return false;
        }

        public bool ContainsValue(V value)
        {
            for (int i = 0; i < size; i++)
            {
                if (EqualityComparer<V>.Default.Equals(entries[i].Value, value))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<IMap<K, V>.IEntry> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return new MapEntry<K, V>(entries[i].Key, entries[i].Value);  // Explicitly using <K, V>
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    // Generic MapEntry<K, V> class
    public class MapEntry<K, V> : IMap<K, V>.IEntry
    {
        public K Key { get; }
        public V Value { get; set; }

        public MapEntry(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}
*/

namespace YourProject1.Implementations
{
    public class ArrayMap<K, V> : IMap<K, V> where K : IComparable<K>
    {
        private const int DefaultCapacity = 16;
        private Entry[] entries;
        private int size;

        private class Entry
        {
            public K Key { get; set; }
            public V Value { get; set; }
        }

        // Parameterless constructor 
        public ArrayMap()
        {
            entries = new Entry[DefaultCapacity];
            size = 0;
        }

        // Original constructor with capacity
        public ArrayMap(int capacity = DefaultCapacity)
        {
            entries = new Entry[capacity];
            size = 0;
        }

        public int Count => size;
        public bool IsEmpty => size == 0;

        public IEnumerable<K> Keys
        {
            get
            {
                for (int i = 0; i < size; i++)
                {
                    yield return entries[i].Key;
                }
            }
        }

        public IEnumerable<V> Values
        {
            get
            {
                for (int i = 0; i < size; i++)
                {
                    yield return entries[i].Value;
                }
            }
        }

        public V this[K key]
        {
            get
            {
                for (int i = 0; i < size; i++)
                {
                    if (entries[i].Key.CompareTo(key) == 0)
                        return entries[i].Value;
                }
                throw new System.Collections.Generic.KeyNotFoundException("Key not found");
            }
            set => Put(key, value);
        }

        public void Put(K key, V value)
        {
            for (int i = 0; i < size; i++)
            {
                if (entries[i].Key.CompareTo(key) == 0)
                {
                    entries[i].Value = value;
                    return;
                }
            }

            if (size == entries.Length)
            {
                Array.Resize(ref entries, entries.Length * 2);
            }

            entries[size++] = new Entry { Key = key, Value = value };
        }

        public void Clear()
        {
            size = 0;
        }

        public void Remove(K key)
        {
            for (int i = 0; i < size; i++)
            {
                if (entries[i].Key.CompareTo(key) == 0)
                {
                    for (int j = i; j < size - 1; j++)
                    {
                        entries[j] = entries[j + 1];
                    }
                    size--;
                    return;
                }
            }
            throw new System.Collections.Generic.KeyNotFoundException("Key not found");
        }

        public bool ContainsKey(K key)
        {
            for (int i = 0; i < size; i++)
            {
                if (entries[i].Key.CompareTo(key) == 0)
                    return true;
            }
            return false;
        }

        public bool ContainsValue(V value)
        {
            for (int i = 0; i < size; i++)
            {
                if (EqualityComparer<V>.Default.Equals(entries[i].Value, value))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<IMap<K, V>.IEntry> GetEnumerator()
        {
            for (int i = 0; i < size; i++)
            {
                yield return new MapEntry<K, V>(entries[i].Key, entries[i].Value);  // Explicitly using <K, V>
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    // Generic MapEntry<K, V> class
    public class MapEntry<K, V> : IMap<K, V>.IEntry where K : IComparable<K>
    {
        public K Key { get; }
        public V Value { get; set; }

        public MapEntry(K key, V value)
        {
            Key = key;
            Value = value;
        }
    }
}
