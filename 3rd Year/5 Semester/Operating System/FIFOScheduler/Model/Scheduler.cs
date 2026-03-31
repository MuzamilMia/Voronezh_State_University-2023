using System;                                
using System.Collections.Generic;            
using System.Linq;                           
using FifoScheduler.Shared;                 

namespace FifoScheduler.Model               
{
    public class Scheduler                  
    {
        /// Объект-блокировка для потоков
        private readonly object _lock = new object();
        /// Генератор случайных чисел
        private readonly Random _random = new Random();

        /// Все активные процессы
        public List<Process> AllProcesses { get; } = new List<Process>();
        /// Очередь Готовых (FIFO)
        public Queue<Process> ReadyQueue { get; } = new Queue<Process>();
        /// Очередь Заблокированных
        public Queue<Process> BlockedQueue { get; } = new Queue<Process>();
        /// Сейчас бегущий процесс
        public Process CurrentRunningProcess { get; private set; }
        /// Счетчик ID процессов
        private int _nextProcessId = 1;

        /// Создать новый процесс
        public Process CreateRandomProcess()
        {
            /// Блокируем доступ другим потокам
            lock (_lock)
            {
                /// Создаем объект процесса
                var process = new Process
                {
                    /// Даем уникальный ID
                    Id = _nextProcessId++,
                    /// Имя P1, P2, P3...
                    Name = $"P{_nextProcessId - 1}",
                    /// Случайное время жизни
                    LifetimeRemaining = _random.Next(Constants.Model.LifetimeMin, Constants.Model.LifetimeMax + 1)
                };

                /// Добавляем во все процессы
                AllProcesses.Add(process);
                /// В ОЧЕРЕДЬ ГОТОВЫХ (первая!)
                ReadyQueue.Enqueue(process);
                /// Возвращаем процесс
                return process;
            }
        }

        /// ТИК планировщика (каждые 1.2 сек)
        public void Tick()
        {
            /// Блокируем доступ
            lock (_lock)
            {
                /// Проверяем разблокировки
                HandleUnblockedProcesses();

                /// Если нет бегущего И есть готовые
                if (CurrentRunningProcess == null && ReadyQueue.Count > 0)
                {
                    /// Берем ПЕРВЫЙ из очереди FIFO
                    CurrentRunningProcess = ReadyQueue.Dequeue();
                    /// Меняем состояние на Running
                    CurrentRunningProcess.State = ProcessState.Running;
                    /// Даем CPU
                    CurrentRunningProcess.HasCpu = true;
                }
            }
        }

        /// Процесс САМ просит блокировку
        public void RequestBlock(Process process)
        {
            /// Блокируем доступ
            lock (_lock)
            {
                /// Меняем состояние на Blocked
                process.State = ProcessState.Blocked;
                /// Устанавливаем время разблокировки
                process.BlockedUntil = DateTime.Now.AddMilliseconds(
                    /// Случайное время блокировки
                    _random.Next(Constants.Model.BlockMin, Constants.Model.BlockMax));
                /// В ОЧЕРЕДЬ ЗАБЛОКИРОВАННЫХ
                BlockedQueue.Enqueue(process);
            }
        }

        /// Процесс завершился
        public void ProcessFinished(Process process)
        {
            /// Блокируем доступ
            lock (_lock)
            {
                /// Отмечаем как завершенный
                process.State = ProcessState.Finished;
                /// Удаляем из всех процессов
                AllProcesses.RemoveAll(p => p.Id == process.Id);
            }
        }

        /// Процесс возвращает CPU
        public void ReturnCpu(Process process)
        {
            /// Блокируем доступ
            lock (_lock)
            {
                /// Если это текущий процесс
                if (process == CurrentRunningProcess)
                {
                    /// Забираем CPU
                    process.HasCpu = false;
                    /// Возвращаем в Ready
                    process.State = ProcessState.Ready;
                    /// В КОНЕЦ очереди FIFO
                    ReadyQueue.Enqueue(process);
                    /// Очищаем текущий
                    CurrentRunningProcess = null;
                }
            }
        }

        /// Обработка разблокировки
        private void HandleUnblockedProcesses()
        {
            /// Текущее время
            var now = DateTime.Now;
            /// Копия очереди заблокированных
            var tempBlocked = new Queue<Process>(BlockedQueue);
            /// Очищаем оригинал
            BlockedQueue.Clear();

            /// Перебираем все заблокированные
            while (tempBlocked.Count > 0)
            {
                /// Берем следующий
                var proc = tempBlocked.Dequeue();
                /// Время блокировки истекло?
                if (proc.BlockedUntil.HasValue && now >= proc.BlockedUntil.Value)
                {
                    /// Разблокируем
                    proc.State = ProcessState.Ready;
                    /// В ОЧЕРЕДЬ ГОТОВЫХ
                    ReadyQueue.Enqueue(proc);
                }
                else
                {
                    /// Оставляем заблокированным
                    BlockedQueue.Enqueue(proc);
                }
            }
        }
    }
}
