// MyVector.h
#pragma once
#include <iostream>
#include <vector>
#include <stdexcept>
#include <cmath>
#include <random>
#include <string>

class Vector {
    size_t n;                   
    std::vector<double> data;   

public:
    Vector() : n(0) {}
    Vector(size_t n_) : n(n_), data(n_ + 1, 0.0) {}

    Vector(std::initializer_list<double> init) {
        n = init.size();
        data.resize(n + 1);
        size_t i = 1;
        for (auto val : init) data[i++] = val;
    }

    size_t size() const { return n; }

    // Оператор доступа с проверкой границ (1-based)
    double& operator[](size_t i) {
        if (i < 1 || i > n) throw std::out_of_range("Индекс вектора вне диапазона");
        return data.at(i);
    }
    double operator[](size_t i) const {
        if (i < 1 || i > n) throw std::out_of_range("Индекс вектора вне диапазона");
        return data.at(i);
    }

    // Арифметические операции
    Vector operator+(const Vector& other) const {
        if (n != other.n) throw std::runtime_error("Несовпадение размерностей");
        Vector r(n);
        for (size_t i = 1; i <= n; ++i) r[i] = (*this)[i] + other[i];
        return r;
    }
    Vector operator-(const Vector& other) const {
        if (n != other.n) throw std::runtime_error("Несовпадение размерностей");
        Vector r(n);
        for (size_t i = 1; i <= n; ++i) r[i] = (*this)[i] - other[i];
        return r;
    }
    Vector operator*(double alpha) const {
        Vector r(n);
        for (size_t i = 1; i <= n; ++i) r[i] = (*this)[i] * alpha;
        return r;
    }

    // Скалярное произведение
    double dot(const Vector& other) const {
        if (n != other.n) throw std::runtime_error("Несовпадение размерностей");
        double s = 0.0;
        for (size_t i = 1; i <= n; ++i) s += (*this)[i] * other[i];
        return s;
    }

    // Норма как максимум модулей компонент 
    double normMax() const {
        double m = 0.0;
        for (size_t i = 1; i <= n; ++i) m = std::max(m, std::fabs((*this)[i]));
        return m;
    }

    // Заполнение случайными числами
    void fillRandom(double low, double high, unsigned seed = std::random_device{}()) {
        std::mt19937_64 rng(seed);
        std::uniform_real_distribution<double> dist(low, high);
        for (size_t i = 1; i <= n; ++i) data[i] = dist(rng);
    }

    // Ввод/вывод
    friend std::istream& operator>>(std::istream& is, Vector& v) {
        size_t nn;
        if (!(is >> nn)) return is;
        v = Vector(nn);
        for (size_t i = 1; i <= nn; ++i) {
            if (!(is >> v[i])) 
                throw std::runtime_error("Недостаточно элементов в векторе b");
        }
        return is;
    }
    friend std::ostream& operator<<(std::ostream& os, const Vector& v) {
        os << v.n << "\n";
        for (size_t i = 1; i <= v.n; ++i) {
            os << v[i];
            if (i < v.n) os << " ";
        }
        os << "\n";
        return os;
    }
};
