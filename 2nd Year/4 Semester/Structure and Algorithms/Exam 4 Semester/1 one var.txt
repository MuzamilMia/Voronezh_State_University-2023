#include<iostream>
#include<fstream>
#include<functional>
#include "HashTable_1.h"
#include<map>
#include<thread>
#include <atomic>
#include <intrin.h>

//task 1
std::vector<std::string> task1(std::string filename, std::function<bool(const std::string&)> predicate)
{
    std::map<std::string, int> wordCount;
    std::ifstream file(filename);
    std::string word;

    if (file)
    {
        while (file >> word)
            wordCount[word]++;
        file.close();

        std::vector<std::string> mywords;
        int maxFreq = 0;
        for (const auto& pair : wordCount)
        {
            if (predicate(pair.first))
            {
                mywords.push_back(pair.first);
                maxFreq = std::max(maxFreq, pair.second);
            }
        }

        std::vector<std::string> result;
        if (maxFreq > 0)
        {
            for (auto& word : mywords)
            {
                if (wordCount[word] == maxFreq)
                    result.push_back(word + " (" + std::to_string(maxFreq) + ")");
            }
        }
        return result.empty() ? std::vector<std::string>{"No words found "} : result;
    }
}
//task 2 (It has been done inside the Header file. 
bool HashTable_1::remove1(HashTable_1& table, std::string key)
{
    bool result = false;
    int size = 150; 
    int index = table.hash(key);
    int start = index;
    while (table.data[index].used != 0 && (index != start || result == false))
    {
        if (table.data[index].used == 1 && table.data[index].elem.key == key)
        {
            result = true;
            table.data[index].used = -1; 
        }
        index = (index + 1) % size;
    }
    return result; 
}
bool HashTable_1::remove(std::string key)
{
    int index = hash(key);
    int originalIndex = index;
    bool first = true;
    bool result = false;

    while (data[index].used != 0 && (first || index != originalIndex) && !result)
    {
        if (data[index].used == 1 && data[index].elem.key == key)
        {
            result = true;
            data[index].used = -1; 
        }
        index = (index + 1) % size;
        first = false;
    }

    return result;
}
//task 3
const int n = 10;
int count_zeros = 0;
int matrix[n][n];
int min_cost = -1;
int max_cost = -1;
int sum{};
void task3(int i, int j)
{
    if (i == n - 1 && j == n - 1 && count_zeros == 2)
    {
        if (sum < min_cost || min_cost == -1)
            min_cost = sum;
        if (sum > max_cost)
            max_cost = sum;
    }
    else
    {
        if (j + 1 < n)
        {
            sum += matrix[i][j + 1];
            count_zeros += (matrix[i][j + 1] == 0) ? 1 : 0;
            task3(i, j + 1);
            sum -= matrix[i][j + 1];
            count_zeros -= (matrix[i][j + 1] == 0) ? 1 : 0;
        }
        if (i + 1 < n)
        {
            sum += matrix[i + 1][j];
            count_zeros += (matrix[i + 1][j] == 0) ? 1 : 0;
            task3(i + 1, j);
            sum -= matrix[i + 1][j];
            count_zeros -= (matrix[i + 1][j] == 0) ? 1 : 0;
        }
    }
}

//task 4
const size_t COUNT = 555;
const size_t NTHREADS = 4;
std::atomic<int> global_max;
std::atomic<int> global_num;
void Max(int* arr, size_t left, size_t right)
{
    int max_val{ arr[left] };
    size_t num{ left };

    for (size_t i = left + 1; i < right; ++i)
    {
        if ((abs(arr[i]) % 2) && ((abs(max_val) % 2) == 0 || arr[i] >= max_val))
        {
            max_val = arr[i];
            num = i;
        }
    }

    int curr_max = global_max.load();
    if (max_val > curr_max || (max_val == curr_max && num > global_num.load()) || curr_max % 2 == 0)
    {
        if (global_max.compare_exchange_weak(curr_max, max_val)) {
            global_num.store(num);
        }
    }
}
size_t max_parallel(int* arr)
{
    std::thread t[NTHREADS - 1];
    size_t block = COUNT / NTHREADS;

    global_max = INT_MIN;    
    global_num = -1;

    for (size_t i = 0; i < NTHREADS - 1; ++i) {
        t[i] = std::thread(Max, arr, block * i, block * (i + 1));
    }

    Max(arr, block * (NTHREADS - 1), COUNT);

    for (size_t i = 0; i < NTHREADS - 1; ++i)
        t[i].join();

    return global_num.load();
}

//task 5


int main()
{
    //--------------- KUM 1 -----------------------
    
    //task 1
    char targ_letter = 'e';
    auto lambda = [targ_letter](const std::string& word) -> bool
        {
            bool found = false;
            for (char c : word)
                if (c == targ_letter)
                    found = true;
            return found;
        };
    std::vector<std::string> result = task1("Text_1.txt", lambda);
    for ( auto& word : result)
        std::cout << word << '\n';
    //---------------------------------
    //Task 2
    HashTable_1 table;
    std::cout << "Before Deletion:\n";
    table.print();
    std::string keyToRemove = "8905185708"; 
    bool removed = table.remove(keyToRemove);
    if (removed)
    {
        std::cout << "Successfully removed.\n The table is: \n";
        table.print();
    }
    else
        std::cout << "Not found\n";
    //---------------------------------
    //Task 3
    std::ifstream file("Matrix_3_1.txt");
    for (int i{}; i < n; ++i)
        for (int j{}; j < n; ++j)
            file >> matrix[i][j];
    task3(0, 0);
    if (min_cost != -1)
        std::cout << "\nMin cost: " << min_cost << '\n';
    if (max_cost != -1)
        std::cout << "Max cost: " << max_cost << '\n';

    //Task 4
    std::ifstream file1("Numbers_4.txt");
    int arr[COUNT];
    for (size_t i = 0; i < COUNT; ++i)
    {
        file1 >> arr[i];
    }
    //std::cout << "parallel = " << max_parallel(arr) << '\n';

    int last_index = max_parallel(arr);
    std::cout << "Max odd Number: " << global_max.load() << '\n';
    std::cout << "The index of this number is: " << last_index << '\n';

	std::cin.ignore();
	return 0;
}