#include "header.h"

int main() {
    int N, M, K1, K2;
    std::string filename = "numbers.txt";
    std::ofstream outputFilename("output.txt");
    std::forward_list<int> numbers;
    std::forward_list<int> modifiedNumbers;
    int choice;

    while (true) {
        displayMenu();
        std::cout << "Choose the choice: ";
        
        if (!(std::cin >> choice)) {
            std::cin.clear();
            while (std::cin.get() != '\n');
            std::cout << "Invalid input. Please enter a number.\n";
            continue;
        }

        switch (choice) {
        case 1:
        {
            std::cout << "-------------------------------------------- \n";
            std::cout << "Enetr N and M: ";
            std::cin >> N >> M;
            std::fstream file = fillFile_Random_Loop(filename, N, M);
            file.close(); // Закрыть файл после использования
        }
        
        break;

        case 2:
        {
            std::cout << "-------------------------------------------- \n";
            std::cout << "Enter N and M: ";
            std::cin >> N >> M;
            std::fstream file = fillFile_Random_Generate(filename, N, M);
            file.close(); // Закрыть файл после использования
           
        }
       
        break;

        case 3:
            std::cout << "-------------------------------------------- \n";
            numbers = read_numbers_fromFile(filename);
            if (numbers.empty())
                std::cout << "Numbers are not existed, Empty container!!! \n";
            else
            {
                outputFilename.clear();
                modifiedNumbers = modify_for(numbers);
                std::cout << "Numbers have been modified (for).\n";
                print_container(modifiedNumbers);
            }
            break;

        case 4:
            std::cout << "-------------------------------------------- \n";
            numbers = read_numbers_fromFile(filename);
            if (numbers.empty())
                std::cout << "The container is empty !!! \n";
            else
            {
                outputFilename.clear();
                modifiedNumbers = modify_transform(numbers);
                std::cout << "Числа модифицированы (std::transform).\n";
                print_container(modifiedNumbers);
                //print_container(modifiedNumbers, outputFilename);
                //print_container(modifiedNumbers, fstream); // file
                 //print_container(modifiedNumbers); // console
            }
            break;

        case 5:
            std::cout << "-------------------------------------------- \n";
            numbers = read_numbers_fromFile(filename);
            if (numbers.empty())
                std::cout << "The container is empty !!! \n"; 
            else
            {
                outputFilename.clear();
                modifiedNumbers = modify_forEach(numbers);
                std::cout << "Numbers have been modified (std::for_each).\n";
                print_container(modifiedNumbers);
            }
            break;

        case 6:
            std::cout << "-------------------------------------------- \n";
            if (modifiedNumbers.empty()) {
                std::cout << "First modify the numbers!!!.\n";
            }
            else {
                int sum = calculate_sum(modifiedNumbers);
                double average = calculate_average(modifiedNumbers);
                std::cout << "Sum: " << sum << "\n";
                std::cout << "Average: " << average << "\n";
            }
            break;

        case 7:
            if (modifiedNumbers.empty())
                std::cout << "First modify the numbers!!!.\n";
            else
            {
                print_container(modifiedNumbers, outputFilename);
                std::cout << "Numbers have been added to the file \n";
                print_container(modifiedNumbers);
            }
            break;

        case 8:
            std::cout << "Exit.\n";
            
            return 0;

        default:
            std::cout << "Wrong choice, Try again.\n";
            break;
        }
    }

    return 0;
}