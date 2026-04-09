#include <iostream>
#include <limits>

const int size_run = 20;        
const int max_size_run = 20;

void Print(const int* arr, int size) 
{
    for (int i = 0; i < size; ++i)
        std::cout << arr[i] << " ";
    std::cout << "\n";
}

void MergeSort(int* data, int N) 
{
    int runs[size_run][max_size_run];
    int runSizes[size_run];         

    // ФАЗА 1: Находим естественные runs
    int run_count = 0;
    int cur_run_size = 0;
    runs[run_count][cur_run_size++] = data[0];

    for (int i = 1; i < N; ++i)
    {
        if (data[i] >= data[i - 1]) 
            runs[run_count][cur_run_size++] = data[i];
        else 
        {
            runSizes[run_count] = cur_run_size;
            run_count++;
            cur_run_size = 0;
            runs[run_count][cur_run_size++] = data[i];
        }
    }
    runSizes[run_count] = cur_run_size;
    run_count++;

    // ФАЗА 2: Однофазное многопутевое слияние runs
    int index[size_run] = { 0 }; // текущие позиции в каждом run

    for (int k = 0; k < N; ++k) {
        int minValue = INT_MAX;
        int minRun = -1;

        for (int i = 0; i < run_count; ++i)
        {
            if (index[i] < runSizes[i]) {
                int val = runs[i][index[i]];
                if (val < minValue) {
                    minValue = val;
                    minRun = i;
                }
            }
        }

        data[k] = minValue;
        index[minRun]++;
    }
}

int main() {
    int data[] = { 1, 4, 5, 2, 6, 8, 3, 7, 9, 0, 11, 12, 10, 15, 14, 16, 18, 17, 19, 13 };
    int N = sizeof(data) / sizeof(data[0]);

    std::cout << "Initial array:\n";
    Print(data, N);

    MergeSort(data, N);

    std::cout << "\nSorted array:\n";
    Print(data, N);

    return 0;
}
