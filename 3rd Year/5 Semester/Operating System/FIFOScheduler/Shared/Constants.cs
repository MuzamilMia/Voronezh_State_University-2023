using System;

namespace FifoScheduler.Shared
{
    public static class Constants
    {
        public static class Model
        {
            /// Минимальное время жизни процесса (в квантах времени)
            public const int LifetimeMin = 8;
            /// Максимальное время жизни процесса (в квантах времени)
            public const int LifetimeMax = 25;
            /// Минимальное время блокировки процесса (в миллисекундах)
            public const int BlockMin = 3000;
            /// Максимальное время блокировки процесса (в миллисекундах)
            public const int BlockMax = 10000;
        }

        public static class Presenter
        {
            /// Максимальное количество одновременно существующих процессов
            public const int MaxProcesses = 12;
            /// Интервал между тактами симуляции (в миллисекундах)
            public const int TickInterval = 1200;
            /// Минимальный интервал генерации новых процессов (в миллисекундах)
            public const int ProcessGenMin = 4000;
            /// Максимальный интервал генерации новых процессов (в миллисекундах)
            public const int ProcessGenMax = 7000;
        }
    }
}