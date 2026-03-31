// GaussianSolver.h
#pragma once
#include "DenseMatrix.h"
#include "MyVector.h"
#include <cmath>
#include <limits>

struct ExperimentResult {
    double absError;     // Максимальная абсолютная погрешность
    double relError;     // Максимальная относительная погрешность
};

class GaussianSolver {
public:
    static constexpr double ZERO = 1e-15;  // Порог для определения нулевого pivot

    static Vector solveColumnPivot(DenseMatrix A, Vector b) {
        size_t n = A.size();

        // ПРЯМАЯ ЧАСТЬ: приведение к верхнетреугольному виду
        for (size_t k = 1; k <= n - 1; ++k) {  //  ПРЯМОЙ ХОД
            // 1. ВЫБОР ПИВОТА ПО СТОЛБЦУ k (partial pivoting)
            size_t pivotRow = k;
            double maxVal = std::fabs(A(k, k));

            for (size_t i = k + 1; i <= n; ++i) {
                double val = std::fabs(A(i, k));
                if (val > maxVal) {
                    maxVal = val;
                    pivotRow = i;
                }
            }

            // 2. ПЕРЕСТАНОВКА СТРОК (Permutation of Rows)
            if (pivotRow != k) {
                for (size_t j = 1; j <= n; ++j) {
                    std::swap(A(k, j), A(pivotRow, j));
                }
                std::swap(b[k], b[pivotRow]);
            }
             
            // 3. ПРОВЕРКА НА ВЫРОЖДЕННОСТЬ (Test for degenracy)
            if (std::fabs(A(k, k)) < ZERO) {
                throw std::runtime_error("Матрица вырожденная или сильно обусловленная");
            }

            // 4. ИСКЛЮЧЕНИЕ НИЖЕ ПИВОТА (Below pivot exception)
            for (size_t i = k + 1; i <= n; ++i) {
                double factor = A(i, k) / A(k, k);  // Множитель исключения
                for (size_t j = k; j <= n; ++j) {
                    A(i, j) -= factor * A(k, j);
                }
                b[i] -= factor * b[k];
            }
        }

        // ОБРАТНАЯ ПОДСТАНОВКА or ОБРАТНЫЙ ХОД
        Vector x(n);
        for (int i = n; i >= 1; --i) {
            double sum = b[i];
            for (size_t j = i + 1; j <= n; ++j) {
                sum -= A(i, j) * x[j];
            }
            x[i] = sum / A(i, i);
        }

        return x;
    }

    /*
    Вход: матрица A, точное решение x_true
    Выход: абсолютная и относительная погрешности
    */
    static ExperimentResult runSingle(const DenseMatrix& A, const Vector& x_true) {
        Vector b = A.multiply(x_true);           // b = A * x_true
        Vector x = solveColumnPivot(A, b);       // x = solve(A, b)

        double eps = std::sqrt(std::numeric_limits<double>::epsilon());
        double absErr = 0.0, relErr = 0.0;
        size_t n = x_true.size();

        // ВЫЧИСЛЕНИЕ ПОГРЕШНОСТЕЙ
        for (size_t i = 1; i <= n; ++i) {
            double diff = std::fabs(x[i] - x_true[i]);
            absErr = std::max(absErr, diff);
                    
            // Относительная погрешность: если |x_true[i]| < √ε, то = absErr
            double denom = std::fabs(x_true[i]) < eps ? 1.0 : std::fabs(x_true[i]);
            relErr = std::max(relErr, diff / denom);
        }
        return { absErr, relErr };
    }
};
