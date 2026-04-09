using myproject.IMap;

using YourProject.Implementations;
using YourProject1.Implementations;

// the working part of the code. 
namespace myproject1.Utils
{
    // Статический класс для утилит, работающих с картами (IMap).
    public static class MapUtils
    {
        // Делегат для создания новой карты. Возвращает экземпляр IMap<K, V>.
        public delegate IMap<K, V> MapConstructorDelegate<K, V>();

        // Делегат для проверки условия на элементе карты (IEntry).
        public delegate bool CheckDelegate<K, V>(IMap<K, V>.IEntry entry);

        // Делегат для выполнения действия над элементом карты (IEntry).
        public delegate void ActionDelegate<K, V>(IMap<K, V>.IEntry entry);

        // Метод для проверки, существует ли хотя бы один элемент в карте, удовлетворяющий условию.
        public static bool Exists<K, V>(IMap<K, V> map, CheckDelegate<K, V> predicate)
        {
            // Перебираем все элементы карты.
            foreach (var entry in map)
                // Если условие выполнено, возвращаем true.
                if (predicate(entry)) return true;
            return false;
        }

        // Метод для поиска всех элементов карты, удовлетворяющих условию, и создания новой карты с этими элементами.
        public static IMap<K, V> FindAll<K, V>(IMap<K, V> map, CheckDelegate<K, V> predicate, MapConstructorDelegate<K, V> constructor)
        {
            // Создаем новую карту с помощью переданного конструктора.
            IMap<K, V> result = constructor();
            // Перебираем все элементы карты.
            foreach (var entry in map)
                // Если элемент удовлетворяет условию, добавляем его в новую карту.
                if (predicate(entry))
                    result.Put(entry.Key, entry.Value);
            return result;
        }

        // Метод для выполнения действия над каждым элементом карты.
        public static void ForEach<K, V>(IMap<K, V> map, ActionDelegate<K, V> action)
        {
            // Перебираем все элементы карты.
            foreach (var entry in map)
                // Выполняем действие над элементом.
                action(entry);
        }

        // Метод для проверки, что все элементы карты удовлетворяют условию.
        public static bool CheckForAll<K, V>(IMap<K, V> map, CheckDelegate<K, V> predicate)
        {
            // Перебираем все элементы карты.
            foreach (var entry in map)
                // Если хотя бы один элемент не удовлетворяет условию, возвращаем false.
                if (!predicate(entry)) return false;
            return true;
        }

        // Метод для создания карты на основе массива.

        //public static IMap<K, V> CreateArrayMap<K, V>() where K : IComparable<K>
        //=> new ArrayMap<K, V>();

        //public static IMap<K, V> CreateLinkedMap<K, V>() where K : IComparable<K>
        //    => new LinkedMap<K, V>();

        //public static IMap<K, V> CreateHashMap<K, V>() where K : IComparable<K>
        //    => new HashMap<K, V>();
        public static IMap<K, V> CreateArrayMap<K, V>() where K : IComparable<K> => new ArrayMap<K, V>();
        public static IMap<K, V> CreateLinkedMap<K, V>() where K : IComparable<K> => new LinkedMap<K, V>();
        public static IMap<K, V> CreateHashMap<K, V>() where K : IComparable<K> => new HashMap<K, V>();
    }
}

