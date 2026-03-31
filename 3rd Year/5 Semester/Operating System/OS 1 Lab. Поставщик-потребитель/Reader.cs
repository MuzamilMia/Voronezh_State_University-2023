using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

//Создать многопоточное приложение с одним потоком - писателем, который в случайные моменты
//времени помещает данные в буфер и сообщает об этом. Главный поток в случайные моменты времени
//порождает потоки - читатели, которые в случайные моменты времени удаляют данные из буфера с
//соответствующим сообщением. Каждый поток – читатель завершается после удаления заданного  числа данных.
//Все читатели и писатели используют один и тот же буфер.
//Will here is the question that we are using only one thered so what is the need of the id however we are using only one thread.
//and the next thing is this that we have only using the queue and why we are using the list. and for adding the elements
//we are using the queue and queue is adding in sorted so why do we need for seraching the empty places.var nonEmptyBuffers = buffers.Where(b => !b.IsEmpty).ToList();
// Reader.cs (множество читателей, каждый с лимитом данных)
//public class Reader
//{
//    private static int nextId = 1;  // Статическая переменная для генерации уникальных ID читателей
//    private readonly Thread thread;  // Поток, в котором работает читатель
//    private readonly List<BufferQueue> buffers; // Ссылка на общий список буферов (а не копия!)
//    private readonly object buffersLock = new object();  // Объект для синхронизации доступа к списку буферов
//    private volatile bool isRunning = false;  // Флаг выполнения потока (volatile для многопоточного доступа)
//    private volatile bool isPaused = false;   // Флаг приостановки потока
//    private readonly Action<string> logger;  // Делегат для логирования сообщений
//    private readonly Action<string, string, string> updateWorkerStatus;  // Делегат для обновления статуса в UI
//    private readonly Action<BufferQueue> updateBufferView;  // Делегат для обновления отображения буфера в UI
//    public int Id { get; }  // Уникальный идентификатор читателя

//    private readonly Random rnd = new Random();  // Генератор случайных чисел для задержек
//    private readonly int dataLimit; // Количество данных для чтения перед завершением
//    private int itemsRead = 0;  // Счетчик прочитанных элементов

//    // Конструктор получает ссылку на общий список буферов
//    public Reader(List<BufferQueue> buffersRef, int dataLimit, Action<string> logger,
//                 Action<string, string, string> updateWorkerStatus, Action<BufferQueue> updateBufferView)
//    {
//        // Атомарное увеличение счетчика для генерации уникального ID
//        Id = Interlocked.Increment(ref nextId);
//        this.buffers = buffersRef; // Получаем ссылку на общий список (требование друга выполнено!)
//        this.dataLimit = dataLimit;
//        this.logger = logger ?? (_ => { });  // Если logger null, используем пустую лямбду
//        this.updateWorkerStatus = updateWorkerStatus;
//        this.updateBufferView = updateBufferView;
//        // Создаем поток с указанием метода Run и настройками
//        thread = new Thread(Run) { IsBackground = true, Name = $"Reader-{Id}" };
//    }

//    // Метод запуска потока читателя
//    public void Start()
//    {
//        isRunning = true;  // Устанавливаем флаг работы
//        thread.Start();  // Запускаем поток
//        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", "Running");  // Обновляем статус в UI
//    }

//    // Метод приостановки работы читателя
//    public void Pause()
//    {
//        isPaused = true;  // Устанавливаем флаг паузы
//        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", "Paused");  // Обновляем статус в UI
//    }

//    // Метод возобновления работы читателя
//    public void Resume()
//    {
//        isPaused = false;  // Сбрасываем флаг паузы
//        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", "Running");  // Обновляем статус в UI
//    }

//    // Метод остановки читателя
//    public void Stop()
//    {
//        isRunning = false;  // Сбрасываем флаг работы
//        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", "Stopping");  // Обновляем статус в UI
//    }

//    // Основной метод работы потока читателя
//    private void Run()
//    {
//        logger?.Invoke($"Reader #{Id} started. Will read {dataLimit} items.");

//        try
//        {
//            // Главный цикл работы - пока работает и не достигнут лимит чтения
//            while (isRunning && itemsRead < dataLimit)
//            {
//                // Проверка на паузу
//                if (isPaused)
//                {
//                    Thread.Sleep(100);  // Короткая пауза при приостановке
//                    continue;  // Переход к следующей итерации цикла
//                }

//                BufferQueue targetBuffer = null;
//                // Блокировка для безопасного доступа к списку буферов
//                lock (buffers)
//                {
//                    // Выбираем случайный непустой буфер
//                    var nonEmptyBuffers = buffers.Where(b => !b.IsEmpty).ToList();
//                    if (nonEmptyBuffers.Count > 0)
//                    {
//                        // Случайный выбор буфера из непустых
//                        targetBuffer = nonEmptyBuffers[rnd.Next(nonEmptyBuffers.Count)];
//                    }
//                }

