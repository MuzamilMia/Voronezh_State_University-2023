//using System;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;

//namespace ProcessThreadScanner
//{
//    public class ProcessThreadScanner
//    {
//        [Flags]
//        internal enum SnapshotFlags : uint
//        {
//            HeapList = 0x00000001,
//            Process = 0x00000002,
//            Thread = 0x00000004,
//            Module = 0x00000008,
//            Module32 = 0x00000010,
//            Inherit = 0x80000000,
//            All = 0x0000001F,
//            NoHeaps = 0x40000000
//        }

//        [StructLayout(LayoutKind.Sequential)]
//        public struct PROCESSENTRY32
//        {
//            public uint dwSize;
//            public uint cntUsage;
//            public uint th32ProcessID;
//            public IntPtr th32DefaultHeapID;
//            public uint th32ModuleID;
//            public uint cntThreads;
//            public uint th32ParentProcessID;
//            public int pcPriClassBase;
//            public uint dwFlags;
//            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
//            public string szExeFile;
//        }

//        [StructLayout(LayoutKind.Sequential)]
//        public struct THREADENTRY32
//        {
//            public uint dwSize;
//            public uint cntUsage;
//            public uint th32ThreadID;
//            public uint th32OwnerProcessID;
//            public int tpBasePri;
//            public int tpDeltaPri;
//            public uint dwFlags;
//        }

//        [DllImport("kernel32.dll", SetLastError = true)]
//        static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

//        [DllImport("kernel32.dll", SetLastError = true)]
//        static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

//        [DllImport("kernel32.dll", SetLastError = true)]
//        static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

//        [DllImport("kernel32.dll", SetLastError = true)]
//        static extern bool Thread32First(IntPtr hSnapshot, ref THREADENTRY32 lpte);

//        [DllImport("kernel32.dll", SetLastError = true)]
//        static extern bool Thread32Next(IntPtr hSnapshot, ref THREADENTRY32 lpte);

//        [DllImport("kernel32.dll", SetLastError = true)]
//        static extern bool CloseHandle(IntPtr hObject);

//        public class ProcessInfo
//        {
//            public uint ProcessID { get; set; }
//            public string ProcessName { get; set; }
//            public uint ThreadCount { get; set; }
//            public uint ParentPID { get; set; }
//            public int Priority { get; set; }
//        }

//        public class ThreadInfo
//        {
//            public uint ThreadID { get; set; }
//            public uint ProcessID { get; set; }
//            public int BasePriority { get; set; }
//            public int DeltaPriority { get; set; }
//        }

//        public static List<ProcessInfo> GetProcessesWithThreadCount(uint targetThreadCount)
//        {
//            var processes = new List<ProcessInfo>();
//            IntPtr snapshot = IntPtr.Zero;

//            try
//            {
//                snapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Process, 0);

//                if (snapshot == IntPtr.Zero || snapshot == new IntPtr(-1))
//                {
//                    throw new ApplicationException($"Failed to create snapshot. Error code: {Marshal.GetLastWin32Error()}");
//                }

//                PROCESSENTRY32 procEntry = new PROCESSENTRY32();
//                procEntry.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

//                if (Process32First(snapshot, ref procEntry))
//                {
//                    do
//                    {
//                        if (procEntry.cntThreads == targetThreadCount)
//                        {
//                            processes.Add(new ProcessInfo
//                            {
//                                ProcessID = procEntry.th32ProcessID,
//                                ProcessName = procEntry.szExeFile,
//                                ThreadCount = procEntry.cntThreads,
//                                ParentPID = procEntry.th32ParentProcessID,
//                                Priority = procEntry.pcPriClassBase
//                            });
//                        }
//                    }
//                    while (Process32Next(snapshot, ref procEntry));
//                }
//                else
//                {
//                    throw new ApplicationException($"Failed to get first process. Error code: {Marshal.GetLastWin32Error()}");
//                }
//            }
//            finally
//            {
//                if (snapshot != IntPtr.Zero && snapshot != new IntPtr(-1))
//                {
//                    CloseHandle(snapshot);
//                }
//            }

//            return processes;
//        }

//        public static List<ThreadInfo> GetThreadsForProcess(uint processId)
//        {
//            var threads = new List<ThreadInfo>();
//            IntPtr snapshot = IntPtr.Zero;

//            try
//            {
//                snapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Thread, 0);

//                if (snapshot == IntPtr.Zero || snapshot == new IntPtr(-1))
//                {
//                    throw new ApplicationException($"Failed to create thread snapshot. Error code: {Marshal.GetLastWin32Error()}");
//                }

//                THREADENTRY32 threadEntry = new THREADENTRY32();
//                threadEntry.dwSize = (uint)Marshal.SizeOf(typeof(THREADENTRY32));

