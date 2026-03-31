#pragma once
#include "GaussianSolver.h"  
#include <iomanip>
#include <random>
#include <vector>
#include <iostream>
#include <limits>
#include <cmath>

class Experiments {
public:
    static ExperimentResult runSingle(const DenseMatrix& A, const Vector& x_true) {
        Vector b = A.multiply(x_true);
        Vector x = GaussianSolver::solveColumnPivot(A, b);

        double eps = std::sqrt(std::numeric_limits<double>::epsilon());
        double absErr = 0.0, relErr = 0.0;
        size_t n = x_true.size();

        for (size_t i = 1; i <= n; ++i) {
            double diff = std::fabs(x[i] - x_true[i]);
            absErr = std::max(absErr, diff);
            double denom = std::fabs(x_true[i]) < eps ? 1.0 : std::fabs(x_true[i]);
            relErr = std::max(relErr, diff / denom);
        }
        return { absErr, relErr };
    }

    static void runSizeExperiment(double alpha = 1.0, double range = 10.0, int trials = 3) {
        std::vector<size_t> sizes = { 8, 16, 32, 64, 128, 256, 512 };

        std::random_device rd;
        std::mt19937_64 rng(rd());

        std::cout << std::setw(8) << "n"
            << std::setw(15) << "абс.A"
            << std::setw(15) << "отн.A"
            << std::setw(15) << "\t абс.B"
            << std::setw(15) << "\t отн.B" << "\n";
        std::cout << std::string(70, '-') << "\n";

        for (size_t n : sizes) {
            double totalAbsA = 0, totalRelA = 0, totalAbsB = 0, totalRelB = 0;

            for (int t = 0; t < trials; ++t) {
                DenseMatrix A(n);
                A.fillRandom(-range, range, static_cast<unsigned>(rng()));
                for (size_t i = 1; i <= n; ++i)
                    A(i, i) *= alpha;

                Vector x_true(n);
                x_true.fillRandom(-1.0, 1.0, static_cast<unsigned>(rng()));

                // A: Стабильный (с pivoting)
                ExperimentResult rA = runSingle(A, x_true);
                totalAbsA += rA.absError;
                totalRelA += rA.relError;

                // B: Нестабильный (слабая диагональ)
                DenseMatrix A_bad(n);
                A_bad = A;
                for (size_t i = 1; i <= n; ++i) 
                    A_bad(i, i) *= 0.01;
                ExperimentResult rB = runSingle(A_bad, x_true);
                totalAbsB += rB.absError;
                totalRelB += rB.relError;
            }

            std::cout << std::setw(8) << n
                << std::setw(15) << std::scientific << (totalAbsA / trials)
                << std::setw(15) << std::scientific << (totalRelA / trials)
                << std::setw(15) << std::scientific << (totalAbsB / trials)
                << std::setw(15) << std::scientific << (totalRelB / trials) << "\n";
        }
    }

    /*static void runConditionExperiment(double range = 10.0, int trials = 3) {
        std::vector<double> alphas = { range * 8.0, range * 4.0, range * 2.0, range,
                                     range / 2.0, range / 4.0, range / 8.0 };
        std::vector<size_t> sizes = { 8, 16, 32, 64, 128, 256, 512 };

        std::cout << "\nCondition experiments:\n";
        std::cout << "|" << std::setw(13) << "alpha" << " |"
            << std::setw(13) << "range" << " |"
            << std::setw(7) << "n" << " |"
            << std::setw(14) << "абс.A" << "    |"
            << std::setw(14) << "отн.A" << "    |"
            << std::setw(14) << "абс.B" << "    |"
            << std::setw(14) << "отн.B" << "    |\n";
        std::cout << "|" << std::string(13, '-') << "-|-"
            << std::string(12, '-') << "-|-"
            << std::string(6, '-') << "-|-"
            << std::string(13, '-') << "-|-"
            << std::string(13, '-') << "-|-"
            << std::string(13, '-') << "-|-"
            << std::string(13, '-') << "-|\n";

        std::random_device rd;
        std::mt19937_64 rng(rd());

        for (double alpha : alphas) {
            bool firstRowForAlpha = true;
            for (size_t n : sizes) {
                double totalAbsA = 0, totalRelA = 0, totalAbsB = 0, totalRelB = 0;

                for (int t = 0; t < trials; ++t) {
                    DenseMatrix A(n);
                    A.fillRandom(-range, range, static_cast<unsigned>(rng()));
                    for (size_t i = 1; i <= n; ++i) 
                        A(i, i) *= alpha;

                    Vector x_true(n);
                    x_true.fillRandom(-1.0, 1.0, static_cast<unsigned>(rng()));

                    ExperimentResult rA = runSingle(A, x_true);
                    totalAbsA += rA.absError;
                    totalRelA += rA.relError;

                    DenseMatrix A_bad(n);
                    A_bad = A;
                    for (size_t i = 1; i <= n; ++i) A_bad(i, i) *= 0.01;
                    ExperimentResult rB = runSingle(A_bad, x_true);
                    totalAbsB += rB.absError;
                    totalRelB += rB.relError;
                }

                std::cout << "|";
                if (firstRowForAlpha) {
                    std::cout << std::setw(13) << alpha << " |"
                        << std::setw(13) << range << " |";
                    firstRowForAlpha = false;
                }
                else {
                    std::cout << std::setw(13) << "" << " |"
                        << std::setw(13) << "" << " |";
                }

                std::cout << std::setw(7) << n << " |"
                    << std::setw(14) << std::scientific << (totalAbsA / trials) << " |"
                    << std::setw(14) << std::scientific << (totalRelA / trials) << " |"
                    << std::setw(14) << std::scientific << (totalAbsB / trials) << " |"
                    << std::setw(14) << std::scientific << (totalRelB / trials) << " |\n";
            }
            if (alpha != alphas.back()) {
                std::cout << "|" << std::string(13, '-') << "-|-"
                    << std::string(12, '-') << "-|-"
                    << std::string(6, '-') << "-|-"
                    << std::string(13, '-') << "-|-"
                    << std::string(13, '-') << "-|-"
                    << std::string(13, '-') << "-|-"
                    << std::string(13, '-') << "-|\n";
            }
        }
        std::cout << "|" << std::string(13, '=') << "=|="
            << std::string(13, '=') << "=|="
            << std::string(6, '=') << "=|="
            << std::string(13, '=') << "=|="
            << std::string(13, '=') << "=|="
            << std::string(13, '=') << "=|="
            << std::string(13, '=') << "=|\n\n";
    }*/

