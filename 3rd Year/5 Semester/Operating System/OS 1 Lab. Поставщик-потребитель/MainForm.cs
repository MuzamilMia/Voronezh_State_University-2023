//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Threading;
//using System.Windows.Forms;

//public partial class MainForm : Form
//{
//    // Коллекции для управления потоками и буферами
//    private readonly List<Reader> readers = new List<Reader>();  // Список всех читателей
//    private readonly List<Writer> writers = new List<Writer>();  // Список всех писателей
//    private readonly List<BufferQueue> buffers = new List<BufferQueue>();  // Общий список всех буферов
//    private readonly object collectionsLock = new object();  // Объект для синхронизации доступа к коллекциям
//    private readonly Random rnd = new Random();  // Генератор случайных чисел для создания потоков
//    private bool isRunning = false;  // Флаг состояния работы приложения

//    public MainForm()
//    {
//        InitializeComponent();  // Инициализация компонентов формы (автосгенерированный метод)
//        InitializeDataGridView();  // Настройка DataGridView для отображения буферов
//        SetupEventHandlers();  // Подписка на события элементов управления
//    }

//    // Метод для настройки колонок DataGridView
//    private void InitializeDataGridView()
//    {
//        dgvBuffers.Columns.Clear();  // Очистка существующих колонок
//        // Добавление колонок для отображения информации о буферах
//        dgvBuffers.Columns.Add("Id", "Buffer Id");
//        dgvBuffers.Columns.Add("Count", "Count");
//        dgvBuffers.Columns.Add("Max", "MaxSize");
//        dgvBuffers.Columns.Add("IsFull", "IsFull");
//        dgvBuffers.Columns.Add("IsEmpty", "IsEmpty");
//    }

//    // Метод для подписки на события элементов управления
//    private void SetupEventHandlers()
//    {
//        btnStart.Click += BtnStart_Click;  // Обработчик кнопки Старт
//        btnPause.Click += BtnPause_Click;  // Обработчик кнопки Пауза
//        btnResume.Click += BtnResume_Click;  // Обработчик кнопки Возобновить
//        readerCreationTimer.Tick += ReaderCreationTimer_Tick;  // Обработчик таймера создания читателей
//        FormClosing += MainForm_FormClosing;  // Обработчик закрытия формы
//    }

//    // Обработчик нажатия кнопки Старт
//    private void BtnStart_Click(object sender, EventArgs e)
//    {
//        if (isRunning) return;  // Если уже работает, выходим
//        isRunning = true;  // Устанавливаем флаг работы

//        Log("Запуск системы с очередью, семафорами и мьютексами.");

//        // Создаем первого писателя
//        CreateWriter();

//        // Запускаем таймер создания читателей
//        readerCreationTimer.Start();

//        // Обновляем состояние кнопок
//        btnStart.Enabled = false;
//        btnPause.Enabled = true;
//        btnResume.Enabled = false;
//    }

//    // Обработчик нажатия кнопки Пауза
//    private void BtnPause_Click(object sender, EventArgs e)
//    {
//        if (!isRunning) return;  // Если не работает, выходим
//        Log("Приостановка всех потоков.");

//        // Блокируем коллекции для безопасного доступа
//        lock (collectionsLock)
//        {
//            foreach (var r in readers) r.Pause();  // Приостанавливаем всех читателей
//            foreach (var w in writers) w.Pause();  // Приостанавливаем всех писателей
//        }

//        // Обновляем состояние кнопок
//        btnPause.Enabled = false;
//        btnResume.Enabled = true;
//    }

//    // Обработчик нажатия кнопки Возобновить
//    private void BtnResume_Click(object sender, EventArgs e)
//    {
//        if (!isRunning) return;  // Если не работает, выходим
//        Log("Возобновление всех потоков.");

//        // Блокируем коллекции для безопасного доступа
//        lock (collectionsLock)
//        {
//            foreach (var r in readers) r.Resume();  // Возобновляем всех читателей
//            foreach (var w in writers) w.Resume();  // Возобновляем всех писателей
//        }

//        // Обновляем состояние кнопок
//        btnPause.Enabled = true;
//        btnResume.Enabled = false;
//    }

//    // Метод создания нового писателя
//    private void CreateWriter()
//    {
//        // Создаем буфер размером 5 элементов
//        BufferQueue buffer = new BufferQueue(5, UpdateBufferView);
//        // Создаем писателя для этого буфера
//        Writer writer = new Writer(buffer, Log, OnWriterStopped, UpdateWorkerStatus);

//        // Блокируем коллекции для безопасного добавления
//        lock (collectionsLock)
//        {
//            buffers.Add(buffer);  // Добавляем буфер в общий список
//            writers.Add(writer);  // Добавляем писателя в список
//        }

