/*//using System;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using System.Transactions;
//namespace our_5_lab
//{
//    class Program
//    {

//        //static void transpos(int[,] arra, int size)
//        //{
//        //    for (int i = 0; i < size; i++)
//        //    {
//        //        for (int j = 0; j < size; j++)
//        //            arra[j, i] = arra[i, j];
//        //    }
//        //}
//        //static void find_trnsapos_Nonprime(int[,] arra, int size, int row)
//        //{
//        //    //transpos(arra, size);
//        //    //int[] storage = new int[size];
//        //    //for (int i = 0; i < size; i++)
//        //    //{
//        //    //    if (!prim_number(arra[column_row, i]))
//        //    //        storage[i] = arra[column_row, i];
//        //    //}
//        //    ////printing the array which is non prime number;
//        //    //for (int i = 0; i < size; i++)
//        //    //    Console.Write(storage[i] + " ");
//        //    transpos(arra, size);
//        //    print(arra, size);
//        //    for (int i = 1; i <= size; i++)
//        //    {
//        //        if (!prim_number(arra[row, i]))
//        //            Console.Write(arra[row, i] + "  ");
//        //    }
//        //}
//        //------------------------------------------------
//        static void giving_value(int[,] arr, int size)
//        {
//            for (int i = 0; i < size; i++)
//            {
//                for (int j = 0; j < size; j++)
//                {
//                    Console.Write("Enter the value for index [" + i + "][" + j + "]: ");
//                    arr[i, j] = int.Parse(Console.ReadLine());
//                }
//            }
//        }
//        static void print(int[,] arr, int size)
//        {
//            for (int i = 0; i < size; ++i)
//            {
//                for (int j = 0; j < size; ++j)
//                    Console.Write(arr[i, j] + " ");
//                Console.WriteLine();
//            }
//        }
//        static bool prim_number(int number)
//        {
//            if (number == 0 || number == 1)
//                return false;
//            if (number == 2)
//                return true;
//            else
//            {
//                for (int i = 2; i <= Math.Sqrt(number); ++i)
//                {
//                    if (number % i == 0)
//                        return false;
//                }
//            }
//            return true;
//        }

//        static void primFunction(int[,] arr, int size, int column)
//        {
//            Console.WriteLine($"Non prime numbers in column {column + 1}: ");

//            if (column < 0 || column > size)
//                Console.WriteLine("Invalid column");

//            bool message = false; //For message that Non Prime Number is not exicted;
//            for (var i = 0; i < size; i++)
//            {

//                int elem = arr[i, column];
//                if (!prim_number(elem))
//                {
//                    Console.Write(arr[i, column] + " ");
//                    message = true;
//                }
//            }
//            if (!message)
//                    Console.Write("Non prime number(s) is (are) not existed! ");
//        }

//        static void Main(string[] args)
//        {
//            Console.Write("Enter the size for the Square Matrix(n*n): ");
//            int size = int.Parse(Console.ReadLine());
//            int[,] array = new int[size, size];
//            giving_value(array, size);
//            Console.WriteLine("---------------------------------------");
//            Console.WriteLine("our array is ");
//            print(array, size);
//            Console.Write("Enter the column that you want to find the Nonprim numbers: ");
//            int column = int.Parse(Console.ReadLine());
//            primFunction(array, size, column - 1);

//            ////-------------------------------------------------
//            //Console.WriteLine();
//            //Console.Write("Enter the Row Number to find the Non Prim Numbers: ");
//            //int row = Console.Read();
//            //transpos(array, size);
//            //print(array, size);
//            ////find_trnsapos_Nonprime(array, size, row - 1);


//        }
//    }
//}


//using System;

//namespace ConsoleApp
//{
//    class Program
//    {
//        // Функция для проверки, является ли число простым
//        public static bool IsPrime(int number)
//        {
//            bool flag = true;
//            if (number < 2)
//                return flag = false;
//            for (int i = 2; i <= Math.Sqrt(number); i++)
//            {
//                if (number % i == 0)
//                    return false;
//            }
//            return true;
//        }


//        public static int[] FindNonPrimesInColumn(int[,] matrix, int n, ref int[,] originalMatrix, out int resultSize)
//        {
//            originalMatrix = matrix;
//            int[] temp = new int[n];
//            int count = 0;

//            bool has_nonprim = false;
//            for (int row = 0; row < n; row++)
//            {
//                for (int coulmn = 0; coulmn < n; coulmn++)
//                {
//                    int num = matrix[row, 2];
//                    if (!IsPrime(num))
//                    {
//                        temp[count++] = num;
//                        has_nonprim |= true;
//                    }
//                }
//            }
//            if (!has_nonprim)
//            {
//                Console.Write("Prime Number is not exicted!! ");
//            }

//            // Создаем массив только для найденных непростых чисел
//            int[] nonPrimes = new int[count];
//            Array.Copy(temp, nonPrimes, count);
//            resultSize = count;

//            return nonPrimes;
//        }


//        public static void PrintMatrix(int[,] matrix, int n)
//        {
//            Console.WriteLine("\nМатрица:");
//            for (int i = 0; i < n; i++)
//            {
//                for (int j = 0; j < n; j++)
//                {
//                    Console.Write($"{matrix[i, j],5} ");
//                }
//                Console.WriteLine();
//            }
//        }

//        static void Main()
//        {
//            Console.Write("Введите размер матрицы n: ");
//            int n = int.Parse(Console.ReadLine());

//            int[,] matrix = new int[n, n];
//            Console.WriteLine("Введите элементы матрицы:");


//            for (int i = 0; i < n; i++)
//            {
//                for (int j = 0; j < n; j++)
//                {
//                    Console.Write($"Элемент [{i},{j}]: ");
//                    matrix[i, j] = int.Parse(Console.ReadLine());
//                }
//            }


//            PrintMatrix(matrix, n);

//            int[,] originalMatrix = null;
//            int resultSize;

//            Console.WriteLine($"\nThe Non prime Numbers in every table:");

//            int[] nonPrimes = FindNonPrimesInColumn(matrix, n, ref originalMatrix, out resultSize);

//            // Вывод результатов

//            for (int i = 0; i < resultSize; i++)
//            {
//                Console.Write($"{nonPrimes[i]} ");
//            }
//            Console.WriteLine();

//        }
//    }
//}*/

