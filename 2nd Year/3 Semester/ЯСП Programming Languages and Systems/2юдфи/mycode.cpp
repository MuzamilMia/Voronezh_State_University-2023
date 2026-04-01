#include"source.h"

// Menu function
void menu() {
    Angle a, b, result;
    std::string filename;
    int choice, deg, min;
    double scalar;

    do {
        std::cout << "\nMenu:\n";
        std::cout << "1. Enter the Angle from Keyboard\n";
        std::cout << "2. Enter the Angle from file\n";
        std::cout << "3. Show the Angle to the Screen\n";
        std::cout << "4. Store the Angle to file\n";
        std::cout << "5. Change the Angle to Radians\n";
        std::cout << "6. Get the Sine of the Angle\n";
        std::cout << "7. Increase the Angle\n";
        std::cout << "8. Decrease the Angle\n";
        std::cout << "9. Add Angles\n";
        std::cout << "10. Multiply an Angle by a Number\n";
        std::cout << "11. Compare Angles\n";
        std::cout << "0. Exit\n";
        std::cout << "Your Choice: ";
        std::cin >> choice;

        switch (choice) {
        case 1:
            a.input_from_keyboard();
            break;
        case 2:
            std::cout << "Enter file name: ";
            std::cin >> filename;
            try {
                a.input_from_file(filename);
            }
            catch (const std::exception& e) {
                std::cerr << "Error: " << e.what() << "\n";
            }
            break;
        case 3:
            std::cout << "Angle: ";
            a.output_to_screen();
            break;
        case 4:
            std::cout << "Enter File name: ";
            std::cin >> filename;
            try {
                a.output_to_file(filename);
            }
            catch (const std::exception& e) {
                std::cerr << "Error: " << e.what() << "\n";
            }
            break;
        case 5:
            std::cout << "Angle to Radians: " << a.to_radians() << "\n";
            break;
        case 6:
            std::cout << "Sine of Angle: " << a.sin_value() << "\n";
            break;
        case 7:
            std::cout << "Enter degrees to increase: ";
            std::cin >> deg;
            std::cout << "Enter minutes to increase: ";
            std::cin >> min;
            a.increase(deg, min);
            break;
        case 8:
            std::cout << "Enter degrees to decrease: ";
            std::cin >> deg;
            std::cout << "Enter minutes to decrease: ";
            std::cin >> min;
            a.decrease(deg, min);
            break;
        case 9:
            std::cout << "Enter the second angle:\n";
            b.input_from_keyboard();
            result = a + b;
            std::cout << "Sum of angles: ";
            result.output_to_screen();
            break;
        case 10:
            std::cout << "Enter a real number to multiply: ";
            std::cin >> scalar;
            result = a * scalar;
            std::cout << "Result of multiplication: ";
            result.output_to_screen();
            break;
        case 11:
            std::cout << "Enter the second angle for comparison:\n";
            b.input_from_keyboard();
            if (a == b)
                std::cout << "Angles are Equal.\n";
            else if (a < b)
                std::cout << "First Angle is smaller than the Second.\n";
            else
                std::cout << "First Angle is larger than the Second.\n";
            break;
        case 0:
            std::cout << "Exiting the program.\n";
            break;
        default:
            std::cout << "Incorrect Choice!\n";
            break;
        }
    } while (choice != 0);
}

// Main function
int main() {
    try {
        menu();
    }
    catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << "\n";
    }
    return 0;
}

