////17. Дана прямоугольная целочисленная матрица. Распараллеливание по элементам.
//// Найти сумму младших разрядов тех чисел, в которых есть повторяющиеся цифры.
//// -- Мьютекс — это примитив синхронизации, который гарантирует, что только 
//// один поток может получить доступ к общему ресурсу (очереди) в один момент времени.
//// -- A C++ utility that automatically manages the locking and unlocking of a mutex 
//// within a scope.
//// -- Утилита C++, которая автоматически управляет блокировкой и разблокировкой 
//// мьютекса в пределах области действия.
//
//#include <iostream>
//#include <thread>
//#include <queue>
//#include <mutex>
//#include <vector>
//#include <string>
//
//const size_t rows = 5;
//const size_t columns = 5;
//const size_t TOTAL = rows * columns;
//const size_t NTHREADS = 4; // Number of threads
//
//
//class Thread_Queue {
//private:
//    std::queue<int> queue;
//    std::mutex mtx;
//
//public:
//    void push(int value) {
//        std::lock_guard<std::mutex> lock(mtx); // Lock with C++ mutex
//        queue.push(value);
//    }
//
//    bool pop(int& value) {
//        std::lock_guard<std::mutex> lock(mtx); // Lock with C++ mutex
//        bool flag = false;
//        if (!queue.empty())
//        {
//            value = queue.front();
//            queue.pop();
//            flag = true;
//        }
//        return flag;
//    }
//
//    bool empty() {
//        std::lock_guard<std::mutex> lock(mtx);
//        return queue.empty();
//    }
//};
//
//struct INFORM {
//    int (*matr)[columns]; 
//    size_t left, right;   // Range of elements to process
//    Thread_Queue* queue; // Shared thread-safe queue
//};
//
//bool repeat_number(int num)
//{
//    num = std::abs(num);
//    bool digit_seen[10] = { false }; 
//    bool flag = false;       
//    while (num > 0 && !flag) {
//        int digit = num % 10;
//        if (digit_seen[digit])
//            flag = true;
//        digit_seen[digit] = true;
//        num /= 10;
//    }
//    return flag;
//}
//
//
//int least_sig_number(int num)
//{
//    return std::abs(num) % 10; 
//}
//
//// Thread function to process a range of elements
//void processElements(void* arg)
//{
//    INFORM* inform = (INFORM*)arg; /// Получение данных потока
//    for (size_t i = inform->left; i < inform->right; ++i)
//    {
//        size_t row = i / columns; /// Преобразование 1D индекса в строку
//        size_t col = i % columns;
//        if (repeat_number(inform->matr[row][col]))
//        {
//            inform->queue->push(least_sig_number(inform->matr[row][col]));
//        }
//    }
//}
//
//
//int sum_parallel(int (*matrix)[columns])
//{
//    Thread_Queue queue; //общей потокобезопасной очереди
//    std::thread t[NTHREADS - 1]; // Массив потоков
//    INFORM inform[NTHREADS]; //array with data
//    size_t block = TOTAL / NTHREADS;
//
//    // Initialize thread information
//    for (size_t i = 0; i < NTHREADS; ++i)
//    {
//        inform[i].matr = matrix; // Указатель на матрицу
//        inform[i].left = block * i; // Начало диапазона
//        inform[i].right = (i != NTHREADS - 1) ? block * (i + 1) : TOTAL;
//        inform[i].queue = &queue;
//        if (i != NTHREADS - 1)
//        {
//            t[i] = std::thread(processElements, &inform[i]); // Запуск рабочего потока
//        }
//    }
//
//    // Main thread processes the last block
//    processElements(&inform[NTHREADS - 1]);
//
//    // Wait for all threads to finish
//    for (size_t i = 0; i < NTHREADS - 1; ++i) {
//        t[i].join();
//    }
//
//  
//    int global_sum = 0;
//    int value;
//    while (queue.pop(value)) {
//        global_sum += value;
//    }
//    return global_sum;
//}
//
//
//int sum_non_parallel(int (*matrix)[columns]) {
//    int result = 0;
//    for (size_t i = 0; i < rows; ++i) {
//        for (size_t j = 0; j < columns; ++j) {
//            if (repeat_number(matrix[i][j])) {
//                result += least_sig_number(matrix[i][j]);
//            }
//        }
//    }
//    return result;
//}
//
//
//void init_matrix(int (*matrix)[columns]) {
//    int fixed_matrix[rows][columns] = {
//        {99, 99, 99, 99, 11},
//        {99, 12, 23, 45, 67},
//        {22, 89, 78, 56, 34},
//        {55, 23, 45, 67, 89},
//        {78, 56, 34, 12, 23}
//    };
//    for (int i = 0; i < rows; ++i)
//        for (int j = 0; j < columns; ++j)
//            matrix[i][j] = fixed_matrix[i][j];
//}
//
//
//void print(int (*matrix)[columns]) {
//    for (int i = 0; i < rows; ++i) {
//        for (int j = 0; j < columns; ++j)
//            std::cout << matrix[i][j] << " ";
//        std::cout << '\n';
//    }
//    std::cout << "\n";
//}
//
//int main()
//{
//    int matrix[rows][columns];
//    init_matrix(matrix);
//
//    std::cout << "Matrix:\n";
//    print(matrix);
//
//    int parallel_result = sum_parallel(matrix);
//    int non_parallel_result = sum_non_parallel(matrix);
//
//    std::cout << "Non-parallel sum: " << non_parallel_result << "\n";
//    std::cout << "Parallel sum: " << parallel_result << "\n";
//
//    return 0;
//}


