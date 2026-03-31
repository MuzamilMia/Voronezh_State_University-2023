/// Подключаем модель планировщика
using FifoScheduler.Model;
/// Подключаем константы проекта
using FifoScheduler.Shared;
/// Подключаем интерфейс формы
using FifoScheduler.View;
/// Подключаем стандартные классы
using System;
/// Подключаем коллекции List, Queue
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FifoScheduler.Presenter
{
    public class MainPresenter
    {
        /// Планировщик процессов FIFO
        private readonly Scheduler _scheduler;
        /// Ссылка на главную форму
        private readonly Form1 _view;
        /// Блокировка для обновления UI (потокобезопасность)
        private readonly object _uiLock = new object();
        /// Флаг работы симуляции
        private bool _running = false;
        /// Поток планировщика (тикает каждые 1.2 сек)
        private Thread _simulationThread;
        /// Поток генератора новых процессов
        private Thread _processGeneratorThread;
        /// Список всех потоков процессов
        private readonly List<Thread> _processThreads = new List<Thread>();
        public MainPresenter(Form1 view)
        {
            /// Сохраняем ссылку на форму
            _view = view;
            /// Создаем планировщик
            _scheduler = new Scheduler();
            /// Подписываемся на кнопку Старт
            _view.StartClicked += (s, e) => Start();
            /// Подписываемся на кнопку Стоп
            _view.StopClicked += (s, e) => Stop();
        }

        /// Запуск симуляции
        public void Start()
        {
            /// Если уже запущено - выходим
            if (_running) return;
            /// Устанавливаем флаг работы
            _running = true;

            /// Создаем генератор процессов
            _processGeneratorThread = new Thread(ProcessGeneratorLoop) { IsBackground = true };
            /// Запускаем генератор
            _processGeneratorThread.Start();

            /// Создаем планировщик
            _simulationThread = new Thread(SimulationLoop) { IsBackground = true };
            /// Запускаем тики
            _simulationThread.Start();

            /// Обновляем интерфейс
            UpdateView();
        }

        /// Остановка симуляции
        public void Stop()
        {
            /// Останавливаем флаг
            _running = false;
            /// Для каждого потока процесса
            foreach (var t in _processThreads.ToList())
            {
                /// Ждем завершения (1 сек таймаут)
                if (t.IsAlive) t.Join(1000);
            }
            /// Очищаем список потоков
            _processThreads.Clear();
            /// Финальное обновление UI
            UpdateView();
        }

        /// Генератор новых процессов (каждые 3-5 сек)
        private void ProcessGeneratorLoop()
        {
            /// Генератор случайных чисел
            var rnd = new Random();
            /// Пока симуляция работает
            while (_running)
            {
                /// Если меньше максимума процессов
                if (_scheduler.AllProcesses.Count < Constants.Presenter.MaxProcesses)
                {
                    /// Создаем новый процесс
                    var process = _scheduler.CreateRandomProcess();
                    /// Запускаем его поток
                    CreateProcessThread(process);
                    /// Обновляем UI
                    UpdateView();
                }
                /// Пауза 3-5 сек
                Thread.Sleep(rnd.Next(Constants.Presenter.ProcessGenMin, Constants.Presenter.ProcessGenMax));
            }
        }

        /// Создает отдельный поток для процесса
        private void CreateProcessThread(Process process)
        {
            /// Создаем поток процесса
            var thread = new Thread(() => ProcessThreadLoop(process)) { IsBackground = true };
            /// Добавляем в список
            _processThreads.Add(thread);
            /// Запускаем
            thread.Start();
        }

        /// Основной цикл жизни процесса (каждый процесс в своем потоке)
        private void ProcessThreadLoop(Process process)
        {
            /// Случайность для каждого процесса
            var rnd = new Random(process.Id);
            /// Пока жив и симуляция работает
            while (_running && process.LifetimeRemaining > 0)
            {
                /// Если планировщик дал CPU
                if (process.HasCpu)
                {
                    /// Имитация работы процесса (1 секунда)
                    Thread.Sleep(1000);

                    /// 25% шанс БЛОКИРОВКИ - ПРОЦЕСС САМ РЕШАЕТ
                    if (rnd.NextDouble() < 0.25)
                    {
                        /// Просим планировщик заблокировать
                        _scheduler.RequestBlock(process);
                    }
                    else
                    {
                        /// Уменьшаем время жизни
                        process.LifetimeRemaining--;
                    }

                    /// Закончили квант - возвращаем CPU
                    _scheduler.ReturnCpu(process);
                }
                else
                {
                    /// Нет CPU - ждем 200мс
                    Thread.Sleep(200);
                }
            }
            /// Закончили жизнь процесса
            _scheduler.ProcessFinished(process);
        }

        /// Цикл планировщика (тикает каждые 1.2 сек)
        private void SimulationLoop()
        {
            /// Пока симуляция работает
            while (_running)
            {
                /// Пауза 1.2 секунды между тиками
                Thread.Sleep(1200);

                /// Вызываем тик планировщика
                _scheduler.Tick();

                /// Обновляем интерфейс
                UpdateView();
            }
        }

        /// Обновление интерфейса (потокобезопасно)
        private void UpdateView()
        {
            /// Блокируем UI обновление
            lock (_uiLock)
            {
                /// Копия очереди готовых
                var readyList = _scheduler.ReadyQueue.ToList();
                /// Копия очереди заблокированных
                var blockedList = _scheduler.BlockedQueue.ToList();
                /// Текущий процесс
                var running = _scheduler.CurrentRunningProcess;

                /// Обновляем левую колонку
                _view.UpdateReadyQueue(readyList.Select(p =>
                    $"{p.Name} (осталось: {p.LifetimeRemaining})").ToList());

                /// Обновляем правую колонку
                _view.UpdateBlockedQueue(blockedList.Select(p =>
                    $"{p.Name} (разблок: {p.BlockedUntil:HH:mm:ss})").ToList());

                /// Обновляем зеленый блок
                _view.UpdateRunningProcess(
                    running?.Name ?? "Пусто",
                    running?.LifetimeRemaining ?? 0);

                /// Статистика
                var stats = $"Процессов: {_scheduler.AllProcesses.Count} | Готовых: {readyList.Count} | Заблокировано: {blockedList.Count}";
                /// Обновляем статистику
                _view.UpdateStats(stats);
            }
        }
    }
}
