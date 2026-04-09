#pragma once
#include <queue>
#include <Windows.h>

class ThreadsafeQueue {
private:
    std::queue<int*> que; 
    HANDLE hSemaphore; // Семафор для потокобезопасности

public:
    ThreadsafeQueue() {
        hSemaphore = CreateSemaphore(NULL, 1, 1, NULL); // Инициализация семафора
    }

    ~ThreadsafeQueue() {
        CloseHandle(hSemaphore); 
    }

    bool empty()
    {
        return que.empty();
    }
    void push(int* arr)
    {
        WaitForSingleObject(hSemaphore, INFINITE); // Ожидание семафора
        try
        {
            que.push(arr); // Добавление массива
            ReleaseSemaphore(hSemaphore, 1, NULL); // Освобождение семафора
        }
        catch (std::exception)
        {
            ReleaseSemaphore(hSemaphore, 1, NULL);
            throw; // Переброс исключения
        }
    }
    bool try_pop(int*& value)
    {
        bool result = false;
        WaitForSingleObject(hSemaphore, INFINITE); // Ожидание семафора
        try
        {
            if (!que.empty())
            {
                result = true;
                value = que.front(); // Извлечение массива
                que.pop();
            }
            ReleaseSemaphore(hSemaphore, 1, NULL); // Освобождение семафора
        }
        catch (std::exception)
        {
            ReleaseSemaphore(hSemaphore, 1, NULL);
            throw; // Переброс исключения
        }
        return result;

    }
};