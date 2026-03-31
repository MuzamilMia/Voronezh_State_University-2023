using System;
using System.IO;
using System.Threading;

namespace _2.Lab_OS_No12
{
    public class DirectoryScanner
    {
        private static int nextId = 1; // Статический счетчик для генерации уникальных ID сканеров
        private readonly Thread thread; // Поток для выполнения сканирования
        private readonly FileScanner fileScanner; // Сканер файлов для обработки
        private volatile bool isRunning = false; // Флаг выполнения 
        private volatile bool isPaused = false; // Флаг паузы 
        private readonly Action<string> logger; // Делегат для логирования
        private readonly Action<DirectoryScanner, FileScanner, string> onCompleted; // Делегат при завершении
        private readonly Action<string, string, string> updateWorkerStatus; // Делегат для обновления статуса
        public int Id { get; } // Уникальный идентификатор сканера

        public DirectoryScanner(FileScanner fileScanner, Action<string> logger,
                              Action<DirectoryScanner, FileScanner, string> onCompleted,
                              Action<string, string, string> updateWorkerStatus)
        {
            // Генерируем уникальный ID потокобезопасным способом
            Id = Interlocked.Increment(ref nextId);
            this.fileScanner = fileScanner;
            this.logger = logger ?? (_ => { }); // Если logger null, используем пустую функцию
            this.onCompleted = onCompleted;
            this.updateWorkerStatus = updateWorkerStatus;

            // Создаем и настраиваем поток для сканирования
            thread = new Thread(Run)
            {
                IsBackground = true, // Фоновый поток 
                Name = $"Scanner-{Id}" // Имя потока для отладки
            };
        }

        public void Start()
        {
            isRunning = true;
            thread.Start(); // Запускаем поток сканирования
            updateWorkerStatus?.Invoke(Id.ToString(), "Scanner", "Running"); // Обновляем статус
        }

        public void Pause()
        {
            isPaused = true;
            updateWorkerStatus?.Invoke(Id.ToString(), "Scanner", "Paused"); // Обновляем статус на "Пауза"
        }

        public void Resume()
        {
            isPaused = false;
            updateWorkerStatus?.Invoke(Id.ToString(), "Scanner", "Running"); // Обновляем статус на "Выполняется"
        }

        public void Stop()
        {
            isRunning = false;
            updateWorkerStatus?.Invoke(Id.ToString(), "Scanner", "Stopping"); // Обновляем статус на "Останавливается"
        }

        
        private void Run()
        {
            // Логируем начало сканирования
            logger?.Invoke($"Сканер #{Id} начал работу для директории: {fileScanner.DirectoryPath}");

            try
            {
                while (isRunning)
                {
           
                    if (isPaused)
                    {
                        Thread.Sleep(100); 
                        continue;
                    }

                    // Получаем следующий файл для обработки
                    string filePath = fileScanner.GetNextFile();
                    if (filePath == null)
                    {
                        break; 
                    }

                    try
                    {
                        // Обрабатываем файл
                        var fileInfo = new FileInfo(filePath);
                        fileScanner.ProcessFileResult(fileInfo); // Обновляем информацию о самом большом файле

                        // Логируем обработку файла
                        logger?.Invoke($"Сканер #{Id}: обработан '{Path.GetFileName(filePath)}' ({FormatFileSize(fileInfo.Length)})");

                        // Обновляем статус с прогрессом
                        updateWorkerStatus?.Invoke(Id.ToString(), "Scanner",
                            $"Обработано: {fileScanner.ProcessedFiles}/{fileScanner.TotalFiles}");
                    }
                    catch (Exception ex)
                    {
                        // Логируем ошибки обработки файла
                        logger?.Invoke($"Сканер #{Id}: ошибка при обработке '{filePath}': {ex.Message}");
                    }
                }

                // Ожидаем завершения обработки всех файлов
                fileScanner.WaitForCompletion();

                // Выводим информацию о самом большом файле
                if (fileScanner.LargestFile != null)
                {
                    logger?.Invoke($"Сканер #{Id}: Самый большой файл в '{fileScanner.DirectoryPath}' " +
                                 $"это '{fileScanner.LargestFile.Name}' ({FormatFileSize(fileScanner.LargestFile.Length)})");
                }
            }
            catch (Exception ex)
            {
                // Логируем критические ошибки
                logger?.Invoke($"Сканер #{Id} исключение: {ex}");
            }
            finally
            {
                // Завершающие действия независимо от результата
                logger?.Invoke($"Сканер #{Id} завершил работу.");
                updateWorkerStatus?.Invoke(Id.ToString(), "Scanner", "Completed"); // Финальный статус
                onCompleted?.Invoke(this, fileScanner, "Сканирование завершено"); // Уведомление о завершении
            }
        }

       
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" }; // Единицы измерения
            int order = 0;
            double len = bytes;

            // Конвертируем в соответствующие единицы измерения
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024; // Делим на 1024 для перехода к следующей единице
            }

            return $"{len:0.##} {sizes[order]}"; // Возвращаем отформатированную строку
        }
    }
}