//---------------------------------------

/*using System;

namespace ConsoleApp
{
    class Program
    {
        // Function to check if a number is prime
        public static bool IsPrime(int number)
        {
            if (number < 2)
                return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }

        // Function to find non-prime numbers in every column
        public static int[][] FindNonPrimesInAllColumns(int[,] matrix, int n)
        {
            int[][] nonPrimesPerColumn = new int[n][]; // Array of arrays for non-prime numbers in each column

            for (int col = 0; col < n; col++)
            {
                int[] temp = new int[n]; // Temp array to store non-prime numbers for the current column
                int count = 0;

                for (int row = 0; row < n; row++)
                {
                    int num = matrix[row, col];
                    if (!IsPrime(num))
                    {
                        temp[count++] = num; // Add non-prime number to the array
                    }
                }

                // Trim the temp array to size
                int[] columnNonPrimes = new int[count];
                Array.Copy(temp, columnNonPrimes, count);
                nonPrimesPerColumn[col] = columnNonPrimes; // Assign to the main array
            }

            return nonPrimesPerColumn;
        }

        // Function to print the matrix
        public static void PrintMatrix(int[,] matrix, int n)
        {
            Console.WriteLine("\nMatrix:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{matrix[i, j],5} ");
                }
                Console.WriteLine();
            }
        }

        static void Main()
        {
            Console.Write("Enter the size of the matrix (n): ");
            int n = int.Parse(Console.ReadLine());

            int[,] matrix = new int[n, n];
            Console.WriteLine("Enter the elements of the matrix:");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"Element [{i},{j}]: ");
                    matrix[i, j] = int.Parse(Console.ReadLine());
                }
            }

            // Print the matrix
            PrintMatrix(matrix, n);

            Console.WriteLine("\nNon-prime numbers in each column:");

            // Find non-prime numbers in all columns
            int[][] nonPrimes = FindNonPrimesInAllColumns(matrix, n);

            // Print non-prime numbers for each column
            for (int col = 0; col < n; col++)
            {
                Console.Write($"Column {col + 1}: ");
                if (nonPrimes[col].Length > 0)
                {
                    Console.WriteLine(string.Join(", ", nonPrimes[col]));
                }
                else
                {
                    Console.WriteLine("No non-prime numbers");
                }
            }
        }
    }
}
*/


//--------------------------------------new------------------------------------

using System;