//        // Обновляем UI в потоке UI
//        InvokeIfRequired(() =>
//        {
//            // Добавляем информацию о буфере в DataGridView
//            dgvBuffers.Rows.Add(buffer.Id, buffer.Count, buffer.MaxSize, buffer.IsFull, buffer.IsEmpty);
//            // Создаем элемент ListView для писателя
//            ListViewItem item = new ListViewItem(new[] { writer.Id.ToString(), "Writer", "Running" });
//            item.Name = $"W{writer.Id}";  // Устанавливаем уникальное имя
//            lvWorkers.Items.Add(item);  // Добавляем в ListView
//        });

//        Log($"Создан писатель #{writer.Id} с буфером #{buffer.Id} (Max={buffer.MaxSize}).");
//        writer.Start();  // Запускаем поток писателя
//    }

//    // Метод создания нового читателя
//    private void CreateReader()
//    {
//        int dataLimit = (int)nudDataLimit.Value;  // Получаем лимит данных из NumericUpDown
//        // Создаем читателя с ссылкой на общий список буферов
//        Reader reader = new Reader(buffers, dataLimit, Log, UpdateWorkerStatus, UpdateBufferView);
//        reader.OnReaderCompleted += OnReaderCompleted;  // Подписываемся на событие завершения

//        // Блокируем коллекции для безопасного добавления
//        lock (collectionsLock)
//        {
//            readers.Add(reader);  // Добавляем читателя в список
//        }

//        // Обновляем UI в потоке UI
//        InvokeIfRequired(() =>
//        {
//            // Создаем элемент ListView для читателя
//            ListViewItem item = new ListViewItem(new[] { reader.Id.ToString(), "Reader", "Running" });
//            item.Name = $"R{reader.Id}";  // Устанавливаем уникальное имя
//            lvWorkers.Items.Add(item);  // Добавляем в ListView
//        });

//        Log($"Создан читатель #{reader.Id} (лимит: {dataLimit} данных).");
//        reader.Start();  // Запускаем поток читателя
//    }

//    // Обработчик таймера для создания новых потоков
//    private void ReaderCreationTimer_Tick(object sender, EventArgs e)
//    {
//        if (!isRunning) return;  // Если приложение не работает, выходим

//        // Случайное создание читателя с вероятностью 40%
//        if (rnd.NextDouble() < 0.4) // 40% шанс
//        {
//            CreateReader();
//        }

//        // Случайное создание писателя с вероятностью 30%
//        if (rnd.NextDouble() < 0.3) // 30% шанс
//        {
//            CreateWriter();
//        }
//    }

//    // Обработчик события завершения работы писателя
//    private void OnWriterStopped(Writer writer, BufferQueue buffer, string reason)
//    {
//        Log($"Писатель #{writer.Id} завершил работу. Причина: {reason}");

//        // Блокируем коллекции для безопасного удаления
//        lock (collectionsLock)
//        {
//            writers.Remove(writer);  // Удаляем писателя из списка
//            buffers.Remove(buffer);  // Удаляем буфер из списка
//        }

//        // Обновляем UI в потоке UI
//        InvokeIfRequired(() =>
//        {
//            // Удаляем писателя из ListView
//            if (lvWorkers.Items.ContainsKey($"W{writer.Id}"))
//                lvWorkers.Items.RemoveByKey($"W{writer.Id}");

//            // Удаляем буфер из DataGridView
//            for (int i = 0; i < dgvBuffers.Rows.Count; i++)
//            {
//                if (dgvBuffers.Rows[i].Cells[0].Value?.ToString() == buffer.Id.ToString())
//                {
//                    dgvBuffers.Rows.RemoveAt(i);
//                    break;
//                }
//            }
//        });
//    }

//    // Обработчик события завершения работы читателя
//    private void OnReaderCompleted(Reader reader)
//    {
//        Log($"Читатель #{reader.Id} завершил работу.");

//        // Блокируем коллекции для безопасного удаления
//        lock (collectionsLock)
//        {
//            readers.Remove(reader);  // Удаляем читателя из списка
//        }

//        // Обновляем UI в потоке UI
//        InvokeIfRequired(() =>
//        {
//            // Удаляем читателя из ListView
//            if (lvWorkers.Items.ContainsKey($"R{reader.Id}"))
//                lvWorkers.Items.RemoveByKey($"R{reader.Id}");
//        });
//    }

//    // Метод для обновления статуса рабочего потока в UI
//    private void UpdateWorkerStatus(string id, string role, string status)
//    {
//        InvokeIfRequired(() =>
//        {
//            // Формируем ключ для поиска элемента
//            string key = (role == "Writer" ? "W" : "R") + id;
//            ListViewItem item = lvWorkers.Items[key];  // Находим элемент по ключу
//            if (item != null)
//            {
//                item.SubItems[2].Text = status;  // Обновляем статус
//            }
//        });
//    }

