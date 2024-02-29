// See https://aka.ms/new-console-template for more information
using System.Drawing;

Console.WriteLine("Hello, World!");

int[] array = GenerateRandomArray(100000);
//int[] array = GenerateRandomArray(1000000);
//int[] array = GenerateRandomArray(10000000);

NormalSum(array);
ThreadsSum(array);

static void NormalSum(int[] array)
{
    
    int sum = 0;
    foreach (int num in array)
    {
        sum += num;
    }
    Console.WriteLine("Сумма элементов массива: " + sum);
}

static void ThreadsSum(int[] array)
{
    int sum = 0;
    List<Thread> threads = new List<Thread>();

    foreach (int num in array)
    {
        Thread thread = new((object obj) =>
        {
            int element = (int)obj;
            Interlocked.Add(ref sum, element);
        });
        threads.Add(thread);
        thread.Start(num);
    }

    foreach (Thread thread in threads)
    {
        thread.Join();
    }

    Console.WriteLine("Сумма элементов массива (параллельно с Thread): " + sum);
}

static int[] GenerateRandomArray(int size)
{
    Random random = new Random();
    int[] array = new int[size];
    for (int i = 0; i < size; i++)
    {
        array[i] = random.Next(); // Генерация случайного числа
    }
    return array;
}