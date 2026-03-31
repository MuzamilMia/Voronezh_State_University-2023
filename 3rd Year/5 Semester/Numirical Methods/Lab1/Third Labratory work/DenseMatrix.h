// DenseMatrix.h
#pragma once
#include "MyVector.h"
#include <vector>
#include <random>
#include <stdexcept>

class DenseMatrix {
    size_t n;                                    // Размер матрицы n×n
    std::vector<std::vector<double>> data;     

public:
    DenseMatrix() : n(0) {}
    DenseMatrix(size_t n_) : n(n_), data(n_ + 1, std::vector<double>(n_ + 1, 0.0)) {}

    size_t size() const { return n; }

    // Оператор доступа (1-based индексация)
    double& operator()(size_t i, size_t j) {
        if (i < 1 || i > n || j < 1 || j > n)
            throw std::out_of_range("Индекс матрицы вне диапазона");
        return data[i][j];
    }
    double operator()(size_t i, size_t j) const {
        if (i < 1 || i > n || j < 1 || j > n)
            throw std::out_of_range("Индекс матрицы вне диапазона");
        return data[i][j];
    }

    // Арифметические операции
    DenseMatrix operator+(const DenseMatrix& other) const {
        if (n != other.n) throw std::runtime_error("Несовпадение размерностей");
        DenseMatrix r(n);
        for (size_t i = 1; i <= n; ++i)
            for (size_t j = 1; j <= n; ++j)
                r(i, j) = (*this)(i, j) + other(i, j);
        return r;
    }
    DenseMatrix operator-(const DenseMatrix& other) const {
        if (n != other.n) throw std::runtime_error("Несовпадение размерностей");
        DenseMatrix r(n);
        for (size_t i = 1; i <= n; ++i)
            for (size_t j = 1; j <= n; ++j)
                r(i, j) = (*this)(i, j) - other(i, j);
        return r;
    }

    // Умножение матрицы на вектор Ax
    Vector multiply(const Vector& v) const {
        if (v.size() != n) throw std::runtime_error("Несовпадение размерностей");
        Vector res(n);
        for (size_t i = 1; i <= n; ++i) {
            res[i] = 0.0;
            for (size_t j = 1; j <= n; ++j)
                res[i] += (*this)(i, j) * v[j];
        }
        return res;
    }

    // Заполнение случайными числами
    void fillRandom(double low, double high, unsigned seed = std::random_device{}()) {
        std::mt19937_64 rng(seed);
        std::uniform_real_distribution<double> dist(low, high);
        for (size_t i = 1; i <= n; ++i)
            for (size_t j = 1; j <= n; ++j)
                data[i][j] = dist(rng);
    }

    // Ввод/вывод матрицы
    friend std::istream& operator>>(std::istream& is, DenseMatrix& m) {
        size_t nn;
        if (!(is >> nn)) return is;
        m = DenseMatrix(nn);
        for (size_t i = 1; i <= nn; ++i)
            for (size_t j = 1; j <= nn; ++j)
                is >> m(i, j);
        return is;
    }
    friend std::ostream& operator<<(std::ostream& os, const DenseMatrix& m) {
        os << m.n << "\n";
        for (size_t i = 1; i <= m.n; ++i) {
            for (size_t j = 1; j <= m.n; ++j) {
                os << m(i, j);
                if (j < m.n) os << " ";
            }
            os << "\n";
        }
        return os;
    }
};
