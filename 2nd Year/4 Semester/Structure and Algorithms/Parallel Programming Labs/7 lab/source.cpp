#include <iostream>
#include <omp.h>

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


int sum(int (*matrix)[columns], size_t size)
{
    int result = 0;
    for (size_t i = 0; i < size; ++i) {
        size_t row = i / columns; 
        size_t col = i % columns; 
        if (repeat_number(matrix[row][col])) {
            result += least_sig_number(matrix[row][col]); 
        }
    }
    return result;
}


int sum_parallel(int (*matrix)[columns])
{
    int global_sum = 0;
    omp_set_num_threads(NTHREADS); // Установка числа потоков
    #pragma omp parallel shared(matrix) reduction(+:global_sum)
    {
        int local_sum = 0;
        #pragma omp for  //divide the loop 
        for (int i = 0; i < COUNT; ++i) {
            size_t row = i / columns; 
            size_t col = i % columns; 
            if (repeat_number(matrix[row][col])) {
                local_sum += least_sig_number(matrix[row][col]); 
            }
        }
        global_sum += local_sum; 
    }
    return global_sum;
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
    return sum(matrix, COUNT);
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