#include <iostream>
#include <Windows.h>
#include <process.h>

const size_t rows = 5;
const size_t columns = 5;
const size_t COUNT = rows * columns;
const size_t NTHREADS = 4;

struct INFORM {
    int (*matrix)[columns];
    size_t left, right;
    int sum{};
};

bool repeat_number(int num) {
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

int least_sig_number(int num) {
    return std::abs(num) % 10;
}

unsigned __stdcall sum(void* arg) {
    INFORM* inform = (INFORM*)arg;
    int local_sum{};
    for (size_t i = inform->left; i < inform->right; ++i) {
        size_t row = i / columns;
        size_t col = i % columns;
        if (repeat_number(inform->matrix[row][col])) {
            local_sum += least_sig_number(inform->matrix[row][col]);
        }
    }
    inform->sum = local_sum;
    if (inform->right != COUNT)
        _endthreadex(0);
    return 0;
}

int sum_parallel(int (*matrix)[columns]) {
    HANDLE t[NTHREADS - 1];
    size_t block = COUNT / NTHREADS;
    INFORM inform[NTHREADS];

    for (size_t i = 0; i < NTHREADS; ++i) {
        inform[i].matrix = matrix;
        inform[i].left = block * i;
        inform[i].right = (i != NTHREADS - 1) ? block * (i + 1) : COUNT;
        if (i != NTHREADS - 1)
            t[i] = (HANDLE)_beginthreadex(nullptr, 0, &sum, inform + i, 0, nullptr);
    }

    sum(inform + NTHREADS - 1);

    WaitForMultipleObjects(NTHREADS - 1, t, true, INFINITE);

    for (size_t i = 0; i < NTHREADS - 1; ++i)
        CloseHandle(t[i]);

    int global_sum{};
    for (size_t i = 0; i < NTHREADS; ++i)
        global_sum += inform[i].sum;

    return global_sum;
}

int sum_non_parallel(int (*matrix)[columns]) {
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
    for (size_t i = 0; i < rows; ++i) {
        for (size_t j = 0; j < columns; ++j) {
            std::cout << matrix[i][j] << " ";
        }
        std::cout << '\n';
    }
    std::cout << '\n';
}

int main() {
    int matrix[rows][columns];
    init_matrix_fixed(matrix);

    std::cout << "Матрица:\n";
    print(matrix);

    std::cout << "Непараллельная сумма: " << sum_non_parallel(matrix) << '\n';
    std::cout << "Параллельная сумма:   " << sum_parallel(matrix) << '\n';

    return 0;
}