//                if (targetBuffer != null)
//                {
//                    // Извлечение элемента из буфера
//                    object item = targetBuffer.Take();
//                    if (item != null)
//                    {
//                        itemsRead++;  // Увеличиваем счетчик прочитанных элементов
//                        logger?.Invoke($"Reader #{Id}: взял '{item}' из буфера #{targetBuffer.Id} ({itemsRead}/{dataLimit})");
//                        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", $"Read: {itemsRead}/{dataLimit}");
//                        updateBufferView?.Invoke(targetBuffer);  // Обновляем отображение буфера

//                        // Имитация обработки данных случайной задержкой
//                        Thread.Sleep(rnd.Next(50, 200));
//                    }
//                }
//                else
//                {
//                    // Все буферы пусты - ждем перед следующей попыткой
//                    Thread.Sleep(100);
//                }
//            }

//            // Логирование успешного завершения работы
//            logger?.Invoke($"Reader #{Id} completed. Read {itemsRead} items.");
//        }
//        catch (ThreadInterruptedException)
//        {
//            // Ожидаемое прерывание - нормальное завершение при остановке
//        }
//        catch (Exception ex)
//        {
//            // Логирование непредвиденных исключений
//            logger?.Invoke($"Reader #{Id} exception: {ex}");
//        }
//        finally
//        {
//            // Блок finally гарантирует выполнение даже при исключениях
//            logger?.Invoke($"Reader #{Id} finished.");
//            updateWorkerStatus?.Invoke(Id.ToString(), "Reader", $"Stopped (read {itemsRead})");

//            // Автоматически удаляем себя из системы при завершении
//            OnReaderCompleted?.Invoke(this);  // Уведомление главной формы о завершении
//        }
//    }

//    // Событие для уведомления о завершении работы читателя
//    public event Action<Reader> OnReaderCompleted;
//}

using System.Threading;
using System;

// Reader.cs для работы с ОДНИМ буфером
using System;
using System.Threading;

public class Reader
{
    private static int nextId = 1;
    private readonly Thread thread;
    private readonly BufferQueue buffer; // ОДИН буфер для всех читателей
    private volatile bool isRunning = false;
    private volatile bool isPaused = false;
    private readonly Action<string> logger;
    private readonly Action<string, string, string> updateWorkerStatus;
    private readonly Action<BufferQueue> updateBufferView;
    public int Id { get; }
    private readonly Random rnd = new Random();
    private readonly int dataLimit;
    private int itemsRead = 0;

    public Reader(BufferQueue buffer, int dataLimit, Action<string> logger,
                 Action<string, string, string> updateWorkerStatus, Action<BufferQueue> updateBufferView)
    {
        Id = Interlocked.Increment(ref nextId);
        this.buffer = buffer; // Все читатели работают с ОДНИМ буфером
        this.dataLimit = dataLimit;
        this.logger = logger ?? (_ => { });
        this.updateWorkerStatus = updateWorkerStatus;
        this.updateBufferView = updateBufferView;
        thread = new Thread(Run) { IsBackground = true, Name = $"Reader-{Id}" };
    }

    public void Start()
    {
        isRunning = true;
        thread.Start();
        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", "Running");
    }

    public void Pause()
    {
        isPaused = true;
        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", "Paused");
    }

    public void Resume()
    {
        isPaused = false;
        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", "Running");
    }

    public void Stop()
    {
        isRunning = false;
        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", "Stopping");
    }

    private void Run()
    {
        logger?.Invoke($"Reader #{Id} started. Will read {dataLimit} items.");

        try
        {
            while (isRunning && itemsRead < dataLimit)
            {
                if (isPaused)
                {
                    Thread.Sleep(100);
                    continue;
                }

                // Простая проверка - если буфер не пуст, берем данные
                if (!buffer.IsEmpty)
                {
                    object item = buffer.Take();
                    if (item != null)
                    {
                        itemsRead++;
                        logger?.Invoke($"Reader #{Id}: взял '{item}' из буфера #{buffer.Id} ({itemsRead}/{dataLimit})");
                        updateWorkerStatus?.Invoke(Id.ToString(), "Reader", $"Read: {itemsRead}/{dataLimit}");
                        updateBufferView?.Invoke(buffer);
                        Thread.Sleep(rnd.Next(50, 200));
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            logger?.Invoke($"Reader #{Id} completed. Read {itemsRead} items.");
        }
        catch (ThreadInterruptedException) { }
        catch (Exception ex)
        {
            logger?.Invoke($"Reader #{Id} exception: {ex}");
        }
        finally
        {
            logger?.Invoke($"Reader #{Id} finished.");
            updateWorkerStatus?.Invoke(Id.ToString(), "Reader", $"Stopped (read {itemsRead})");
            OnReaderCompleted?.Invoke(this);
        }
    }

    public event Action<Reader> OnReaderCompleted;
}