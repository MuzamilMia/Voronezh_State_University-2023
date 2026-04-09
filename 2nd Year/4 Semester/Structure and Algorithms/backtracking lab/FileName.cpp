//51. Удалить из заданного числового массива наименьшее число 
// элементов так, чтобы оставшиеся составили возрастающую последовательность 
// (не меняя их порядок следования).

#include <iostream>
const int n = 7;
int arr[n] = { 1,100,2,3,1,4,5 };

int current[n];  
int best[n];     
int best_length = 0; 

void Task(int index, int current_length) {
    if (index < n) 
    {
        // Включаем текущий элемент, если он сохраняет порядок возрастания
        
        if (current_length == 0 || arr[index] > current[current_length - 1]) {
            current[current_length] = arr[index];
            Task(index + 1, current_length + 1);
        }

        Task(index + 1, current_length);
    }

    // Обновляем наилучшую подпоследовательность при достижении конца массива
    if (index == n) {
        bool isIncreasing = true;
        for (int i{1}; i < current_length; i++) {
            if (current[i] <= current[i - 1]) {
                isIncreasing = false;
            }
        }
        // Если подпосле-ность возрас-ая и длиннее найденной ранее
        if (isIncreasing && current_length > best_length) {
            best_length = current_length;
            // Сохраняем новую наилучшую подпоследовательность
            for (int i = 0; i < current_length; i++) {
                best[i] = current[i];
            }
        }
    }
}

int main() 
{
    Task(0, 0);

    std::cout << "\nLongest Increasing Subsequence: ";
    for (int i = 0; i < best_length; i++) {
        std::cout << best[i] << " ";
    }
    std::cout << "\n";

    return 0;
}

 
//#include <iostream>
//#include <vector>
//
//void backtrack(const std::vector<int>& arr, int index, std::vector<int>& current, int& max_length, std::vector<int>& best_sequence) {
//    // Base case: if we reach the end of the array, update max_length and best_sequence if needed
//
//    if (index == arr.size()) {
//        if (current.size() > max_length) {
//            max_length = current.size();
//            best_sequence = current; 
//        }
//    }
//    else
//    {
//        //if (current.empty() || arr[index] > current.back()) 
//        //{
//        //    //this should be the loop in place of useing the resursiv call. 
//        //    current.push_back(arr[index]);
//        //    backtrack(arr, index + 1, current, max_length, best_sequence); 
//        //    current.pop_back(); // Backtrack by removing the element
//        //}
//
//        if (current.empty() || arr[index] > current.back()) 
//        {
//            current.push_back(arr[index]); 
//            for (int i = index + 1; i <= arr.size(); ++i) {
//                if (i == arr.size()) {
//                    if (current.size() > max_length) {
//                        max_length = current.size();
//                        best_sequence = current;
//                    }
//                }
//                else if (current.empty() || arr[i] > current.back()) {
//                    current.push_back(arr[i]); // Include arr[i]
//                    // Need to continue exploring, but we can't recurse here
//                }
//                // Backtrack: remove the last included element
//                if (i < arr.size()) {
//                    current.pop_back();
//                }
//            }
//            current.pop_back(); // Backtrack by removing arr[index]
//        }
//
//        backtrack(arr, index + 1, current, max_length, best_sequence); // Recurse without including
//
//    }
//}
//
//
//int minElementsToRemove(const std::vector<int>& arr) {
//    if (!arr.empty()) {
//
//        std::vector<int> current;
//        std::vector<int> best_sequence; 
//        int max_length = 0; 
//
//        backtrack(arr, 0, current, max_length, best_sequence);
//
//        std::cout << "возрастающая последовательность: ";
//        for (int x : best_sequence) {
//            std::cout << x << " ";
//        }
//
//        return arr.size() - max_length;
//    }
//}
//
//int main() {
//    std::vector<int> arr = { 1, 100, 2, 3, 1,4, 5 };
//
//    int result = minElementsToRemove(arr);
//    std::cout << "\nМинимальное количество элементов для удаления: " << result << std::endl;
//
//    return 0;
//}

//#include <iostream>
//
//const int MAX_N = 100; // Maximum array size
//
//void backtrack(const int arr[], int n, int index, int current[], int& current_size, int& max_length) {
//    // Base case: if we reach the end of the array, update max_length if needed
//    if (index == n) {
//        if (current_size > max_length) {
//            max_length = current_size;
//        }
//    }
//    else {
//        // Option 1: Include the current element if it maintains the increasing order
//        if (current_size == 0 || arr[index] > current[current_size - 1]) {
//            current[current_size] = arr[index]; // Include arr[index]
//            current_size++; // Increment size
//            backtrack(arr, n, index + 1, current, current_size, max_length); // Recurse
//            current_size--; // Backtrack by reducing size
//        }
//        // Option 2: Exclude the current element
//        backtrack(arr, n, index + 1, current, current_size, max_length); // Recurse without including
//    }
//}
//
//int minElementsToRemove(const int arr[], int n) {
//    if (n == 0) return 0;
//
//    int current[MAX_N]; // Current subsequence
//    int current_size = 0; // Tracks size of current subsequence
//    int max_length = 0; // Length of the longest increasing subsequence
//    backtrack(arr, n, 0, current, current_size, max_length);
//
//    // Minimum elements to remove = total elements - length of LIS
//    return n - max_length;
//}
//
//int main() {
//    int n;
//    std::cout << "Введите размер массива: ";
//    std::cin >> n;
//
//    if (n < 0 || n > MAX_N) {
//        std::cout << "Недопустимый размер массива!" << std::endl;
//        return 1;
//    }
//
//    int arr[MAX_N];
//    std::cout << "Введите элементы массива: ";
//    for (int i = 0; i < n; ++i) {
//        std::cin >> arr[i];
//    }
//
//    int result = minElementsToRemove(arr, n);
//    std::cout << "Минимальное количество элементов для удаления: " << result << std::endl;
//
//    return 0;
//}