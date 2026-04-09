//using myproject.IMap;

//namespace myproject.Implementations
//{
//    public class MapEntry<K, V> : IMap<K, V>.IEntry where K : IComparable<K>
//    {
//        public K Key { get; }
//        public V Value { get; set; }

//        public MapEntry(K key, V value)
//        {
//            Key = key;
//            Value = value;
//        }
//    }
//}


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
