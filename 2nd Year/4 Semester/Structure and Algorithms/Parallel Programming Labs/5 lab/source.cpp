/*17. Дана прямоугольная целочисленная матрица.Распараллеливание по элементам.
 Найти сумму младших разрядов тех чисел, в которых есть повторяющиеся цифры.
 -- Мьютекс — это примитив синхронизации, который гарантирует, что только
 один поток может получить доступ к общему ресурсу (очереди) в один момент времени.
 -- A C++ utility that automatically manages the locking and unlocking of a mutex
 within a scope.
 -- Утилита C++, которая автоматически управляет блокировкой и разблокировкой
 мьютекса в пределах области действия.*/

//Организация пула потоков. Создание потокобезопасного стека/очереди с использованием объектов ядра.

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
int least_sig_number(int num)
{
    return std::abs(num) % 10;
}

// Функция потока для обработки диапазона элементов
void process_elements(int (*matrix)[columns], int& global_sum)
{
    Pair pair;
    while (TSQ.try_pop(pair))
    { // Попытка извлечь диапазон из очереди
        long local_sum = 0;
        for (size_t i = pair.left; i < pair.right; ++i) {
            size_t row = i / columns;
            size_t col = i % columns;
            if (repeat_number(matrix[row][col])) {
                local_sum += least_sig_number(matrix[row][col]);
            }
            //std::this_thread::sleep_for(std::chrono::milliseconds(1)); // Имитация работы
        }
        std::lock_guard<std::mutex> locker(mutex_task); // Блокировка мьютекса
        global_sum += local_sum;
    }
}


int sum_parallel(int (*matrix)[columns])
{
    size_t chunk = 10; // Размер блока
    size_t left = 0, right = 0;
    while (right != COUNT) 
    { // Разделение на блоки
        left = right;
        right = right + chunk < COUNT ? right + chunk : COUNT;
        TSQ.push(Pair{ left, right }); // Добавление диапазона в очередь
    }

    std::thread thr[THREAD - 1]; // Массив потоков
    int global_sum = 0; // Общая сумма
    for (size_t i = 0; i < THREAD - 1; ++i) {
        thr[i] = std::thread(process_elements, matrix, std::ref(global_sum)); 
    }

    //main thread
    process_elements(matrix, global_sum); 

    for (size_t i = 0; i < THREAD - 1; ++i) {
        thr[i].join();
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