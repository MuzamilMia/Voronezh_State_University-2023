////#include <iostream>
////#include <fstream>
////#include <cmath>
////#include <string>
////#include <sstream>
////
////// Автор: Ваше имя
////// Задача: Класс для работы с углами
////
////class Angle {
////private:
////    int degrees; // Градусы
////    int minutes; // Минуты
////
////    // Приведение угла к диапазону 0–360 градусов
////    void normalize() {
////        int totalMinutes = degrees * 60 + minutes;
////        totalMinutes %= 360 * 60; // Приведение к 0–360 градусов в минутах
////        if (totalMinutes < 0) totalMinutes += 360 * 60; // Если отрицательно, корректируем
////        degrees = totalMinutes / 60;
////        minutes = totalMinutes % 60;
////    }
////
////public:
////    // Конструкторы
////    Angle() : degrees(0), minutes(0) {}
////    Angle(int deg, int min) : degrees(deg), minutes(min) {
////        normalize();
////    }
////
////    // Преобразование в радианы
////    double to_radians() const {
////        double totalDegrees = degrees + minutes / 60.0;
////        return totalDegrees * M_PI / 180.0; // Перевод в радианы
////    }
////
////    // Получение синуса угла
////    double sin_value() const {
////        return sin(to_radians());
////    }
////
////    // Увеличение угла на заданную величину
////    void increase(int deg, int min) {
////        degrees += deg;
////        minutes += min;
////        normalize();
////    }
////
////    // Уменьшение угла на заданную величину
////    void decrease(int deg, int min) {
////        degrees -= deg;
////        minutes -= min;
////        normalize();
////    }
////
////    // Перегрузка операторов
////    Angle operator+(const Angle& other) const {
////        return Angle(degrees + other.degrees, minutes + other.minutes);
////    }
////
////    Angle operator*(double scalar) const {
////        int totalMinutes = (degrees * 60 + minutes) * scalar;
////        return Angle(totalMinutes / 60, totalMinutes % 60);
////    }
////
////    auto operator<=>(const Angle& other) const = default;
////
////    // Ввод и вывод
////    void input_from_keyboard() {
////        std::cout << "Введите градусы: ";
////        std::cin >> degrees;
////        std::cout << "Введите минуты: ";
////        std::cin >> minutes;
////        normalize();
////    }
////
////    void input_from_file(const std::string& filename) {
////        std::ifstream file(filename);
////        if (!file.is_open()) throw std::runtime_error("Не удалось открыть файл.");
////        file >> degrees >> minutes;
////        normalize();
////    }
////
////    void output_to_screen() const {
////        std::cout << degrees << "° " << minutes << "'\n";
////    }
////
////    void output_to_file(const std::string& filename) const {
////        std::ofstream file(filename);
////        if (!file.is_open()) throw std::runtime_error("Не удалось открыть файл.");
////        file << degrees << " " << minutes;
////    }
////
////    // Преобразование в строку
////    std::string to_string() const {
////        std::ostringstream oss;
////        oss << degrees << "° " << minutes << "'";
////        return oss.str();
////    }
////};
//#include <iostream>
//#include <cmath>
//#include <string>
//#include <sstream>
//
//// Define M_PI if missing
//#ifndef M_PI
//#define M_PI 3.14159265358979323846
//#endif
//
//class Angle {
//private:
//    int degrees;
//    int minutes;
//
//    void normalize() {
//        int totalMinutes = degrees * 60 + minutes;
//        totalMinutes %= 360 * 60;
//        if (totalMinutes < 0) totalMinutes += 360 * 60;
//        degrees = totalMinutes / 60;
//        minutes = totalMinutes % 60;
//    }
//
//public:
//    Angle() : degrees(0), minutes(0) {}
//    Angle(int deg, int min) : degrees(deg), minutes(min) {
//        normalize();
//    }
//
//    double to_radians() const {
//        double totalDegrees = degrees + minutes / 60.0;
//        return totalDegrees * M_PI / 180.0;
//    }
//
//    double sin_value() const {
//        return sin(to_radians());
//    }
//
//    void increase(int deg, int min) {
//        degrees += deg;
//        minutes += min;
//        normalize();
//    }
//
//    void decrease(int deg, int min) {
//        degrees -= deg;
//        minutes -= min;
//        normalize();
//    }
//
//    Angle operator+(const Angle& other) const {
//        return Angle(degrees + other.degrees, minutes + other.minutes);
//    }
//
//    Angle operator*(double scalar) const {
//        int totalMinutes = (degrees * 60 + minutes) * scalar;
//        return Angle(totalMinutes / 60, totalMinutes % 60);
//    }
//
//    bool operator==(const Angle& other) const {
//        return degrees == other.degrees && minutes == other.minutes;
//    }
//
//    bool operator<(const Angle& other) const {
//        return (degrees * 60 + minutes) < (other.degrees * 60 + other.minutes);
//    }
//
//    bool operator>(const Angle& other) const {
//        return (degrees * 60 + minutes) > (other.degrees * 60 + other.minutes);
//    }
//
//    bool operator<=(const Angle& other) const {
//        return !(*this > other);
//    }
//
//    bool operator>=(const Angle& other) const {
//        return !(*this < other);
//    }
//
//    void input_from_keyboard() {
//        std::cout << "Введите градусы: ";
//        std::cin >> degrees;
//        std::cout << "Введите минуты: ";
//        std::cin >> minutes;
//        normalize();
//    }
//
//    void output_to_screen() const {
//        std::cout << degrees << "° " << minutes << "'\n";
//    }
//};
//
//
//void menu()
//{
//
//    Angle a, b, result;
//    std::string filename;
//    int choice, deg, min;
//    double scalar;
//
//    do {
//        std::cout << "\nМеню:\n";
//        std::cout << "1. Ввод угла с клавиатуры\n";
//        std::cout << "2. Ввод угла из файла\n";
//        std::cout << "3. Вывод угла на экран\n";
//        std::cout << "4. Вывод угла в файл\n";
//        std::cout << "5. Перевод угла в радианы\n";
//        std::cout << "6. Получение синуса угла\n";
//        std::cout << "7. Увеличение угла\n";
//        std::cout << "8. Уменьшение угла\n";
//        std::cout << "9. Сложение углов\n";
//        std::cout << "10. Умножение угла на число\n";
//        std::cout << "11. Сравнение углов\n";
//        std::cout << "0. Выход\n";
//        std::cout << "Ваш выбор: ";
//        std::cin >> choice;
//
//        switch (choice) {
//        case 1:
//            a.input_from_keyboard();
//            break;
//        case 2:
//            std::cout << "Введите имя файла: ";
//            std::cin >> filename;
//            a.input_from_file(filename);
//            break;
//        case 3:
//            std::cout << "Угол: ";
//            a.output_to_screen();
//            break;
//        case 4:
//            std::cout << "Введите имя файла: ";
//            std::cin >> filename;
//            a.output_to_file(filename);
//            break;
//        case 5:
//            std::cout << "Угол в радианах: " << a.to_radians() << "\n";
//            break;
//        case 6:
//            std::cout << "Синус угла: " << a.sin_value() << "\n";
//            break;
//        case 7:
//            std::cout << "Введите градусы для увеличения: ";
//            std::cin >> deg;
//            std::cout << "Введите минуты для увеличения: ";
//            std::cin >> min;
//            a.increase(deg, min);
//            break;
//        case 8:
//            std::cout << "Введите градусы для уменьшения: ";
//            std::cin >> deg;
//            std::cout << "Введите минуты для уменьшения: ";
//            std::cin >> min;
//            a.decrease(deg, min);
//            break;
//        case 9:
//            std::cout << "Ввод второго угла:\n";
//            b.input_from_keyboard();
//            result = a + b;
//            std::cout << "Сумма углов: ";
//            result.output_to_screen();
//            break;
//        case 10:
//            std::cout << "Введите вещественное число для умножения: ";
//            std::cin >> scalar;
//            result = a * scalar;
//            std::cout << "Результат умножения: ";
//            result.output_to_screen();
//            break;
//        case 11:
//            std::cout << "Ввод второго угла для сравнения:\n";
//            b.input_from_keyboard();
//            if (a == b)
//                std::cout << "Углы равны.\n";
//            else if (a < b)
//                std::cout << "Первый угол меньше второго.\n";
//            else
//                std::cout << "Первый угол больше второго.\n";
//            break;
//        case 0:
//            std::cout << "Выход из программы.\n";
//            break;
//        default:
//            std::cout << "Некорректный выбор!\n";
//            break;
//        }
//    } while (choice != 0);
//}
//
//int main() {
//    try {
//        menu();
//    }
//    catch (const std::exception& e) {
//        std::cerr << "Ошибка: " << e.what() << "\n";
//    }
//    return 0;
//}


