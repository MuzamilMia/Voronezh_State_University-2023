#include "header.h"


//std::fstream fillFile_Random_Loop(const std::string& filename, int N, int M)
//{
//    std::fstream outFile(filename, std::ios::out); // Открыть файл для записи
//    std::random_device rd;
//    std::mt19937 gen(rd());
//    std::uniform_int_distribution<> dis(-M, M);
//    for (int i = 0; i < N; ++i) {
//        outFile << dis(gen) << " ";
//    }
//    
//    return outFile;
//}

std::fstream fillFile_Random_Loop(const std::string& filename, int N, int M) 
{
    std::fstream outFile(filename, std::ios::out);
    std::random_device rd;
    std::mt19937 gen(rd());
    std::uniform_int_distribution<> dis(-M, M);

    std::forward_list<int> tempList;
    std::forward_list<int>::iterator it = tempList.before_begin(); // auto!!!

    for (int i = 0; i < N; ++i) {
        int num = dis(gen);
        outFile << num << " ";
        it = tempList.insert_after(it, num);
    }
    if (tempList.empty())
        std::cout << "The container is empty, numbers are not generated \n";
    else
    {
        std::cout << "Generated numbers: ";
        print_container(tempList);
    }

    return outFile;
}

std::fstream fillFile_Random_Generate(const std::string& filename, int N, int M) {
    std::fstream outFile(filename, std::ios::out);
    std::random_device rd;
    std::mt19937 gen(rd());
    std::uniform_int_distribution<> dis(-M, M);

    std::forward_list<int> tempList;
    std::forward_list<int>::iterator it = tempList.before_begin();

    std::generate_n(std::ostream_iterator<int>(outFile, " "), N, [&]() {
        int num = dis(gen);
        it = tempList.insert_after(it, num);
        return num;
    });
    if (tempList.empty())
        std::cout << "The conatainer is empty, please add the numbers \n ";
    else
    {
        std::cout << "Generated numbers:  ";
        print_container(tempList); 
        
    }

    return outFile;
}

std::forward_list<int> read_numbers_fromFile(const std::string& filename)
{
    std::ifstream inFile(filename);
    std::forward_list<int> numbers;
    std::forward_list<int>::iterator it = numbers.before_begin(); // Итератор перед началом списка
    int number;

    while (inFile >> number) {
        it = numbers.insert_after(it, number);
    }
    inFile.close();
    return numbers;
}

std::forward_list<int> modify_for(const std::forward_list<int>& container)
{
    std::forward_list<int> result;
    std::forward_list<int>::iterator it_result = result.before_begin();

    int minElement = *std::min_element(container.begin(), container.end());

    for (std::forward_list<int>::const_iterator it = container.begin(); it != container.end(); ++it) {
        int newValue = static_cast<int>(std::round(std::sqrt(std::abs((*it) * minElement))));
        it_result = result.insert_after(it_result, newValue);
    }
    return result;
}

std::forward_list<int> modify_transform(const std::forward_list<int>& container)
{
    if (!container.empty()) //
    {
        const int minElement = *std::min_element(container.begin(), container.end());
        std::forward_list<int> result;

        std::transform(
            container.begin(), container.end(),
            std::front_inserter(result),  
            [minElement](int x) {
                return static_cast<int>(std::round(std::sqrt(std::abs(x * minElement))));
            }
        );
        result.reverse();

        return result;
    }
}



std::forward_list<int> modify_forEach(const std::forward_list<int>& container)
{
    std::forward_list<int> result = container;

    int minElement = *std::min_element(container.begin(), container.end());

    std::for_each(result.begin(), result.end(), [minElement](int& x) {
        x = static_cast<int>(std::round(std::sqrt(std::abs(x * minElement)))); });

    return result;
}

int calculate_sum(const std::forward_list<int>& container)
{
    return std::accumulate(container.begin(), container.end(), 0);
}

double calculate_average(const std::forward_list<int>& container)
{
    int sum = calculate_sum(container);
    int count = std::distance(container.begin(), container.end());
    return static_cast<double>(sum) / count;
}

//std::ostream operator << (std::ostream& out, std::forward_list<int> list)
//{
//    for (std::forward_list<int>::const_iterator it = list.begin(); it != list.end(); ++it) {
//        std::cout << *it << " ";
//        out << *it << " ";
//    }
//}
//void printContainer(const std::forward_list<int>& container, const std::string& filename) { // ostream out
//    //std::ofstream outFile(filename);
//    //outFile << container;
//}

//void printContainer(const std::forward_list<int>& container,
//    const std::string& filename = "",  // Default here
//    std::ostream& os = std::cout)
//{
//    // Create forward_list of output streams
//    std::forward_list<std::reference_wrapper<std::ostream>> outputs;
//    outputs.push_front(os);  // Always include main output
//
//    // Add file stream if filename provided
//    std::ofstream fileStream;
//    if (!filename.empty()) {
//        fileStream.open(filename);
//        if (fileStream) {
//            outputs.push_front(fileStream);
//        }
//        else {
//            std::cerr << "Error opening file: " << filename << std::endl;
//        }
//    }
//
//    // Print to all streams
//    for (auto& stream : outputs) {
//        stream.get() << "Container: [";
//
//        bool first = true;
//        for (int val : container) {
//            if (!first) stream.get() << ", ";
//            stream.get() << val;
//            first = false;
//        }
//
//        stream.get() << "]" << std::endl;
//    }
//}

/*void print_container(const std::forward_list<int>& container, std::ostream& os)
{
    //bool first = true;
    for (const auto& val : container) {
        
        //if (!first) 
            os << " ";
           os << val;
       // first = false;
    }
    std::cout << "\n-------------------------------------------- \n";
}*/

void print_container(const std::forward_list<int>& container, std::ostream& os)
{
    for (const auto& val : container) 
    {
        os << " ";
        os << val;
    }
    std::cout << "\n-------------------------------------------- \n";
}
void displayMenu()
{
    std::cout << "Меню:\n";
    std::cout << "1. Fill the file with random numbers (цикл)\n";
    std::cout << "2. Fill the file with random numbers (std::generate_n)\n";
    std::cout << "3. Read numbers from file and modification (for)\n";
    std::cout << "4. Read numbers from file and modification (std::transform)\n";
    std::cout << "5. Read numbers from file and modification (std::for_each)\n";
    std::cout << "6. Count the Sum and Average of numebrs\n";
    std::cout << "7. Return result to screen and file\n";
    std::cout << "8. Exit \n";
}
