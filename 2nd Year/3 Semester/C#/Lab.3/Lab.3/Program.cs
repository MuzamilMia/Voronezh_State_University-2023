
using System;
class Program
{
    static void Main()
    {
        Console.Write("ENter the size of the array: ");
        int n = int.Parse(System.Console.ReadLine());
        int[] array = new int[n];

        Console.WriteLine("ENter the value of the array:");
        for (int i = 0; i < n; i++)
        {
            array[i] = int.Parse(System.Console.ReadLine());
        }

        bool[] found = new bool[n + 1];

        for (int i = 0; i < n; i++)
        {
            int element = array[i];

            if (element < 1 || element > n)
            {
                Console.WriteLine($"First Non-Acceptable: {i + 1}");
                return;
            }
            
            if (found[element])
            {
                Console.WriteLine($"First Non-Acceptable Number is : {i + 1}");
                return;
            }

            found[element] = true;
        }
        Console.WriteLine(0);
    }
}