    static void runConditionExperiment(double range = 10.0) {
        std::vector<double> alphas = { range * 8.0, range * 4.0, range * 2.0, range,
                                     range / 2.0, range / 4.0, range / 8.0 };
        std::vector<size_t> sizes = { 10,20,40,80,160,320,640 };  // БОЛЬШИЕ n для драмы!

        std::cout << "\nЭксперимент по обусловленности (range=" << range << "):\n";
        std::cout << "|" << std::setw(13) << "alpha" << " |"
            << std::setw(13) << "range" << " |"
            << std::setw(7) << "n" << " |"
            << std::setw(23) << "абс.погр." << " |"
            << std::setw(23) << "отн.погр." << " |\n";
        std::cout << "|" << std::string(13, '-') << "-|-"
            << std::string(12, '-') << "-|-"
            << std::string(6, '-') << "-|-"
            << std::string(15, '-') << "-|-"
            << std::string(15, '-') << "-|\n";

        std::random_device rd;
        std::mt19937_64 rng(rd());

        for (double alpha : alphas) {
            bool firstRow = true;
            for (size_t n : sizes) {
                double totalAbs = 0.0, totalRel = 0.0;

                // 5 прогонов для стабильности
                for (int t = 0; t < 5; ++t) {
                    DenseMatrix A(n);
                    A.fillRandom(-range, range, static_cast<unsigned>(rng()));
                    for (size_t i = 1; i <= n; ++i) {
                        A(i, i) *= alpha /10;  // ×10 для БОЛЬШЕЙ разницы!
                    }

                    Vector x_true(n);
                    x_true.fillRandom(-1.0, 1.0, static_cast<unsigned>(rng()));

                    ExperimentResult res = runSingle(A, x_true);
                    totalAbs += res.absError;
                    totalRel += res.relError;
                }

                std::cout << "|";
                if (firstRow) {
                    std::cout << std::setw(13) << alpha << " |"
                        << std::setw(13) << range << " |";
                    firstRow = false;
                }
                else {
                    std::cout << std::setw(13) << "" << " |"
                        << std::setw(13) << "" << " |";
                }

                std::cout << std::setw(7) << n << " |"
                    << std::setw(16) << std::scientific << (totalAbs / 5.0) << " |"
                    << std::setw(16) << std::scientific << (totalRel / 5.0) << " |\n";
            }
            if (alpha != alphas.back()) {
                std::cout << "|" << std::string(13, '-') << "-|-"
                    << std::string(12, '-') << "-|-"
                    << std::string(6, '-') << "-|-"
                    << std::string(15, '-') << "-|-"
                    << std::string(15, '-') << "-|\n";
            }
        }
        std::cout << "|" << std::string(13, '=') << "=|="
            << std::string(12, '=') << "=|="
            << std::string(6, '=') << "=|="
            << std::string(15, '=') << "=|="
            << std::string(15, '=') << "=|\n\n";
    }

};
