#include <iostream>
#include <Windows.h>
#include <process.h>


const size_t rows = 5;
const size_t columns = 5;
const size_t TOTAL = rows * columns;
const size_t NTHREADS = 4; // Количество потоков


struct INFORM {
    int (*matr)[columns];  
    size_t left, right;    
    int sum;               
    bool is_worker_thread; 
};


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


unsigned __stdcall processElements(void* arg)
{
    INFORM* inform = (INFORM*)arg;
    int local_sum = 0;
    for (size_t i = inform->left; i < inform->right; ++i)
    {
        size_t row = i / columns; 
        size_t col = i % columns; 
        if (repeat_number(inform->matr[row][col]))
        {
            local_sum += least_sig_number(inform->matr[row][col]);
        }
    }
    inform->sum = local_sum; // Сохранение локальной суммы
    if (inform->is_worker_thread) {
        _endthreadex(0); 
    }
    return 0; 
}


int sum_parallel(int (*matrix)[columns])
{
    int global_sum = 0; 
    HANDLE t[NTHREADS - 1];
    INFORM inform[NTHREADS]; 
    size_t block = TOTAL / NTHREADS;

    for (size_t i = 0; i < NTHREADS; ++i)
    {
        inform[i].matr = matrix;
        inform[i].left = block * i;
        inform[i].right = (i != NTHREADS - 1) ? block * (i + 1) : TOTAL;
        inform[i].sum = 0; // Инициализация локальной суммы
        inform[i].is_worker_thread = (i != NTHREADS - 1);
        if (i != NTHREADS - 1)
        {
            t[i] = (HANDLE)_beginthreadex(nullptr, 0, &processElements, &inform[i], 0, nullptr);
        }
    }

    // Основной поток обрабатывает последний блок
    processElements(&inform[NTHREADS - 1]);
 
    WaitForMultipleObjects(NTHREADS - 1, t, TRUE, INFINITE);

    // Освобождение дескрипторов потоков
    for (size_t i = 0; i < NTHREADS - 1; ++i) {
        CloseHandle(t[i]);
    }

    
    for (size_t i = 0; i < NTHREADS; ++i) {
        global_sum += inform[i].sum;
    }

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

void init_matrix(int (*matrix)[columns]) {
    for (int i = 0; i < rows; ++i)
        for (int j = 0; j < columns; ++j)
            matrix[i][j] = rand() % 100;
}

void print(int (*matrix)[columns]) {
    for (int i = 0; i < rows; ++i) {
        for (int j = 0; j < columns; ++j)
            std::cout << matrix[i][j] << " ";
        std::cout << '\n';
    }
    std::cout << "\n";
}

int main()
{
    srand(GetTickCount());
    int matrix[rows][columns];
    init_matrix(matrix); 

    std::cout << "Матрица:\n";
    print(matrix);

    int parallel_result = sum_parallel(matrix);
    int non_parallel_result = sum_non_parallel(matrix);

    std::cout << "Непараллельная сумма: " << non_parallel_result << "\n";
    std::cout << "Параллельная сумма: " << parallel_result << "\n";

    return 0;
}