//    // Метод для обновления отображения буфера в UI
//    private void UpdateBufferView(BufferQueue buffer)
//    {
//        InvokeIfRequired(() =>
//        {
//            // Ищем буфер в DataGridView
//            for (int i = 0; i < dgvBuffers.Rows.Count; i++)
//            {
//                if (dgvBuffers.Rows[i].Cells[0].Value?.ToString() == buffer.Id.ToString())
//                {
//                    // Обновляем данные буфера
//                    dgvBuffers.Rows[i].Cells[1].Value = buffer.Count;
//                    dgvBuffers.Rows[i].Cells[2].Value = buffer.MaxSize;
//                    dgvBuffers.Rows[i].Cells[3].Value = buffer.IsFull;
//                    dgvBuffers.Rows[i].Cells[4].Value = buffer.IsEmpty;
//                    return;
//                }
//            }
//            // Если буфер не найден, добавляем новую строку
//            dgvBuffers.Rows.Add(buffer.Id, buffer.Count, buffer.MaxSize, buffer.IsFull, buffer.IsEmpty);
//        });
//    }

//    // Метод для добавления сообщений в лог
//    private void Log(string text)
//    {
//        string line = $"[{DateTime.Now:HH:mm:ss}] {text}";  // Форматируем строку с временем
//        InvokeIfRequired(() =>
//        {
//            rtbLog.AppendText(line + Environment.NewLine);  // Добавляем текст в RichTextBox
//            rtbLog.ScrollToCaret();  // Прокручиваем к последней строке
//        });
//    }

//    // Обработчик закрытия формы
//    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
//    {
//        readerCreationTimer.Stop();  // Останавливаем таймер
//        isRunning = false;  // Сбрасываем флаг работы

//        // Блокируем коллекции для безопасного останова потоков
//        lock (collectionsLock)
//        {
//            foreach (var r in readers) r.Stop();  // Останавливаем всех читателей
//            foreach (var w in writers) w.Stop();  // Останавливаем всех писателей
//        }

//        // Короткая пауза для корректного завершения потоков
//        Thread.Sleep(300);
//    }

//    // Вспомогательный метод для безопасного вызова в UI потоке
//    private void InvokeIfRequired(Action action)
//    {
//        if (IsDisposed) return;  // Если форма уничтожена, выходим
//        if (InvokeRequired)  // Если вызов не из UI потока
//            BeginInvoke(action);  // Выполняем асинхронно в UI потоке
//        else
//            action();  // Выполняем непосредственно
//    }

//}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

public partial class MainForm : Form
{

    // Данные приложения - ОДИН писатель и ОДИН буфер
    private readonly List<Reader> readers = new List<Reader>();
    private Writer writer;
    private BufferQueue buffer;
    private readonly object collectionsLock = new object();
    private readonly Random rnd = new Random();
    private bool isRunning = false;

    public MainForm()
    {
        InitializeComponent();
        InitializeDataGridView();
        SetupEventHandlers();
    }

    private void InitializeDataGridView()
    {
        dgvBuffers.Columns.Clear();
        dgvBuffers.Columns.Add("Id", "Buffer Id");
        dgvBuffers.Columns.Add("Count", "Count");
        dgvBuffers.Columns.Add("Max", "MaxSize");
        dgvBuffers.Columns.Add("IsFull", "IsFull");
        dgvBuffers.Columns.Add("IsEmpty", "IsEmpty");
    }

    private void SetupEventHandlers()
    {
        btnStart.Click += BtnStart_Click;
        btnPause.Click += BtnPause_Click;
        btnResume.Click += BtnResume_Click;
        readerCreationTimer.Tick += ReaderCreationTimer_Tick;
        FormClosing += MainForm_FormClosing;
    }

    private void BtnStart_Click(object sender, EventArgs e)
    {
        if (isRunning) return;
        isRunning = true;

        Log("Запуск системы с ОДНИМ писателем и очередью.");

        // Создаем ТОЛЬКО ОДНОГО писателя
        CreateWriter();

        // Запускаем таймер создания читателей
        readerCreationTimer.Start();

        btnStart.Enabled = false;
        btnPause.Enabled = true;
        btnResume.Enabled = false;
    }

    private void BtnPause_Click(object sender, EventArgs e)
    {
        if (!isRunning) return;
        Log("Приостановка всех потоков.");

        lock (collectionsLock)
        {
            foreach (var r in readers) r.Pause();
            writer?.Pause();
        }

        btnPause.Enabled = false;
        btnResume.Enabled = true;
    }

    private void BtnResume_Click(object sender, EventArgs e)
    {
        if (!isRunning) return;
        Log("Возобновление всех потоков.");

        lock (collectionsLock)
        {
            foreach (var r in readers) r.Resume();
            writer?.Resume();
        }

        btnPause.Enabled = true;
        btnResume.Enabled = false;
    }

