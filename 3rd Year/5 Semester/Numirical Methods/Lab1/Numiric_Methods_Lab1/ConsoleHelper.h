#pragma once

#include <iostream>
#include <iomanip>
#include <fstream>
#include <cstdarg>
#include <string>

class ConsoleHelper {
public:
    static short displayMenu(const char* firstOption, ...) {
        std::cout << "\nInput number from Menu:\n";
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
            std::cout << "Input number from 1 to " << count << '\n';
            std::cout << "-> ";
        }
        std::cin.clear();
        std::cin.ignore(255, '\n');
        return output;
    }

    template<class T, class Predicat>
    static void validation(T& x, Predicat condition, const char* message)
    {
        std::cout << message << "\n-> ";
        while (true)
        {
            try {
                std::cin >> x;
            }
            catch (std::exception& e) {
                std::cout << e.what() << '\n';
                std::cin.clear();
                std::cin.ignore(255, '\n');
                continue;
            }
            if (condition(x))
                break;
            else {
                std::cout << "Input error!" << '\n';
                std::cin.clear();
                std::cin.ignore(255, '\n');
                std::cout << message << "\n-> ";
            }
        }
    }

    static bool checkFile(std::ifstream& file) {
        bool result{ false };
        if (!file)
            std::cout << "File is not found!\n";
        else if (file.peek() == EOF)
            std::cout << "File is empty!\n";
        else
            result = true;
        return result;
    }

    static void getValidFileConsole(std::ifstream& file) {
        bool condition{};
        do {
            std::cout << "Input filename (with .txt extension): ";

            std::string filename;
            std::getline(std::cin, filename);

            std::cout << "Trying to open: " << filename << '\n';

            file.open(filename);
            condition = checkFile(file);
            if (!condition) {
                file.close();
                std::cout << "Please try again.\n";
            }
        } while (!condition);
        std::cout << "File opened successfully!\n";
    }
};