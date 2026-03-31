// Source.cpp
#include "ConsoleHelper.h"
#include "Experiments.h"
#include "GaussianSolver.h"
#include "DenseMatrix.h"
#include "MyVector.h"
#include <fstream>
#include <iostream>

int main() {
    std::cout << "ЛАБОРАТОРНАЯ РАБОТА: Метод Гаусса с выбором ведущего по столбцу\n";
    std::cout << "Вариант 3п.3\n\n";

    short choice{};
    do {
        choice = ConsoleHelper::displayMenu(
            "Решить СЛАУ из файла",
            "Эксперимент по размерности",
            "Эксперимент по обусловленности",
            "Выход", nullptr);

        switch (choice) {
        case 1: {
            std::cout << "\n=== РЕШЕНИЕ СИСТЕМЫ ИЗ ФАЙЛА ===\n";
            std::ifstream file;
            ConsoleHelper::getValidFileConsole(file);

            DenseMatrix A;
            file >> A;  // Читает n=3 + матрицу 3×3

            // ✅ РУЧНОЕ ЧТЕНИЕ b с размером A.size()!
            size_t n = A.size();
            Vector b(n);  // Создаём b размером n=3

            for (size_t i = 1; i <= n; i++) {
                if (!(file >> b[i])) {
                    throw std::runtime_error("Недостаточно элементов в векторе b");
                }
            }

            file.close();

            std::cout << "Матрица A:\n" << A << "\n";
            std::cout << "Правая часть b:\n" << b << "\n";

            try {
                Vector x = GaussianSolver::solveColumnPivot(A, b);
                std::cout << "\nРешение x:\n" << x << "\n";
            }
            catch (const std::exception& e) {
                std::cout << "ОШИБКА: " << e.what() << "\n";
            }
            break;
        }

        case 2: {
            std::cout << "\n=== ЭКСПЕРИМЕНТ ПО РАЗМЕРНОСТИ ===\n";
            double alpha = 2.0;  // или от пользователя
            double range{};
            ConsoleHelper::validation(range, [](double x) { return x > 0; }, "Введите диапазон (>0):");
            Experiments::runSizeExperiment(alpha, range);
            break;
        }
        case 3: {
            std::cout << "\n=== ЭКСПЕРИМЕНТ ПО ОБУСЛОВЛЕННОСТИ ===\n";
            double range{};
            ConsoleHelper::validation(range, [](double x) { return x > 0; }, "Введите диапазон (>0):");
            Experiments::runConditionExperiment(range);
            break;
        }

        case 4:
            std::cout << "До свидания!\n";
            break;
        }
        std::cout << "\n";
    } while (choice != 4);

    return 0;
}