    private void CreateWriter()
    {
        // Создаем ОДИН буфер если его еще нет
        if (buffer == null)
        {
            buffer = new BufferQueue(5, UpdateBufferView);

            InvokeIfRequired(() =>
            {
                dgvBuffers.Rows.Add(buffer.Id, buffer.Count, buffer.MaxSize, buffer.IsFull, buffer.IsEmpty);
            });
        }

        // Создаем ОДНОГО писателя
        writer = new Writer(buffer, Log, OnWriterStopped, UpdateWorkerStatus);

        InvokeIfRequired(() =>
        {
            ListViewItem item = new ListViewItem(new[] { writer.Id.ToString(), "Writer", "Running" });
            item.Name = $"W{writer.Id}";
            lvWorkers.Items.Add(item);
        });

        Log($"Создан писатель #{writer.Id} с буфером #{buffer.Id} (Max={buffer.MaxSize}).");
        writer.Start();
    }

    private void CreateReader()
    {
        int dataLimit = (int)nudDataLimit.Value;

        //  Все читатели работают с ОДНИМ буфером
        Reader reader = new Reader(buffer, dataLimit, Log, UpdateWorkerStatus, UpdateBufferView);
        reader.OnReaderCompleted += OnReaderCompleted;

        lock (collectionsLock)
        {
            readers.Add(reader);
        }

        InvokeIfRequired(() =>
        {
            ListViewItem item = new ListViewItem(new[] { reader.Id.ToString(), "Reader", "Running" });
            item.Name = $"R{reader.Id}";
            lvWorkers.Items.Add(item);
        });

        Log($"Создан читатель #{reader.Id} (лимит: {dataLimit} данных).");
        reader.Start();
    }

    //  Таймер создает ТОЛЬКО читателей
    private void ReaderCreationTimer_Tick(object sender, EventArgs e)
    {
        if (!isRunning) return;

        // Создаем только читателей
        if (rnd.NextDouble() < 0.4)
        {
            CreateReader();
        }
        // НЕТ создания писателей!
    }

    private void OnWriterStopped(Writer writer, BufferQueue buffer, string reason)
    {
        Log($"Писатель #{writer.Id} завершил работу. Причина: {reason}");

        lock (collectionsLock)
        {
            this.writer = null;
        }

        InvokeIfRequired(() =>
        {
            if (lvWorkers.Items.ContainsKey($"W{writer.Id}"))
                lvWorkers.Items.RemoveByKey($"W{writer.Id}");
        });
    }

    private void OnReaderCompleted(Reader reader)
    {
        Log($"Читатель #{reader.Id} завершил работу.");

        lock (collectionsLock)
        {
            readers.Remove(reader);
        }

        InvokeIfRequired(() =>
        {
            if (lvWorkers.Items.ContainsKey($"R{reader.Id}"))
                lvWorkers.Items.RemoveByKey($"R{reader.Id}");
        });
    }

    private void UpdateWorkerStatus(string id, string role, string status)
    {
        InvokeIfRequired(() =>
        {
            string key = (role == "Writer" ? "W" : "R") + id;
            if (lvWorkers.Items.ContainsKey(key))
            {
                var item = lvWorkers.Items[key];
                item.SubItems[2].Text = status;
            }
        });
    }

    private void UpdateBufferView(BufferQueue buffer)
    {
        InvokeIfRequired(() =>
        {
            for (int i = 0; i < dgvBuffers.Rows.Count; i++)
            {
                if (dgvBuffers.Rows[i].Cells[0].Value?.ToString() == buffer.Id.ToString())
                {
                    dgvBuffers.Rows[i].Cells[1].Value = buffer.Count;
                    dgvBuffers.Rows[i].Cells[2].Value = buffer.MaxSize;
                    dgvBuffers.Rows[i].Cells[3].Value = buffer.IsFull;
                    dgvBuffers.Rows[i].Cells[4].Value = buffer.IsEmpty;
                    return;
                }
            }
        });
    }

    private void Log(string text)
    {
        string line = $"[{DateTime.Now:HH:mm:ss}] {text}";
        InvokeIfRequired(() =>
        {
            rtbLog.AppendText(line + Environment.NewLine);
            rtbLog.ScrollToCaret();
        });
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        readerCreationTimer.Stop();
        isRunning = false;

        lock (collectionsLock)
        {
            foreach (var r in readers) r.Stop();
            writer?.Stop();
        }

        Thread.Sleep(300);
    }

    private void InvokeIfRequired(Action action)
    {
        if (IsDisposed) return;
        if (InvokeRequired)
            BeginInvoke(action);
        else
            action();
    }
}