//                if (Thread32First(snapshot, ref threadEntry))
//                {
//                    do
//                    {
//                        if (threadEntry.th32OwnerProcessID == processId)
//                        {
//                            threads.Add(new ThreadInfo
//                            {
//                                ThreadID = threadEntry.th32ThreadID,
//                                ProcessID = threadEntry.th32OwnerProcessID,
//                                BasePriority = threadEntry.tpBasePri,
//                                DeltaPriority = threadEntry.tpDeltaPri
//                            });
//                        }
//                    }
//                    while (Thread32Next(snapshot, ref threadEntry));
//                }
//                else
//                {
//                    throw new ApplicationException($"Failed to get first thread. Error code: {Marshal.GetLastWin32Error()}");
//                }
//            }
//            finally
//            {
//                if (snapshot != IntPtr.Zero && snapshot != new IntPtr(-1))
//                {
//                    CloseHandle(snapshot);
//                }
//            }

//            return threads;
//        }

//        public static Dictionary<uint, List<ThreadInfo>> GetAllProcessesWithThreads(uint targetThreadCount)
//        {
//            var result = new Dictionary<uint, List<ThreadInfo>>();

//            // Get processes with target thread count
//            var processes = GetProcessesWithThreadCount(targetThreadCount);

//            // Get threads for each process
//            foreach (var process in processes)
//            {
//                var threads = GetThreadsForProcess(process.ProcessID);
//                result[process.ProcessID] = threads;
//            }

//            return result;
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//Для каждого процесса, имеющего заданное число потоков, вывести идентификаторы этих потоков.
namespace ProcessThreadScanner
{
    public class ProcessThreadScanner
    {
        // Флаги для создания снимка системы - определяют, какую информацию включать в снимок
        [Flags]
        internal enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,      // Включить информацию о кучах процессов
            Process = 0x00000002,       // Включить информацию о процессах
            Thread = 0x00000004,        // Включить информацию о потоках
            Module = 0x00000008,        // Включить информацию о модулях
            Module32 = 0x00000010,      // Включить 32-битные модули
            Inherit = 0x80000000,       // Дескриптор снимка будет наследуемым
            All = 0x0000001F,           // Включить всю информацию (процессы, потоки, модули, кучи)
            NoHeaps = 0x40000000        // Исключить информацию о кучах
        }

        // Структура для хранения информации о процессе из WinAPI
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESSENTRY32
        {
            public uint dwSize;              // Размер структуры (должен быть инициализирован перед использованием)
            public uint cntUsage;            // Счетчик ссылок на процесс
            public uint th32ProcessID;       // Идентификатор процесса (PID)
            public IntPtr th32DefaultHeapID; // ID кучи по умолчанию (только для ToolHelp32)
            public uint th32ModuleID;        // ID модуля (только для ToolHelp32)
            public uint cntThreads;          // Количество потоков в процессе
            public uint th32ParentProcessID; // Идентификатор родительского процесса
            public int pcPriClassBase;       // Базовый приоритет процесса
            public uint dwFlags;             // Зарезервировано
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;         // Имя исполняемого файла процесса
        }

        // Структура для хранения информации о потоке из WinAPI
        [StructLayout(LayoutKind.Sequential)]
        public struct THREADENTRY32
        {
            public uint dwSize;              // Размер структуры
            public uint cntUsage;            // Счетчик ссылок на поток
            public uint th32ThreadID;        // Идентификатор потока (TID)
            public uint th32OwnerProcessID;  // Идентификатор процесса-владельца
            public int tpBasePri;           // Базовый приоритет потока
            public int tpDeltaPri;          // Дельта-приоритет (относительное смещение)
            public uint dwFlags;             // Зарезервировано
        }

        // Импорт функций из Windows API (kernel32.dll)

        // Создает снимок указанных процессов, потоков, модулей или куч
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        // Извлекает информацию о первом процессе в снимке
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        // Извлекает информацию о следующем процессе в снимке
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        // Извлекает информацию о первом потоке в снимке
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Thread32First(IntPtr hSnapshot, ref THREADENTRY32 lpte);

        // Извлекает информацию о следующем потоке в снимке
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Thread32Next(IntPtr hSnapshot, ref THREADENTRY32 lpte);

        // Закрывает дескриптор снимка и освобождает ресурсы
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        // Класс для хранения информации о процессе в удобном формате
        public class ProcessInfo
        {
            public uint ProcessID { get; set; }     // Идентификатор процесса
            public string ProcessName { get; set; } // Имя процесса (EXE файл)
            public uint ThreadCount { get; set; }   // Количество потоков
            public uint ParentPID { get; set; }     // Идентификатор родительского процесса
            public int Priority { get; set; }       // Базовый приоритет
        }

        // Класс для хранения информации о потоке в удобном формате
        public class ThreadInfo
        {
            public uint ThreadID { get; set; }      // Идентификатор потока
            public uint ProcessID { get; set; }     // Идентификатор процесса-владельца
            public int BasePriority { get; set; }   // Базовый приоритет потока
            public int DeltaPriority { get; set; }  // Дельта-приоритет
        }