//Автор: Mia Muzamil 
//Задача: 14. Создать класс Angle для работы с углами на плоскости, задаваемыми величиной в градусах и минутах.
// Обязательно должны быть реализованы : перевод в радианы,приведение к диапазону 0 - 360, увеличение и уменьшение
// угла на заданную величину, получение синуса, сравнение углов, сложение углов, умножение угла на вещественное число.

/*#include <iostream>
#include <cmath>
#include <string>
#include <fstream>
#include <sstream>

// Define M_PI if missing
#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

class Angle {
private:
    int degrees;
    int minutes;

    void normalize() {
        int totalMinutes = degrees * 60 + minutes;
        totalMinutes %= 360 * 60;
        if (totalMinutes < 0) totalMinutes += 360 * 60;
        degrees = totalMinutes / 60;
        minutes = totalMinutes % 60;
    }

public:
    Angle() : degrees(0), minutes(0) {}
    Angle(int deg, int min) : degrees(deg), minutes(min) {
        normalize();
    }

    double to_radians() const {
        double totalDegrees = degrees + minutes / 60.0;
        return totalDegrees * M_PI / 180.0;
    }

    double sin_value() const {
        return sin(to_radians());
    }

    void increase(int deg, int min) {
        degrees += deg;
        minutes += min;
        normalize();
    }

    void decrease(int deg, int min) {
        degrees -= deg;
        minutes -= min;
        normalize();
    }

    Angle operator+(const Angle& other) const {
        return Angle(degrees + other.degrees, minutes + other.minutes);
    }

    Angle operator*(double scalar) const {
        int totalMinutes = (degrees * 60 + minutes) * scalar;
        return Angle(totalMinutes / 60, totalMinutes % 60);
    }

    bool operator==(const Angle& other) const {
        return degrees == other.degrees && minutes == other.minutes;
    }

    bool operator<(const Angle& other) const {
        return (degrees * 60 + minutes) < (other.degrees * 60 + other.minutes);
    }

    bool operator>(const Angle& other) const {
        return (degrees * 60 + minutes) > (other.degrees * 60 + other.minutes);
    }

    bool operator<=(const Angle& other) const {
        return !(*this > other);
    }

    bool operator>=(const Angle& other) const {
        return !(*this < other);
    }

    void input_from_keyboard() {
        std::cout << "Введите градусы: ";
        std::cin >> degrees;
        std::cout << "Введите минуты: ";
        std::cin >> minutes;
        normalize();
    }

    void input_from_file(const std::string& filename) {
        std::ifstream file(filename+".txt");
        if (!file.is_open()) throw std::runtime_error("Не удалось открыть файл.");
        file >> degrees >> minutes;
        normalize();
    }

    void output_to_screen() const {
        std::cout << degrees << "° " << minutes << "'\n";
    }

    void output_to_file(const std::string& filename) const {
        std::ofstream file(filename+".txt");
        if (!file.is_open()) throw std::runtime_error("Не удалось открыть файл.");
        file << degrees << " " << minutes;
    }

    std::string to_string() const {
        std::ostringstream oss;
        oss << degrees << "° " << minutes << "'";
        return oss.str();
    }
};

// Menu function
void menu() {
    Angle a, b, result;
    std::string filename;
    int choice, deg, min;
    double scalar;

    do {
        std::cout << "\nМеню:\n";
        std::cout << "1. Ввод угла с клавиатуры\n";
        std::cout << "2. Ввод угла из файла\n";
        std::cout << "3. Вывод угла на экран\n";
        std::cout << "4. Вывод угла в файл\n";
        std::cout << "5. Перевод угла в радианы\n";
        std::cout << "6. Получение синуса угла\n";
        std::cout << "7. Увеличение угла\n";
        std::cout << "8. Уменьшение угла\n";
        std::cout << "9. Сложение углов\n";
        std::cout << "10. Умножение угла на число\n";
        std::cout << "11. Сравнение углов\n";
        std::cout << "0. Выход\n";
        std::cout << "Ваш выбор: ";
        std::cin >> choice;

        switch (choice) {
        case 1:
            a.input_from_keyboard();
            break;
        case 2:
            std::cout << "Введите имя файла: ";
            std::cin >> filename;
            try {
                a.input_from_file(filename);
            }
            catch (const std::exception& e) {
                std::cerr << "Ошибка: " << e.what() << "\n";
            }
            break;
        case 3:
            std::cout << "Угол: ";
            a.output_to_screen();
            break;
        case 4:
            std::cout << "Введите имя файла: ";
            std::cin >> filename;
            try {
                a.output_to_file(filename);
            }
            catch (const std::exception& e) {
                std::cerr << "Ошибка: " << e.what() << "\n";
            }
            break;
        case 5:
            std::cout << "Угол в радианах: " << a.to_radians() << "\n";
            break;
        case 6:
            std::cout << "Синус угла: " << a.sin_value() << "\n";
            break;
        case 7:
            std::cout << "Введите градусы для увеличения: ";
            std::cin >> deg;
            std::cout << "Введите минуты для увеличения: ";
            std::cin >> min;
            a.increase(deg, min);
            break;
        case 8:
            std::cout << "Введите градусы для уменьшения: ";
            std::cin >> deg;
            std::cout << "Введите минуты для уменьшения: ";
            std::cin >> min;
            a.decrease(deg, min);
            break;
        case 9:
            std::cout << "Ввод второго угла:\n";
            b.input_from_keyboard();
            result = a + b;
            std::cout << "Сумма углов: ";
            result.output_to_screen();
            break;
        case 10:
            std::cout << "Введите вещественное число для умножения: ";
            std::cin >> scalar;
            result = a * scalar;
            std::cout << "Результат умножения: ";
            result.output_to_screen();
            break;
        case 11:
            std::cout << "Ввод второго угла для сравнения:\n";
            b.input_from_keyboard();
            if (a == b)
                std::cout << "Углы равны.\n";
            else if (a < b)
                std::cout << "Первый угол меньше второго.\n";
            else
                std::cout << "Первый угол больше второго.\n";
            break;
        case 0:
            std::cout << "Выход из программы.\n";
            break;
        default:
            std::cout << "Некорректный выбор!\n";
            break;
        }
    } while (choice != 0);
}

// Main function
int main() {
    try {
        menu();
    }
    catch (const std::exception& e) {
        std::cerr << "Ошибка: " << e.what() << "\n";
    }
    return 0;
}*/

