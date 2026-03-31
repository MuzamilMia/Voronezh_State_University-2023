#include "Experiments.h"
#include "ConsoleHelper.h"
#include "TridiagonalMatrix.h"
#include <fstream>
using namespace std;

int main() {
    TridiagonalMatrix A(1);
    Vector B(1);
    short choice{};
    do {
        choice = ConsoleHelper::displayMenu("Solve the СЛАУ from file",
            "Conduct a computational experiment", "Exit", nullptr);
        switch (choice) {
        case 1: {
            std::ifstream file;
            ConsoleHelper::getValidFileConsole(file);
            file >> A;
            file >> B;
            file.close();

            short choice1{};
            do {
                choice1 = ConsoleHelper::displayMenu("Solve by sweep(Thomas) method/ прогонки",
                    "Solve неустойчивым Method", "Exit", nullptr);
                if (choice1 == 1 || choice1 == 2) {
                    Vector result = (choice1 == 1) ?
                        SolveAlgorithms::solveThomas(A, B) :
                        SolveAlgorithms::solveNotStable(A, B);
                    std::cout << result;
                }
            } while (choice1 != 3);
            break;
        }
        case 2: {
            double range{};
            ConsoleHelper::validation(range, [](double x) { return x > 0; },
                "Input range for random values (>0):");
            Experiments::runConditionExperiment(range);
            break;
        }
        case 3:
            std::cout << "Get out from the program...";
            break;
        default:
            break;
        }
    } while (choice != 3);
    return 0;
}