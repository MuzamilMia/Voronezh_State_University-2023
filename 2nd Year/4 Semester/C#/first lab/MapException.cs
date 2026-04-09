
namespace myproject.IMap
{
    // Класс MapException представляет пользовательское исключение для работы с картами (IMap).
    // Наследуется от стандартного класса Exception.
    public class MapException : Exception
    {
        // Конструктор, который принимает сообщение об ошибке и передает его в базовый класс Exception.
        public MapException(string message) : base(message) { }
    }

    // Класс KeyNotFoundException представляет исключение, которое выбрасывается, когда ключ не найден в карте.
    // Наследуется от MapException.
    public class KeyNotFoundException : MapException
    {
        // Конструктор, который принимает ключ и формирует сообщение об ошибке.
        public KeyNotFoundException(string key) : base($"Key not found: {key}") { }
    }
}