//-----------------------

//#include<iostream>
//#include <cmath>
//#include <string>
//#include <fstream>
//#include <sstream>
//#include <numbers>
//
//// Define M_PI if missing
//#ifndef M_PI
//#define M_PI std::numbers::pi //3.14159265358979323846
//#endif
//
//class Angle {
//private:
//    int degrees;
//    int minutes;
//
//    void normalize() {
//        int totalMinutes = degrees * 60 + minutes;
//        totalMinutes %= 360 * 60;
//        if (totalMinutes < 0) totalMinutes += 360 * 60;
//        degrees = totalMinutes / 60;
//        minutes = totalMinutes % 60;
//    }
//
//public:
//    Angle() : degrees(0), minutes(0) {}
//    Angle(int deg, int min) : degrees(deg), minutes(min) {
//        normalize();
//    }
//
//    double to_radians() const {
//        double totalDegrees = degrees + minutes / 60.0;
//        return totalDegrees * M_PI / 180.0;
//    }
//
//    double sin_value() const {
//        return sin(to_radians());
//    }
//
//    void increase(int deg, int min) {
//        degrees += deg;
//        minutes += min;
//        normalize();
//    }
//
//    void decrease(int deg, int min) {
//        degrees -= deg;
//        minutes -= min;
//        normalize();
//    }
//
//    Angle operator+(const Angle& other) const {
//        return Angle(degrees + other.degrees, minutes + other.minutes);
//    }
//
//    Angle operator*(double scalar) const {
//        int totalMinutes = (degrees * 60 + minutes) * scalar;
//        return Angle(totalMinutes / 60, totalMinutes % 60);
//    }
//
//    bool operator==(const Angle& other) const {
//        return degrees == other.degrees && minutes == other.minutes;
//    }
//
//    // operator <=>
//    bool operator<(const Angle& other) const {
//        return (degrees * 60 + minutes) < (other.degrees * 60 + other.minutes);
//    }
//
//    bool operator>(const Angle& other) const {
//        return (degrees * 60 + minutes) > (other.degrees * 60 + other.minutes);
//    }
//
//    bool operator<=(const Angle& other) const {
//        return !(*this > other);
//    }
//
//    bool operator>=(const Angle& other) const {
//        return !(*this < other);
//    }
//    //
//
//    void input_from_keyboard() {
//        std::cout << "Enter Degrees: ";
//        std::cin >> degrees;
//        std::cout << "Enter Minutes: ";
//        std::cin >> minutes;
//        normalize();
//    }
//
//    void input_from_file(const std::string& filename) {
//        std::ifstream file(filename + ".txt");
//        if (!file.is_open()) throw std::runtime_error("File is not open.");
//        file >> degrees >> minutes;
//        normalize();
//    }
//
//    void output_to_screen() const {
//        std::cout << degrees << "° " << minutes << "'\n";
//    }
//
//    void output_to_file(const std::string& filename) const {
//        std::ofstream file(filename + ".txt");
//        if (!file.is_open()) throw std::runtime_error("File is not open.");
//        file << degrees << " " << minutes;
//    }
//
//    std::string to_string() const {
//        std::ostringstream oss;
//        oss << degrees << "° " << minutes << "'";
//        return oss.str();
//    }
//};
//
//// -----------------------------------------
//
//// Menu function
//void menu() {
//    Angle a, b, result;
//    std::string filename;
//    int choice, deg, min;
//    double scalar;
//
//    do {
//        std::cout << "\nMenu:\n";
//        std::cout << "1. Enter the Angle from Keyboard\n";
//        std::cout << "2. Enter the Angle from file\n";
//        std::cout << "3. Show the Angle to the Screen\n";
//        std::cout << "4. Store the Angle to file\n";
//        std::cout << "5. Change the Angle to Radian's\n";
//        std::cout << "6. Getting the sine of an angle\n";
//        std::cout << "7. Increase angle\n";
//        std::cout << "8. Decreasing the angle\n";
//        std::cout << "9. Addition of angles\n";
//        std::cout << "10. Multiplying an angle by a number\n";
//        std::cout << "11. Comparison of angles\n";
//        std::cout << "0. Exit\n";
//        std::cout << "Your Choice: ";
//        std::cin >> choice;
//
//        switch (choice) {
//        case 1:
//            a.input_from_keyboard();
//            break;
//        case 2:
//            std::cout << "Enter file name: ";
//            std::cin >> filename;
//            try {
//                a.input_from_file(filename);
//            }
//            catch (const std::exception& e) {
//                std::cerr << "Error: " << e.what() << "\n";
//            }
//            break;
//        case 3:
//            std::cout << "Angle: ";
//            a.output_to_screen();
//            break;
//        case 4:
//            std::cout << "Enter File name: ";
//            std::cin >> filename;
//            try {
//                a.output_to_file(filename);
//            }
//            catch (const std::exception& e) {
//                std::cerr << "Error: " << e.what() << "\n";
//            }
//            break;
//        case 5:
//            std::cout << "Angle to Radians: " << a.to_radians() << "\n";
//            break;
//        case 6:
//            std::cout << "Sin of Angle: " << a.sin_value() << "\n";
//            break;
//        case 7:
//            std::cout << "Enter degrees to increase (увеличения): ";
//            std::cin >> deg;
//            std::cout << "Enter minutes to increase (увеличения): ";
//            std::cin >> min;
//            a.increase(deg, min);
//            break;
//        case 8:
//            std::cout << "Enter degrees to decrease (уменьшения): ";
//            std::cin >> deg;
//            std::cout << "Enter minutes to decrease (уменьшения): ";
//            std::cin >> min;
//            a.decrease(deg, min);
//            break;
//        case 9:
//            std::cout << "Enter the second angle:\n";
//            b.input_from_keyboard();
//            result = a + b;
//            std::cout << "Sum of angles: ";
//            result.output_to_screen();
//            break;
//        case 10:
//            std::cout << "Enter a real number to multiply (умножения): ";
//            std::cin >> scalar;
//            result = a * scalar;
//            std::cout << "Result of multiplication (умножения): ";
//            result.output_to_screen();
//            break;
//        case 11:
//            std::cout << "Enter the second angle for comparison:\n";
//            b.input_from_keyboard();
//            if (a == b)
//                std::cout << "Angles are Equal.\n";
//            else if (a < b)
//                std::cout << "First Angle is smaller then Second.\n";
//            else
//                std::cout << "First ANgle is bigger then second.\n";
//            break;
//        case 0:
//            std::cout << "Exit from the program.\n";
//            break;
//        default:
//            std::cout << "Incorrect Choice!\n";
//            break;
//        }
//    } while (choice != 0);
//}
//
//// Main function
//int main() {
//    try {
//        menu();
//    }
//    catch (const std::exception& e) {
//        std::cerr << "Error: " << e.what() << "\n";
//    }
//    return 0;
//}
