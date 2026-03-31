//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Runtime.InteropServices;

//namespace _2.Lab_OS_No12
//{
//    /// Класс для поиска файлов в каталоге с использованием FindFirstFile и FindNextFile
//    /// Выполняет задание для одного каталога
//    public class WinAPIFileFinder
//    {
//        /// Структура Find_DATA для хранения информации о найденном файле

//        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
//        public struct Find_DATA
//        {
//            public uint dwFileAttributes;      // Атрибуты файла
//            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;   // Время создания
//            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime; // Время последнего доступа
//            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime; // Время последней записи
//            public uint nFileSizeHigh; // Старшие 32 бита размера файла
//            public uint nFileSizeLow; // Младшие 32 бита размера файла
//            public uint dwReserved0;  // Зарезервировано
//            public uint dwReserved1;  // Зарезервировано
//            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
//            public string cFileName;  // Имя файла (максимум 260 символов)
//            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
//            public string cAlternateFileName;  // Альтернативное имя файла (8.3 формат)
//        }

//        // WinAPI функции
//        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
//        private static extern IntPtr FindFirstFile(string lpFileName, out Find_DATA lpFindFileData);

//        /// Функция FindNextFile - продолжает поиск файлов
//        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
//        private static extern bool FindNextFile(IntPtr hFindFile, out Find_DATA lpFindFileData);

//        /// Функция FindClose - закрывает дескриптор поиска
//        [DllImport("kernel32.dll")]
//        private static extern bool FindClose(IntPtr hFindFile);

//        private readonly string _directoryPath;  // Путь к сканируемой директории
//        public WinAPIFileFinder(string directoryPath)
//        {
//            _directoryPath = directoryPath;
//        }

//        /// Найти самый большой файл в каталоге (рекурсивно)
//        /* public FileInfo FindLargestFile()
//         {
//             FileInfo largestFile = null;  // Самый большой файл
//             long maxSize = -1;            // Максимальный размер файла

//             // Рекурсивно обходим все файлы в директории и поддиректориях
//             FindFilesRecursive(_directoryPath, (filePath, findData) =>
//             {
//                 // Получаем размер текущего файла
//                 long fileSize = GetFileSize(findData);

//                 // Сравниваем с текущим максимальным размером
//                 if (fileSize > maxSize)
//                 {
//                     maxSize = fileSize;
//                     largestFile = new FileInfo(filePath);  // Обновляем самый большой файл
//                 }
//             });

//             return largestFile;  // Возвращаем результат поиска
//         }
//        */
//        public string FindLargestFileName()
//        {
//            string largestFileName = null;
//            long maxSize = -1;

//            FindFilesRecursive(_directoryPath, (filePath, findData) =>
//            {
//                long fileSize = GetFileSize(findData);

//                if (fileSize > maxSize)
//                {
//                    maxSize = fileSize;
//                    largestFileName = Path.GetFileName(filePath);  
//                }
//            });

//            return largestFileName;  // Возвращаем только имя (string)
//        }
//        /// Получить все файлы в каталоге (рекурсивно)
//        /// Вспомогательный метод для получения полного списка файлов
//        public List<string> GetAllFiles()
//        {
//            var files = new List<string>();  // Список для хранения путей к файлам

//            // Рекурсивно обходим все файлы и добавляем их в список
//            FindFilesRecursive(_directoryPath, (filePath, findData) =>
//            {
//                files.Add(filePath);  
//            });

//            return files;  
//        }

//        /// Рекурсивный поиск файлов с использованием FindFirstFile и FindNextFile
//        /// Основной метод, реализующий поиск через WinAPI функции

//        private void FindFilesRecursive(string directory, Action<string, Find_DATA> fileAction)
//        {
//            Find_DATA findData;

//            IntPtr findHandle = FindFirstFile(Path.Combine(directory, "*"), out findData);

//            // Проверяем валидность дескриптора поиска
//            if (findHandle == IntPtr.Zero || findHandle == new IntPtr(-1))
//                return;  

//            try
//            {
//                do
//                {
//                    string fileName = findData.cFileName;  // Имя найденного файла/папки

//                    if (fileName == "." || fileName == "..")
//                        continue;

//                    string fullPath = Path.Combine(directory, fileName);  // Полный путь

