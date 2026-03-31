#pragma once

#include <iostream>
#include <vector>
#include <stdexcept>
#include <cmath>
#include "MyVector.h"

using namespace std;

class TridiagonalMatrix {
    size_t n;
    Vector a;
    Vector b;
    Vector c;
public:
    TridiagonalMatrix() : n(0) {}
    TridiagonalMatrix(size_t n_) : n(n_), a(n_), b(n_), c(n_) {}
    TridiagonalMatrix(const Vector& B, const Vector& C, const Vector& A) {
        if (B.size() != C.size() + 1 || B.size() != A.size() + 1)
            throw std::runtime_error("The size of vecotrs is not corrected");
        n = B.size();
        a = Vector(n);
        b = Vector(n);
        c = Vector(n);
        for (size_t i = 1; i <= n; ++i) b[i] = B[i];
        for (size_t i = 1; i <= n - 1; ++i) c[i] = C[i];
        for (size_t i = 2; i <= n; ++i) a[i] = A[i - 1];
    }
    size_t size() const { return n; }
    double& A(size_t i) {
        if (i < 2 || i > n) throw std::out_of_range("A index out of range");
        return a[i];
    }
    double A(size_t i) const {
        if (i < 2 || i > n) throw std::out_of_range("A index out of range");
        return a[i];
    }
    double& B(size_t i) {
        if (i < 1 || i > n) throw std::out_of_range("B index out of range");
        return b[i];
    }
    double B(size_t i) const {
        if (i < 1 || i > n) throw std::out_of_range("B index out of range");
        return b[i];
    }
    double& C(size_t i) {
        if (i < 1 || i > n - 1) throw std::out_of_range("C index out of range");
        return c[i];
    }
    double C(size_t i) const {
        if (i < 1 || i > n - 1) throw std::out_of_range("C index out of range");
        return c[i];
    }
    void fillRandom(double low, double high, unsigned seed = std::random_device{}()) {
        b.fillRandom(low, high, seed);
        if (n > 1) {
            c.fillRandom(low, high, seed + 1);
            a.fillRandom(low, high, seed + 2);
        }
    }
    TridiagonalMatrix operator+(const TridiagonalMatrix& other) const {
        if (n != other.n) throw runtime_error("size mismatch");
        TridiagonalMatrix r(n);
        for (size_t i = 1; i <= n; ++i) r.B(i) = B(i) + other.B(i);
        for (size_t i = 1; i <= n - 1; ++i) r.C(i) = C(i) + other.C(i);
        for (size_t i = 2; i <= n; ++i) r.A(i) = A(i) + other.A(i);
        return r;
    }
    TridiagonalMatrix operator-(const TridiagonalMatrix& other) const {
        if (n != other.n) throw runtime_error("size mismatch");
        TridiagonalMatrix r(n);
        for (size_t i = 1; i <= n; ++i) r.B(i) = B(i) - other.B(i);
        for (size_t i = 1; i <= n - 1; ++i) r.C(i) = C(i) - other.C(i);
        for (size_t i = 2; i <= n; ++i) r.A(i) = A(i) - other.A(i);
        return r;
    }
    Vector multiply(const Vector& v) const {
        if (v.size() != n) throw runtime_error("size mismatch");
        Vector res(n);
        if (n == 0) return res;

        // First row
        res[1] = B(1) * v[1] + C(1) * v[2];

        // Middle rows
        for (size_t i = 2; i <= n - 1; ++i)
            res[i] = A(i) * v[i - 1] + B(i) * v[i] + C(i) * v[i + 1];

        // Last row
        if (n >= 2)
            res[n] = A(n) * v[n - 1] + B(n) * v[n];

        return res;
    }
    friend istream& operator>>(istream& is, TridiagonalMatrix& m) {
        size_t nn;
        if (!(is >> nn)) return is;
        m = TridiagonalMatrix(nn);
        for (size_t i = 1; i <= nn; ++i) is >> m.B(i);
        for (size_t i = 1; i <= nn - 1; ++i) is >> m.C(i);
        for (size_t i = 2; i <= nn; ++i) is >> m.A(i);
        return is;
    }
    friend ostream& operator<<(ostream& os, const TridiagonalMatrix& m) {
        os << m.n << "\n";
        for (size_t i = 1; i <= m.n; ++i) {
            os << m.B(i);
            if (i < m.n) os << " ";
        }
        os << "\n";
        for (size_t i = 1; i <= max<size_t>(1, m.n - 1); ++i) {
            if (i <= m.n - 1) os << m.C(i);
            if (i < m.n - 1) os << " ";
        }
        os << "\n";
        for (size_t i = 2; i <= m.n; ++i) {
            os << m.A(i);
            if (i < m.n) os << " ";
        }
        os << "\n";
        return os;
    }
};