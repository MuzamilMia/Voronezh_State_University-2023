//#include <iostream>
//#include <mutex>
//#include <queue>
//#include <thread>
//#include <atomic>
//
//
//const size_t rows = 5;
//const size_t columns = 5;
//const size_t COUNT = rows * columns;
//const size_t THREAD = 4; // Количество потребителей
//
//// Класс потокобезопасной очереди
//class ThreadSafeQueue {
//private:
//    std::mutex mutex; // Мьютекс для потокобезопасности
//    std::queue<size_t> queue; 
//public:
//    ThreadSafeQueue() {}
//    void push(size_t index) 
//    {
//        std::lock_guard<std::mutex> lock(mutex); 
//        queue.push(index);
//    }
//    bool try_pop(size_t& index) 
//    {
//        bool flag{};
//        std::lock_guard<std::mutex> lock(mutex);
//        if (!queue.empty()) 
//        {
//            index = queue.front();
//            queue.pop();
//            flag = true;
//        }
//        return flag;
//    }
//    bool empty() {
//        std::lock_guard<std::mutex> lock(mutex);
//        return queue.empty();
//    }
//};
//
//ThreadSafeQueue TSQ; // Глобальная очередь
//std::mutex mutex_task; // Мьютекс для защиты общей суммы
//std::atomic<bool> is_done(false); // Флаг завершения работы
//
//bool repeat_number(int num)
//{
//    num = std::abs(num);
//    bool digit_seen[10] = { false };
//    bool flag = false;
//    while (num > 0 && !flag) 
//    {
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
//// Функция потребителя для обработки индексов
//void consumer(int (*matrix)[columns], long& global_sum)
//{
//    while (!is_done || !TSQ.empty()) // Продолжать, пока очередь не пуста или не завершено
//    {
//        size_t index;
//        if (TSQ.try_pop(index)) // Попытка извлечь индекс из очереди
//        {
//            size_t row = index / columns; // Преобразование 1D индекса в строку
//            size_t col = index % columns; // Преобразование 1D индекса в столбец
//            if (repeat_number(matrix[row][col])) {
//                std::lock_guard<std::mutex> locker(mutex_task); // Блокировка мьютекса
//                global_sum += least_sig_number(matrix[row][col]); // Обновление общей суммы
//            }
//        }
//    }
//}
//
//int sum_parallel(int (*matrix)[columns])
//{
//    long global_sum = 0; // Общая сумма
//    std::thread consumers[THREAD]; // Массив потоков-потребителей
//
//    // Запуск потребителей
//    for (size_t i = 0; i < THREAD; ++i) {
//        consumers[i] = std::thread(consumer, matrix, std::ref(global_sum)); // Запуск рабочего потока
//    }
//
//    // Производитель: добавление индексов в очередь
//    for (size_t i = 0; i < COUNT; ++i) {
//        TSQ.push(i); // Добавление индекса в очередь
//    }
//
//    is_done = true;
//
//    // Ожидание завершения потребителей
//    for (size_t i = 0; i < THREAD; ++i) {
//        consumers[i].join(); // Ожидание завершения рабочих потоков
//    }
//
//    return global_sum;
//}
//
//
//int sum_non_parallel(int (*matrix)[columns])
//{
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
//void init_matrix_fixed(int (*matrix)[columns])
//{
//    int fixed_matrix[rows][columns] = {
//        {99, 99, 99, 99, 11},
//        {99, 12, 23, 45, 67},
//        {22, 89, 78, 56, 34},
//        {55, 23, 45, 67, 89},
//        {78, 56, 34, 12, 23}
//    };
//    for (size_t i = 0; i < rows; ++i)
//        for (size_t j = 0; j < columns; ++j)
//            matrix[i][j] = fixed_matrix[i][j];
//}
//void print(int (*matrix)[columns])
//{
//    for (size_t i = 0; i < rows; ++i) {
//        for (size_t j = 0; j < columns; ++j) {
//            std::cout << matrix[i][j] << " ";
//        }
//        std::cout << '\n';
//    }
//    std::cout << '\n';
//}
//
//int main()
//{
//    int matrix[rows][columns];
//    init_matrix_fixed(matrix); // Используем фиксированную матрицу для суммы 62
//
//    std::cout << "Матрица:\n";
//    print(matrix);
//
//    std::cout << "Непараллельная сумма: " << sum_non_parallel(matrix) << '\n';
//    std::cout << "Параллельная сумма:   " << sum_parallel(matrix) << '\n';
//
//    return 0;
//}