//                    // Проверяем атрибуты файла - является ли он директорией
//                    if ((findData.dwFileAttributes & 0x10) != 0)
//                    {
//                        // Это директория - рекурсивно обрабатываем её содержимое
//                        FindFilesRecursive(fullPath, fileAction);
//                    }
//                    else
//                    {
//                        // Это файл - выполняем переданное действие
//                        fileAction(fullPath, findData);
//                    }
//                }
//                while (FindNextFile(findHandle, out findData)); // FindNextFile - продолжение поиска

//            }
//            finally
//            {
//                // для гарантии освобождения ресурсов
//                FindClose(findHandle);
//            }
//        }

//        /// Получить размер файла из Find_DATA
//        /// Вспомогательный метод для вычисления размера файла
//        private long GetFileSize(Find_DATA findData)
//        {
//            return (long)findData.nFileSizeHigh << 32 | findData.nFileSizeLow;
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace _2.Lab_OS_No12
{
    /// Класс для поиска файлов в каталоге с использованием FindFirstFile и FindNextFile
    public class WinAPIFileFinder
    {
        // WinAPI структуры
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        // WinAPI функции
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll")]
        private static extern bool FindClose(IntPtr hFindFile);

        private readonly string _directoryPath;
        public class FileData
        {
            public string Name { get; set; }           // Имя файла
            public string FullPath { get; set; }       // Полный путь
            public long Size { get; set; }             // Размер файла
            public bool IsDirectory { get; set; }      // Это директория?

            public FileData(string name, string fullPath, long size, bool isDirectory)
            {
                Name = name;
                FullPath = fullPath;
                Size = size;
                IsDirectory = isDirectory;
            }
        }

        public WinAPIFileFinder(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public FileData FindLargestFile()
        {
            FileData largestFile = null;
            long maxSize = -1;

            // Рекурсивно обходим все файлы
            FindFilesRecursive(_directoryPath, (filePath, fileName, findData) =>
            {
                long fileSize = GetFileSize(findData);
                bool isDirectory = IsDirectory(findData);

                // Ищем только файлы (не директории) и сравниваем размер
                if (!isDirectory && fileSize > maxSize)
                {
                    maxSize = fileSize;
                    largestFile = new FileData(fileName, filePath, fileSize, false);
                }
            });

            return largestFile;
        }

        /// Получить все файлы в каталоге (рекурсивно) - возвращает только пути
        public List<string> GetAllFiles()
        {
            var files = new List<string>();

            FindFilesRecursive(_directoryPath, (filePath, fileName, findData) =>
            {
                bool isDirectory = IsDirectory(findData);
                if (!isDirectory) // Добавляем только файлы, не директории
                {
                    files.Add(filePath);
                }
            });

            return files;
        }

        /// Рекурсивный поиск файлов с использованием FindFirstFile и FindNextFile

        private void FindFilesRecursive(string directory, Action<string, string, WIN32_FIND_DATA> fileAction)
        {
            WIN32_FIND_DATA findData;

            // Создаем путь для поиска
            string searchPath = directory;
            if (!searchPath.EndsWith("\\"))
                searchPath += "\\";
            searchPath += "*";

            // FindFirstFile - начало поиска
            IntPtr findHandle = FindFirstFile(searchPath, out findData);

            if (findHandle == IntPtr.Zero || findHandle == new IntPtr(-1))
                return;

            try
            {
                do
                {
                    string fileName = findData.cFileName;

                    // Пропускаем текущую и родительскую директории
                    if (fileName == "." || fileName == "..")
                        continue;

                    // Создаем полный путь
                    string fullPath = directory;
                    if (!fullPath.EndsWith("\\"))
                        fullPath += "\\";
                    fullPath += fileName;

                    bool isDirectory = IsDirectory(findData);

                    if (isDirectory)
                    {
                        // Рекурсивный обход поддиректории
                        FindFilesRecursive(fullPath, fileAction);
                    }
                    else
                    {
                        // Обработка файла
                        fileAction(fullPath, fileName, findData);
                    }
                }
                while (FindNextFile(findHandle, out findData));
            }
            finally
            {
                FindClose(findHandle);
            }
        }

        private long GetFileSize(WIN32_FIND_DATA findData)
        {
            return (long)findData.nFileSizeHigh << 32 | findData.nFileSizeLow;
        }

        private bool IsDirectory(WIN32_FIND_DATA findData)
        {
            return (findData.dwFileAttributes & 0x10) != 0; // FILE_ATTRIBUTE_DIRECTORY
        }
    }
}