// ConsoleHelper.h
#pragma once
#include <iostream>
#include <iomanip>
#include <fstream>
#include <cstdarg>
#include <string>

class ConsoleHelper {
public:
    // Отображает меню и возвращает выбор пользователя
    static short displayMenu(const char* firstOption, ...) {
        std::cout << "\nВыберите номер из меню:\n";
        short count = 0;
        va_list args;
        va_start(args, firstOption);
        const char* option = firstOption;
        while (option != nullptr) {
            std::cout << ++count << ". " << option << "\n";
            option = va_arg(args, const char*);
        }
        va_end(args);

        short output{};
        std::cout << "-> ";
        while (!(std::cin >> output && 0 < output && output <= count)) {
            std::cin.clear();
            std::cin.ignore(255, '\n');
            std::cout << "Введите число от 1 до " << count << '\n';
            std::cout << "-> ";
        }
        std::cin.clear();
        std::cin.ignore(255, '\n');
        return output;
    }

    // Валидация ввода с пользовательским условием
    template<class T, class Predicat>
    static void validation(T& x, Predicat condition, const char* message) {
        std::cout << message << "\n-> ";
        while (true) {
            try {
                std::cin >> x;
            }
            catch (std::exception& e) {
                std::cout << e.what() << '\n';
                std::cin.clear();
                std::cin.ignore(255, '\n');
                continue;
            }
            if (condition(x)) break;
            else {
                std::cout << "Ошибка ввода!" << '\n';
                std::cin.clear();
                std::cin.ignore(255, '\n');
                std::cout << message << "\n-> ";
            }
        }
    }

    // Проверка корректности файла
    static bool checkFile(std::ifstream& file) {
        bool result{ false };
        if (!file) std::cout << "Файл не найден!\n";
        else if (file.peek() == EOF) std::cout << "Файл пуст!\n";
        else result = true;
        return result;
    }

    // Получение корректного файла от пользователя
    static void getValidFileConsole(std::ifstream& file) {
        bool condition{};
        do {
            std::cout << "Введите имя файла (с расширением .txt): ";
            std::string filename;
            std::getline(std::cin, filename);
            std::cout << "Попытка открыть: " << filename << '\n';
            file.open(filename);
            condition = checkFile(file);
            if (!condition) {
                file.close();
                std::cout << "Попробуйте снова.\n";
            }
        } while (!condition);
        std::cout << "Файл успешно открыт!\n";
    }
};
