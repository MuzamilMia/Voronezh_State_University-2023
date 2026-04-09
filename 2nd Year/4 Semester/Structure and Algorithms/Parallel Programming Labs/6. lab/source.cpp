#include "queue.h"
#include <iostream>
#include <condition_variable>
#include <thread>
#include <mutex>
#include <Windows.h>


const size_t rows = 5;
const size_t columns = 5;
const size_t COUNT = rows * columns;
const size_t NTHREADS = 5; // 2 производителя + 3 потребителя

ThreadsafeQueue que; // Потокобезопасная очередь
std::condition_variable cv; // Условная переменная
std::mutex mut; // Мьютекс для cv и global_sum
volatile long volume_work_producer = 5; // Количество задач для производителей (5 строк)
volatile long volume_work_consumer = 5; // Количество задач для потребителей
std::mutex sum_mutex; // Мьютекс для global_sum
long global_sum = 0; // Общая сумма

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

void init_matrix_fixed(int (*matrix)[columns])
{
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

int sum_non_parallel(int (*matrix)[columns])
{
    int result = 0;
    for (size_t i = 0; i < rows; ++i) {
        for (size_t j = 0; j < columns; ++j) {
            if (repeat_number(matrix[i][j])) {
                result += least_sig_number(matrix[i][j]); 
            }
        }
    }
    return result;
}

// Функция производителя
void task_producer(int id, int (*matrix)[columns])
{
    srand(GetTickCount64() + id); // Уникальный seed для каждого производителя

    //checks if tasks remain.
    while (_InterlockedExchangeAdd(&volume_work_producer, -1) > 0)
    {
        size_t row = volume_work_producer; 
        if (row < rows) {
            int* arr = new int[columns]; // Массив для строки
            for (size_t j = 0; j < columns; ++j) {
                arr[j] = matrix[row][j]; // Копируем строку
            }
            que.push(arr); // Добавляем в queue
            {
                std::lock_guard<std::mutex> lock(mut);
                cv.notify_all(); // Уведомляем потребителей
            }
            //std::this_thread::sleep_for(std::chrono::milliseconds(2));
        }
    }
}

void task_consumer(int (*matrix)[columns])
{
    while (_InterlockedExchangeAdd(&volume_work_consumer, -1) > 0)
    {
        int* arr = nullptr;
        {
            std::unique_lock<std::mutex> locker(mut);
            cv.wait_for(locker, std::chrono::seconds(5), []() { return !que.empty(); });
        }
        if (que.try_pop(arr)) // Извлечение строки
        {
            int local_sum = 0;
            for (size_t j = 0; j < columns; ++j) {
                if (repeat_number(arr[j])) {
                    local_sum += least_sig_number(arr[j]); // Локальная сумма
                }
            }
            {
                std::lock_guard<std::mutex> lock(sum_mutex); // Защита global_sum
                global_sum += local_sum; // Обновление общей суммы
            }
            delete[] arr; // Освобождение памяти
            //std::this_thread::sleep_for(std::chrono::milliseconds(5));
        }
        else
        {
            _InterlockedExchangeAdd(&volume_work_consumer, 1); // Возврат задачи
        }
    }
}

int sum_parallel(int (*matrix)[columns])
{
    global_sum = 0; 
    std::thread wor_thread[NTHREADS];

    // Запуск производителей и потребителей
    for (size_t i = 0; i < NTHREADS; ++i) {
        if (i < 2)
            wor_thread[i] = std::thread(task_producer, 10 * i, matrix); // Производители
        else
            wor_thread[i] = std::thread(task_consumer, matrix); // Потребители
    }

    for (size_t i = 0; i < NTHREADS; ++i) {
        wor_thread[i].join(); 
    }

    return global_sum;
}


void print(int (*matrix)[columns])
{
    for (size_t i = 0; i < rows; ++i) {
        for (size_t j = 0; j < columns; ++j) {
            std::cout << matrix[i][j] << " ";
        }
        std::cout << '\n';
    }
    std::cout << '\n';
}

int main()
{
    int matrix[rows][columns];
    init_matrix_fixed(matrix); 

    std::cout << "Матрица:\n";
    print(matrix);

    std::cout << "Непараллельная сумма: " << sum_non_parallel(matrix) << '\n';
    std::cout << "Параллельная сумма:   " << sum_parallel(matrix) << '\n';

    return 0;
}