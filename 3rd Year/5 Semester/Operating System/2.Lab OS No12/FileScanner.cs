//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;

//// Заданы два каталога. Для каждого из них для каждого каталога вывести имя самого большого файла.

//namespace _2.Lab_OS_No12
//{
//    public class FileScanner
//    {
//        private static int nextId = 1; // Статический счетчик для генерации уникальных ID
//        private readonly Queue<string> fileQueue; // Очередь файлов для обработки
//        private readonly SemaphoreSlim availableFiles; // Семафор для доступных файлов
//        private readonly SemaphoreSlim processedFiles; // Семафор для обработанных файлов
//        private readonly object queueLock = new object(); // Объект для синхронизации доступа к очереди

//        private FileInfo largestFile; 
//        private readonly object resultLock = new object(); // Объект для синхронизации доступа к результатам
//        private readonly string directoryPath; // Путь к сканируемой директории

//        public int Id { get; } // Уникальный идентификатор сканера
//        public string DirectoryPath => directoryPath; // Путь к директории
//        public FileInfo LargestFile => largestFile; // Самый большой файл 
//        public int TotalFiles { get; private set; } // Общее количество файлов
//        public int ProcessedFiles { get; private set; } // Количество обработанных файлов

//        /// <param name="directoryPath">Путь к директории для сканирования</param>
//        public FileScanner(string directoryPath)
//        {
//            // Генерируем уникальный ID потокобезопасным способом
//            Id = Interlocked.Increment(ref nextId);
//            this.directoryPath = directoryPath;

//            // Инициализируем очередь и семафоры
//            fileQueue = new Queue<string>();
//            availableFiles = new SemaphoreSlim(0); // Изначально 0 доступных файлов
//            processedFiles = new SemaphoreSlim(0); // Изначально 0 обработанных файлов

//            InitializeFileQueue(); // Заполняем очередь файлами
//        }


//        /// Инициализация очереди файлов из указанной директории
//        private void InitializeFileQueue()
//        {
//            try
//            {
//                //  получаем все файлы в директории и поддиректориях
//                var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
//                TotalFiles = files.Length; // Сохраняем общее количество файлов

//                // Потокобезопасное добавление файлов в очередь
//                lock (queueLock)
//                {
//                    foreach (var file in files)
//                    {
//                        fileQueue.Enqueue(file); // Добавляем файл в очередь
//                    }
//                }

//                // Освобождаем семафор для всех доступных файлов
//                availableFiles.Release(TotalFiles);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Ошибка при сканировании директории {directoryPath}: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Получение следующего файла из очереди для обработки
//        /// </summary>
//        /// <returns>Путь к следующему файлу или null если очередь пуста</returns>
//        public string GetNextFile()
//        {
//            // Ожидаем доступный файл (блокируем поток если файлов нет)
//            availableFiles.Wait();

//            // Потокобезопасное извлечение файла из очереди
//            lock (queueLock)
//            {
//                if (fileQueue.Count > 0)
//                {
//                    return fileQueue.Dequeue(); // Извлекаем и возвращаем файл
//                }
//            }

//            return null; 
//        }

//        /// Обработка результата сканирования файла
//        /// <param name="fileInfo">Информация о обработанном файле</param>
//        public void ProcessFileResult(FileInfo fileInfo)
//        {
//            // Потокобезопасное обновление информации о самом большом файле
//            lock (resultLock)
//            {
//                // Если это первый файл или текущий файл больше предыдущего максимального
//                if (largestFile == null || fileInfo.Length > largestFile.Length)
//                {
//                    largestFile = fileInfo; // Обновляем самый большой файл
//                }
//            }

//            ProcessedFiles++; 
//            processedFiles.Release(); // Освобождаем семафор обработанных файлов
//        }

//        public void WaitForCompletion()
//        {
//            // Ожидаем завершения обработки всех файлов
//            for (int i = 0; i < TotalFiles; i++)
//            {
//                processedFiles.Wait(); // Ожидаем каждый обработанный файл
//            }
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace _2.Lab_OS_No12
{
    /// <summary>
    /// Класс для сканирования файлов с использованием очереди и семафоров
    /// </summary>
    public class FileScanner
    {
        private static int nextId = 1;
        private readonly Queue<string> fileQueue;
        private readonly SemaphoreSlim availableFiles;
        private readonly SemaphoreSlim processedFiles;
        private readonly object queueLock = new object();

        private FileInfo largestFile;
        private readonly object resultLock = new object();
        private readonly string directoryPath;

        public int Id { get; }
        public string DirectoryPath => directoryPath;
        public FileInfo LargestFile => largestFile;
        public int TotalFiles { get; private set; }
        public int ProcessedFiles { get; private set; }

        public FileScanner(string directoryPath)
        {
            Id = Interlocked.Increment(ref nextId);
            this.directoryPath = directoryPath;
            fileQueue = new Queue<string>();
            availableFiles = new SemaphoreSlim(0);
            processedFiles = new SemaphoreSlim(0);

            InitializeFileQueue();
        }

        /// Инициализация очереди файлов с использованием отдельного класса WinAPIFileFinder
        private void InitializeFileQueue()
        {
            try
            {
                // Используем отдельный класс для поиска файлов через WinAPI
                var fileFinder = new WinAPIFileFinder(directoryPath);
                var files = fileFinder.GetAllFiles();

                TotalFiles = files.Count;

                lock (queueLock)
                {
                    foreach (var file in files)
                    {
                        fileQueue.Enqueue(file);
                    }
                }

                availableFiles.Release(TotalFiles);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сканировании директории {directoryPath}: {ex.Message}");
            }
        }

        public string GetNextFile()
        {
            availableFiles.Wait();
            lock (queueLock)
            {
                if (fileQueue.Count > 0)
                {
                    return fileQueue.Dequeue();
                }
            }
            return null;
        }

        public void ProcessFileResult(FileInfo fileInfo)
        {
            lock (resultLock)
            {
                if (largestFile == null || fileInfo.Length > largestFile.Length)
                {
                    largestFile = fileInfo;
                }
            }
            ProcessedFiles++;
            processedFiles.Release();
        }

        public void WaitForCompletion()
        {
            for (int i = 0; i < TotalFiles; i++)
            {
                processedFiles.Wait();
            }
        }
    }
}