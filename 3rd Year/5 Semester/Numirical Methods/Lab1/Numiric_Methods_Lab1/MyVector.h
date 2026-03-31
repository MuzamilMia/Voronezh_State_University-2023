#pragma once

#include <iostream>
#include <vector>
#include <stdexcept>
#include <cmath>
#include <random>
#include <string>

using namespace std;

class Vector {
    size_t n;
    vector<double> data;
public:
    Vector() : n(0) {}
    Vector(size_t n_) : n(n_), data(n_ + 1, 0.0) {}
    Vector(std::initializer_list<double> init) {
        n = init.size();
        data.resize(n + 1);
        size_t i = 1;
        for (auto val : init) {
            data[i++] = val;
        }
    }
    size_t size() const { return n; }
    double& operator[](size_t i) {
        if (i < 1 || i > n) throw std::out_of_range("Vector index out of range");
        return data.at(i);
    }
    double operator[](size_t i) const {
        if (i < 1 || i > n) throw std::out_of_range("Vector index out of range");
        return data.at(i);
    }
    Vector operator+(const Vector& other) const {
        if (n != other.n) throw runtime_error("size mismatch");
        Vector r(n);
        for (size_t i = 1; i <= n; ++i) r[i] = (*this)[i] + other[i];
        return r;
    }
    Vector operator-(const Vector& other) const {
        if (n != other.n) throw runtime_error("size mismatch");
        Vector r(n);
        for (size_t i = 1; i <= n; ++i) r[i] = (*this)[i] - other[i];
        return r;
    }
    Vector operator*(double alpha) const {
        Vector r(n);
        for (size_t i = 1; i <= n; ++i) r[i] = (*this)[i] * alpha;
        return r;
    }
    double dot(const Vector& other) const {
        if (n != other.n) throw runtime_error("size mismatch");
        double s = 0.0;
        for (size_t i = 1; i <= n; ++i) s += (*this)[i] * other[i];
        return s;
    }
    double normMax() const {
        double m = 0.0;
        for (size_t i = 1; i <= n; ++i) m = max(m, fabs((*this)[i]));
        return m;
    }
    void fillRandom(double low, double high, unsigned seed = std::random_device{}()) {
        std::mt19937_64 rng(seed);
        std::uniform_real_distribution<double> dist(low, high);
        for (size_t i = 1; i <= n; ++i) data[i] = dist(rng);
    }
    friend istream& operator>>(istream& is, Vector& v) {
        size_t nn;
        if (!(is >> nn)) return is;
        v = Vector(nn);
        for (size_t i = 1; i <= nn; ++i) is >> v[i];
        return is;
    }
    friend ostream& operator<<(ostream& os, const Vector& v) {
        os << v.n << "\n";
        for (size_t i = 1; i <= v.n; ++i) {
            os << v[i];
            if (i < v.n) os << " ";
        }
        os << "\n";
        return os;
    }
};