#include"Class.h"

void menu() {
    Set<int> set1, set2;
    while (true) {
        std::cout << "\nМеню:\n";
        std::cout << "1. Добавить элемент в множество 1\n";
        std::cout << "2. Добавить элемент в множество 2\n";
        std::cout << "3. Удалить элемент из множества 1\n";
        std::cout << "4. Удалить элемент из множества 2\n";
        std::cout << "5. Печать множества 1\n";
        std::cout << "6. Печать множества 2\n";
        std::cout << "7. Объединение множеств\n";
        std::cout << "8. Пересечение множеств\n";
        std::cout << "9. Разность множеств\n";
        std::cout << "10. Инициализация множества 1 из файла\n";
        std::cout << "11. Инициализация множества 2 из файла\n";
        std::cout << "12. Вывод множества в файл\n";
        std::cout << "13. Выход\n";
        int choice;
        std::cin >> choice;

        if (choice == 13) break;

        switch (choice) {
        case 1: {
            int elem;
            std::cout << "Введите элемент для добавления в множество 1: ";
            std::cin >> elem;
            set1.add(elem);
            break;
        }
        case 2: {
            int elem;
            std::cout << "Введите элемент для добавления в множество 2: ";
            std::cin >> elem;
            set2.add(elem);
            break;
        }
        case 3: {
            int elem;
            std::cout << "Введите элемент для удаления из множества 1: ";
            std::cin >> elem;
            set1.remove(elem);
            break;
        }
        case 4: {
            int elem;
            std::cout << "Введите элемент для удаления из множества 2: ";
            std::cin >> elem;
            set2.remove(elem);
            break;
        }
        case 5:
            std::cout << "Множество 1: ";
            set1.print();
            break;
        case 6:
            std::cout << "Множество 2: ";
            set2.print();
            break;
        case 7: {
            Set<int> result = set1.unite(set2);
            std::cout << "Объединение множеств: ";
            result.print();
            break;
        }
        case 8: {
            Set<int> result = set1.intersect(set2);
            std::cout << "Пересечение множеств: ";
            result.print();
            break;
        }
        case 9: {
            Set<int> result = set1.difference(set2);
            std::cout << "Разность множеств: ";
            result.print();
            break;
        }
        case 10: {
            std::string filename;
            std::cout << "Введите имя файла для инициализации множества 1: ";
            std::cin >> filename;
            set1.initFromFile(filename);
            break;
        }
        case 11: {
            std::string filename;
            std::cout << "Введите имя файла для инициализации множества 2: ";
            std::cin >> filename;
            set2.initFromFile(filename);
            break;
        }
        case 12: {
            std::string filename;
            std::cout << "Введите имя файла для вывода множества: ";
            std::cin >> filename;
            set1.printToFile(filename);
            break;
        }
        default:
            std::cout << "Неверный выбор.\n";
        }
    }
}

int main()
{
	menu();
	return 0;
}