class Program
{
    static bool IsPrime(int num)
    {
        if (num <= 1)
            return false;
        for (int i = 2; i <= Math.Sqrt(num); i++)
        {
            if (num % i == 0)
                return false;
        }
        return true;
    }

    static int[] FindNonPrimeNumbersInColumns(int[,] matrix, int n)
    {
        int[] nonPrimeNumbers = new int[n * n]; 
        int index = 0;

        for (int col = 0; col < n; col++)
        {
            Console.Write($"Non-prime numbers in column {col + 1}: ");
            bool foundNonPrime = false;

            for (int row = 0; row < n; row++)
            {
                int currentNumber = matrix[row, col];

                if (!IsPrime(currentNumber))
                {
                    if (foundNonPrime)
                    {
                        Console.Write(", ");
                    }
                    Console.Write(currentNumber);
                    nonPrimeNumbers[index++] = currentNumber;
                    foundNonPrime = true;
                }
            }

            if (!foundNonPrime)
            {
                Console.Write("None");
            }

            Console.WriteLine();
        }

        Array.Resize(ref nonPrimeNumbers, index);

        return nonPrimeNumbers;
    }

    public static void PrintMatrix(int[,] matrix, int n)
    {
        Console.WriteLine("\nMatrix:");
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"{matrix[i, j],5} ");
            }
            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        Console.Write("Enter the size of the matrix (n): ");
        int n = int.Parse(Console.ReadLine());

        int[,] matrix = new int[n, n];
        Console.WriteLine("Enter the elements of the matrix:");

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"Element [{i},{j}]: ");
                matrix[i, j] = int.Parse(Console.ReadLine());
            }
        }

        PrintMatrix(matrix, n);
    
        int[] nonPrimeNumbers = FindNonPrimeNumbersInColumns(matrix, n);

        Console.WriteLine("\nAll non-prime numbers in the matrix:");
        foreach (int num in nonPrimeNumbers)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }
}




//--------------------------------------Printing in Main function----------------------
/*
using System;
class Program
{
    static bool IsPrime(int num)
    {
        if (num <= 1)
            return false;
        for (int i = 2; i <= Math.Sqrt(num); i++)
        {
            if (num % i == 0)
                return false;
        }
        return true;
    }
    static int[] FindNonPrimeNumbersInColumns(int[,] matrix, int n, out int[] nonPrimeNumbersInColumns)
    {
        nonPrimeNumbersInColumns = new int[n * n]; 
        int index = 0;

        for (int col = 0; col < n; col++)
        {
            for (int row = 0; row < n; row++)
            {
                int currentNumber = matrix[row, col];

                if (!IsPrime(currentNumber))
                {
                    nonPrimeNumbersInColumns[index++] = currentNumber;
                }
            }
        }

        Array.Resize(ref nonPrimeNumbersInColumns, index);

        return nonPrimeNumbersInColumns;
    }

    public static void PrintMatrix(int[,] matrix, int n)
    {
        Console.WriteLine("\nMatrix:");
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"{matrix[i, j],5} ");
            }
            Console.WriteLine();
        }
    }
    static void Main(string[] args)
    {
        Console.Write("Enter the size of the matrix (n): ");
        int n = int.Parse(Console.ReadLine());

        int[,] matrix = new int[n, n];
        Console.WriteLine("Enter the elements of the matrix:");

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"Element [{i},{j}]: ");
                matrix[i, j] = int.Parse(Console.ReadLine());
            }
        }

        PrintMatrix(matrix, n);

        int[] nonPrimeNumbersInColumns;
        FindNonPrimeNumbersInColumns(matrix, n, out nonPrimeNumbersInColumns);

        for (int col = 0; col < n; col++)
        {
            Console.Write($"Non-prime numbers in column {col + 1}: ");

            bool foundNonPrime = false;
            for (int row = 0; row < n; row++)
            {
                int currentNumber = matrix[row, col];

                if (!IsPrime(currentNumber))
                {
                    if (foundNonPrime)
                    {
                        Console.Write(", ");
                    }
                    Console.Write(currentNumber);
                    foundNonPrime = true;
                }
            }

            if (!foundNonPrime)
            {
                Console.Write("None");
            }
            Console.WriteLine();
        }
        
        Console.WriteLine("\nAll non-prime numbers in the matrix:");
        foreach (int num in nonPrimeNumbersInColumns)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }
}
*/