/*17. Дана прямоугольная целочисленная матрица.Распараллеливание по элементам.
 Найти сумму младших разрядов тех чисел, в которых есть повторяющиеся цифры.
 -- Мьютекс — это примитив синхронизации, который гарантирует, что только 
 один поток может получить доступ к общему ресурсу (очереди) в один момент времени.
 -- A C++ utility that automatically manages the locking and unlocking of a mutex 
 within a scope.
 -- Утилита C++, которая автоматически управляет блокировкой и разблокировкой 
 мьютекса в пределах области действия.*/

 #include <iostream>
#include <mutex>
#include <queue>
#include <thread>
#include <chrono>


const int COUNT = 25; 
const int THREAD = 4; 
const size_t rows = 5;
const size_t columns = 5;


struct Pair {
    size_t left, right;
};

// Класс потокобезопасной очереди
class ThreadSafeQueue {
private:
    std::mutex mutex; // Мьютекс для потокобезопасности
    std::queue<Pair> queue; 
public:
    ThreadSafeQueue() {}
    void push(Pair elem) {
        std::lock_guard<std::mutex> lock(mutex); 
        queue.push(elem); 
    }
    bool try_pop(Pair& elem) {
        bool result = false;
        if (!queue.empty()) {
            std::lock_guard<std::mutex> lock(mutex);
            if (!queue.empty()) {
                result = true;
                elem = queue.front();
                queue.pop();
            }
        }
        return result;
    }
    bool empty() {
        std::lock_guard<std::mutex> lock(mutex);
        return queue.empty();
    }
};

ThreadSafeQueue TSQ; // Глобальная очередь
std::mutex mutex_task; // Мьютекс для защиты общей суммы


bool repeat_number(int num)
{
    num = std::abs(num);
    bool digit_seen[10] = { false };
    bool flag = false;
    while (num > 0 && !flag) {
        int digit = num % 10; 
        if (digit_seen[digit])
            flag = true;
        digit_seen[digit] = true;
        num /= 10;
    }
    return flag;
}

// Получение младшей цифры числа
int least_sig_number(int num)
{
    return std::abs(num) % 10; 
}

// Функция потока для обработки диапазона элементов
void process_elements(int (*matrix)[columns], long volatile& global_sum)
{
    Pair pair;
    while (TSQ.try_pop(pair)) 
    { // Попытка извлечь диапазон из очереди
        long local_sum = 0;
        for (size_t i = pair.left; i < pair.right; ++i) {
            size_t row = i / columns; 
            size_t col = i % columns; 
            if (repeat_number(matrix[row][col])) {
                local_sum += least_sig_number(matrix[row][col]); // Накопление локальной суммы
            }
            std::this_thread::sleep_for(std::chrono::milliseconds(1)); // Имитация работы
        }
        std::lock_guard<std::mutex> locker(mutex_task); // Блокировка мьютекса
        global_sum += local_sum; // Обновление общей суммы
    }
}


int sum_parallel(int (*matrix)[columns])
{
    size_t chunk = 10; // Размер блока
    size_t left = 0, right = 0;
    while (right != COUNT) { // Разделение на блоки
        left = right;
        right = right + chunk < COUNT ? right + chunk : COUNT;
        TSQ.push(Pair{ left, right }); // Добавление диапазона в очередь
    }

    std::thread thr[THREAD - 1]; // Массив потоков
    long volatile global_sum = 0; // Общая сумма
    for (size_t i = 0; i < THREAD - 1; ++i) {
        thr[i] = std::thread(process_elements, matrix, std::ref(global_sum)); // Запуск рабочего потока
    }

    process_elements(matrix, global_sum); // Основной поток обрабатывает блоки

    for (size_t i = 0; i < THREAD - 1; ++i) {
        thr[i].join(); // Ожидание завершения рабочих потоков
    }

    return global_sum;
}

int sum_non_parallel(int (*matrix)[columns]) {
    int result = 0;
    for (size_t i = 0; i < rows; ++i) {
        for (size_t j = 0; j < columns; ++j) {
            if (repeat_number(matrix[i][j])) {
                result += least_sig_number(matrix[i][j]); // Добавляем младшую цифру
            }
        }
    }
    return result;
}
void init_matrix_fixed(int (*matrix)[columns]) {
    int fixed_matrix[rows][columns] = {
        {99, 99, 99, 99, 11},
        {99, 12, 23, 45, 67},
        {22, 89, 78, 56, 34},
        {55, 23, 45, 67, 89},
        {78, 56, 34, 12, 23}
    };
    for (size_t i = 0; i < rows; ++i)
        for (size_t j = 0; j < columns; ++j)
            matrix[i][j] = fixed_matrix[i][j];
}
void print(int (*matrix)[columns]) {
    for (size_t i = 0; i < rows * columns; ++i) {
        size_t row = i / columns;
        size_t col = i % columns;
        std::cout << matrix[row][col] << ' ';
    }
    std::cout << '\n';
}

int main()
{
    int matrix[rows][columns];
    init_matrix_fixed(matrix); // Используем фиксированную матрицу для суммы 62

    std::cout << "Array: ";
    print(matrix);

    std::cout << "Non parallel sum: " << sum_non_parallel(matrix) << '\n';
    std::cout << "Parallel sum:     " << sum_parallel(matrix) << '\n';

    return 0;
}