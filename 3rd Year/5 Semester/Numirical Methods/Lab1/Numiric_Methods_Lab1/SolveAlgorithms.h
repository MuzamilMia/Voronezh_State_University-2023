#pragma once

#include "TridiagonalMatrix.h"
#include "MyVector.h"
#include <limits>
#include <cmath>

class SolveAlgorithms {
public:
    static constexpr double ZERO = 1e-15;
    static constexpr double MIN_DENOM = std::numeric_limits<double>::epsilon();

    static inline double getDenom(double denom) {
        return std::fabs(denom) < ZERO ? std::copysign(MIN_DENOM, denom) : denom;
    }

    static Vector solveThomas(const TridiagonalMatrix& A, const Vector& d) {
        size_t n = A.size();
        if (n == 0) return Vector(0);

        Vector L(n + 1);  // L[1] to L[n]
        Vector M(n + 1);  // M[1] to M[n]  
        Vector x(n);

        // Forward sweep
        double denom = getDenom(A.B(1));
        L[1] = A.C(1) / denom;
        M[1] = d[1] / denom;

        for (size_t i = 2; i <= n; ++i) {
            denom = getDenom(A.B(i) - A.A(i) * L[i - 1]);
            if (i < n) {
                L[i] = A.C(i) / denom;
            }
            M[i] = (d[i] - A.A(i) * M[i - 1]) / denom;
        }

        // Backward substitution
        x[n] = M[n];
        for (int i = n - 1; i >= 1; --i) {
            x[i] = M[i] - L[i] * x[i + 1];
        }

        return x;
    }

    static Vector solveNotStable(const TridiagonalMatrix& A, const Vector& d) {
        size_t n = A.size();
        if (n == 0) return Vector(0);

        Vector y(n + 1);  // y[1] to y[n]
        Vector z(n + 1);  // z[1] to z[n]
        Vector x(n);

        // Initialize
        y[1] = 0.0;
        z[1] = 1.0;

        if (n >= 1) {
            double denom = getDenom(A.C(1));
            y[2] = d[1] / denom;
            z[2] = -A.B(1) / denom;
        }

        // Recurrence relations
        for (size_t i = 2; i <= n - 1; ++i) {
            double denom = getDenom(A.C(i));
            y[i + 1] = (d[i] - A.A(i) * y[i - 1] - A.B(i) * y[i]) / denom;
            z[i + 1] = (-A.A(i) * z[i - 1] - A.B(i) * z[i]) / denom;
        }

        // Compute K
        double K = (d[n] - A.A(n) * y[n - 1] - A.B(n) * y[n]);
        double denom = getDenom(A.A(n) * z[n - 1] + A.B(n) * z[n]);
        K /= denom;

        // Compute solution
        for (size_t i = 1; i <= n; ++i) {
            x[i] = y[i] + K * z[i];
        }

        return x;
    }
};