        // Основной метод: получает список процессов с заданным количеством потоков
        public static List<ProcessInfo> GetProcessesWithThreadCount(uint targetThreadCount)
        {
            var processes = new List<ProcessInfo>(); // Список для хранения результатов
            IntPtr snapshot = IntPtr.Zero;           // Дескриптор снимка системы

            try
            {
                // Создаем снимок всех процессов в системе
                snapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Process, 0);

                // Проверяем успешность создания снимка
                if (snapshot == IntPtr.Zero || snapshot == new IntPtr(-1))
                {
                    throw new ApplicationException($"Не удалось создать снимок системы. Код ошибки: {Marshal.GetLastWin32Error()}");
                }

                // Инициализируем структуру для хранения информации о процессе
                PROCESSENTRY32 procEntry = new PROCESSENTRY32();
                procEntry.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32)); // Важно: установить размер структуры

                // Начинаем перечисление процессов с первого в снимке
                if (Process32First(snapshot, ref procEntry))
                {
                    do
                    {
                        // Проверяем, соответствует ли процесс нашему критерию (количество потоков)
                        if (procEntry.cntThreads == targetThreadCount)
                        {
                            // Добавляем процесс в результат
                            processes.Add(new ProcessInfo
                            {
                                ProcessID = procEntry.th32ProcessID,
                                ProcessName = procEntry.szExeFile,
                                ThreadCount = procEntry.cntThreads,
                                ParentPID = procEntry.th32ParentProcessID,
                                Priority = procEntry.pcPriClassBase
                            });
                        }
                    }
                    while (Process32Next(snapshot, ref procEntry)); // Переходим к следующему процессу
                }
                else
                {
                    throw new ApplicationException($"Не удалось получить первый процесс. Код ошибки: {Marshal.GetLastWin32Error()}");
                }
            }
            finally
            {
                // Важно: всегда освобождаем дескриптор снимка
                if (snapshot != IntPtr.Zero && snapshot != new IntPtr(-1))
                {
                    CloseHandle(snapshot);
                }
            }

            return processes; // Возвращаем найденные процессы
        }

        // Метод для получения всех потоков указанного процесса
        public static List<ThreadInfo> GetThreadsForProcess(uint processId)
        {
            var threads = new List<ThreadInfo>(); // Список для хранения потоков
            IntPtr snapshot = IntPtr.Zero;        // Дескриптор снимка

            try
            {
                // СОЗДАЕМ СНИМОК ТОЛЬКО ДЛЯ КОНКРЕТНОГО ПРОЦЕССА
                snapshot = CreateToolhelp32Snapshot((uint)SnapshotFlags.Thread, processId);

                if (snapshot == IntPtr.Zero || snapshot == new IntPtr(-1))
                {
                    throw new ApplicationException($"Не удалось создать снимок потоков для процесса {processId}. Код ошибки: {Marshal.GetLastWin32Error()}");
                }

                // Инициализируем структуру для хранения информации о потоке
                THREADENTRY32 threadEntry = new THREADENTRY32();
                threadEntry.dwSize = (uint)Marshal.SizeOf(typeof(THREADENTRY32)); // Устанавливаем размер

                // Начинаем перечисление потоков
                if (Thread32First(snapshot, ref threadEntry))
                {
                    do
                    {
                        // Добавляем поток в результат
                        threads.Add(new ThreadInfo
                        {
                            ThreadID = threadEntry.th32ThreadID,
                            ProcessID = threadEntry.th32OwnerProcessID,
                            BasePriority = threadEntry.tpBasePri,
                            DeltaPriority = threadEntry.tpDeltaPri
                        });
                    }
                    while (Thread32Next(snapshot, ref threadEntry)); // Переходим к следующему потоку
                }
                else
                {
                    // Если процесс не имеет потоков или произошла ошибка
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode != 0) // Если есть реальная ошибка, а не просто отсутствие потоков
                    {
                        throw new ApplicationException($"Не удалось получить первый поток для процесса {processId}. Код ошибки: {errorCode}");
                    }
                }
            }
            finally
            {
                // Освобождаем ресурсы
                if (snapshot != IntPtr.Zero && snapshot != new IntPtr(-1))
                {
                    CloseHandle(snapshot);
                }
            }

            return threads; // Возвращаем найденные потоки
        }

        // Комбинированный метод: получает процессы с заданным количеством потоков и их потоки
        public static Dictionary<uint, List<ThreadInfo>> GetAllProcessesWithThreads(uint targetThreadCount)
        {
            var result = new Dictionary<uint, List<ThreadInfo>>();

            // Получаем все процессы с заданным количеством потоков
            var processes = GetProcessesWithThreadCount(targetThreadCount);

            // Для каждого процесса получаем его потоки
            foreach (var process in processes)
            {
                var threads = GetThreadsForProcess(process.ProcessID);
                result[process.ProcessID] = threads; // Сохраняем в словаре: PID -> список потоков
            }

            return result; // Возвращаем словарь с полной информацией
        }
    }
}