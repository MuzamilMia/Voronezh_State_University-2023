#include <iostream>
#include <thread>

//We are using the std::thread. 
// 17. Дана прямоугольная целочисленная матрица. Распараллеливание по элементам. 
// Найти сумму младших разрядов тех чисел, в которых есть повторяющиеся цифры.

const size_t rows = 5;
const size_t columns = 5;
const size_t COUNT = rows * columns;
const size_t NTHREADS = 4;


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

// Функция для вычисления локальной суммы в диапазоне индексов
void sum(int (*matrix)[columns], size_t left, size_t right, int& result)
{
    result = 0;
    for (size_t i = left; i < right; ++i) {
        size_t row = i / columns; // Преобразование 1D индекса в строку
        size_t col = i % columns; // Преобразование 1D индекса в столбец
        if (repeat_number(matrix[row][col])) {
            result += least_sig_number(matrix[row][col]); 
        }
    }
}

int sum_parallel(int (*matrix)[columns])
{
    std::thread t[NTHREADS - 1]; 
    int result[NTHREADS - 1]; // Локальные суммы для рабочих потоков
    size_t block = COUNT / NTHREADS; // Размер блока

    // Запуск рабочих потоков
    for (size_t i = 0; i < NTHREADS - 1; ++i) {
        t[i] = std::thread(sum, matrix, block * i, block * (i + 1), std::ref(result[i])); // Запуск потока
    }

    // Основной поток обрабатывает последний блок
    int global_sum;
    sum(matrix, block * (NTHREADS - 1), COUNT, global_sum);

  
    for (size_t i = 0; i < NTHREADS - 1; ++i) {
        t[i].join(); 
        global_sum += result[i];
    }
    return global_sum;
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