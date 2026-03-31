#pragma once

#include <iostream>
#include <iomanip>
#include <cmath>
#include <vector>
#include <limits>
#include <random>
#include "TridiagonalMatrix.h"
#include "MyVector.h"
#include "SolveAlgorithms.h"

struct ExperimentResult {
    double absError;
    double relError;
};

class Experiments {
public:
    static ExperimentResult runSingle(const TridiagonalMatrix& A, const Vector& x_true,
        Vector(*solve)(const TridiagonalMatrix&, const Vector&) = SolveAlgorithms::solveThomas)
    {
        Vector b = A.multiply(x_true);
        Vector x = solve(A, b);

        double eps = std::sqrt(std::numeric_limits<double>::epsilon());
        double abs = 0.0, rel = 0.0;
        size_t n = x_true.size();

        for (size_t i = 1; i <= n; ++i) {
            double diff = std::fabs(x[i] - x_true[i]);
            abs = std::max(abs, diff);
            double denom = std::fabs(x_true[i]) < eps ? 1.0 : std::fabs(x_true[i]);
            rel = std::max(rel, diff / denom);
        }
        return { abs, rel };
    }

    static void runSizeExperiment(double alpha = 1.0, double range = 100.0, Vector(*solveA)(const TridiagonalMatrix&, const Vector&) = SolveAlgorithms::solveThomas,
        Vector(*solveB)(const TridiagonalMatrix&, const Vector&) = SolveAlgorithms::solveNotStable) {
        std::vector<size_t> sizes = { 10,20,40,80,160,320,640,1280,2560,5120,10240,20480,40960,81920,163840,327680,655360 };
        std::random_device rd;
        std::mt19937_64 rng(rd());
        std::uniform_real_distribution<double> dist(-range, range);

        std::cout << std::setw(8) << "n"
            << std::setw(15) << "absA"
            << std::setw(15) << "relA"
            << std::setw(15) << "absB"
            << std::setw(15) << "relB" << "\n";

        for (auto n : sizes) {
            TridiagonalMatrix A(n);
            A.fillRandom(-range, range, rng());
            for (size_t i = 1; i <= n; ++i) A.B(i) *= alpha;

            Vector x_true(n);
            for (size_t i = 1; i <= n; ++i) x_true[i] = dist(rng);

            ExperimentResult rA = runSingle(A, x_true, solveA);
            ExperimentResult rB = runSingle(A, x_true, solveB);
            std::cout << std::setw(8) << n
                << std::setw(15) << rA.absError
                << std::setw(15) << rA.relError
                << std::setw(15) << rB.absError
                << std::setw(15) << rB.relError << "\n";
        }
    }

    //     static void runConditionExperiment(double range = 2.0, Vector (*solveA)(const TridiagonalMatrix&, const Vector&)=SolveAlgorithms::solveThomas,
    //                                        Vector (*solveB)(const TridiagonalMatrix&, const Vector&)=SolveAlgorithms::solveNotStable) {
    //       std::vector<double> alphas = {range * 8.0, range * 4.0, range * 2.0, range, range / 2.0, range / 4.0, range / 8.0};
    //       for (double alpha : alphas) {
    //         std::cout << "\nCondition experiment for alpha = " << alpha << " and range = [" << -range << ", " << range << "]:\n";
    //         runSizeExperiment(alpha, range, solveA, solveB);
    //       }
    //   }
    static void runConditionExperiment(double range = 2.0,
        Vector(*solveA)(const TridiagonalMatrix&, const Vector&) = SolveAlgorithms::solveThomas,
        Vector(*solveB)(const TridiagonalMatrix&, const Vector&) = SolveAlgorithms::solveNotStable) {
        std::vector<double> alphas = { range * 8.0, range * 4.0, range * 2.0, range, range / 2.0, range / 4.0, range / 8.0 };
        std::vector<size_t> sizes = { 10,20,40,80,160,320,640,1280,2560,5120,10240,20480,40960,81920,163840,327680,655360 };

        std::cout << "\nCondition experiments:\n";
        std::cout << "|" << std::setw(9) << "alpha" << " |"
            << std::setw(9) << "range" << " |"
            << std::setw(7) << "n" << " |"
            << std::setw(14) << "absA" << " |"
            << std::setw(14) << "relA" << " |"
            << std::setw(14) << "absB" << " |"
            << std::setw(14) << "relB" << " |\n";
        std::cout << "|" << std::string(9, '-') << "-|-"
            << std::string(8, '-') << "-|-"
            << std::string(6, '-') << "-|-"
            << std::string(13, '-') << "-|-"
            << std::string(13, '-') << "-|-"
            << std::string(13, '-') << "-|-"
            << std::string(13, '-') << "-|\n";

        std::random_device rd;
        std::mt19937_64 rng(rd());

        for (double alpha : alphas) {
            std::uniform_real_distribution<double> dist(-range, range);

            bool firstRowForAlpha = true;
            for (auto n : sizes) {
                TridiagonalMatrix A(n);
                A.fillRandom(-range, range, rng());
                for (size_t i = 1; i <= n; ++i) A.B(i) *= alpha;

                Vector x_true(n);
                for (size_t i = 1; i <= n; ++i) x_true[i] = dist(rng);

                ExperimentResult rA = runSingle(A, x_true, solveA);
                ExperimentResult rB = runSingle(A, x_true, solveB);

                std::cout << "|";

                if (firstRowForAlpha) {
                    std::cout << std::setw(9) << alpha << " |"
                        << std::setw(9) << range << " |";
                    firstRowForAlpha = false;
                }
                else {
                    std::cout << std::setw(9) << "" << " |"
                        << std::setw(9) << "" << " |";
                }

                std::cout << std::setw(7) << n << " |"
                    << std::setw(14) << rA.absError << " |"
                    << std::setw(14) << rA.relError << " |"
                    << std::setw(14) << rB.absError << " |"
                    << std::setw(14) << rB.relError << " |\n";
            }
            if (alpha != alphas.back()) {
                std::cout << "|" << std::string(9, '-') << "-|-"
                    << std::string(8, '-') << "-|-"
                    << std::string(6, '-') << "-|-"
                    << std::string(13, '-') << "-|-"
                    << std::string(13, '-') << "-|-"
                    << std::string(13, '-') << "-|-"
                    << std::string(13, '-') << "-|\n";
            }
        }
        std::cout << "|" << std::string(9, '=') << "=|="
            << std::string(8, '=') << "=|="
            << std::string(6, '=') << "=|="
            << std::string(13, '=') << "=|="
            << std::string(13, '=') << "=|="
            << std::string(13, '=') << "=|="
            << std::string(13, '=') << "=|\n\n";
    }
};
