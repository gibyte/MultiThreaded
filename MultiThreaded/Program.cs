using System.Diagnostics;
using System.Numerics;

int[] array1 = GenerateRandomArray(100000);
int[] array2 = GenerateRandomArray(1000000);
int[] array3 = GenerateRandomArray(10000000);

MeasureTime(array1, "Array size 100000");
MeasureTime(array2, "Array size 1000000");
MeasureTime(array3, "Array size 10000000");

static void MeasureTime(int[] array, string message)
{
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine(message);
    Stopwatch stopwatch = new Stopwatch();

    // Normal sum
    Console.WriteLine("Normal");
    stopwatch.Start();
    NormalSum(array);
    stopwatch.Stop();
    Console.WriteLine("time: " + stopwatch.ElapsedMilliseconds + "ms");
    Console.WriteLine();
    
    // Threads sum
    Console.WriteLine("Threads");
    stopwatch.Reset();
    stopwatch.Start();
    ThreadsSum(array);
    stopwatch.Stop();
    Console.WriteLine("time: " + stopwatch.ElapsedMilliseconds + "ms");
    Console.WriteLine();
    
    // LINQ sum
    Console.WriteLine("LINQ");
    stopwatch.Reset();
    stopwatch.Start();
    LINQSum(array);
    stopwatch.Stop();
    Console.WriteLine("time: " + stopwatch.ElapsedMilliseconds + "ms");
}


static void NormalSum(int[] array)
{

    BigInteger sum = 0;
    foreach (int num in array)
    {
        sum += num;
    }
    Console.WriteLine("Sum: " + sum);
}

static void ThreadsSum(int[] array)
{
    int chunkSize = array.Length / 4; // Размер части массива для каждого потока
    BigInteger[] results = new BigInteger[4];
    Thread[] threads = new Thread[results.Length];

    for (int i = 0; i < results.Length; i++)
    {
        int threadIndex = i;
        threads[i] = new Thread(() => CalculateSum(array, threadIndex, results));
        threads[i].Start();
    }

    foreach (Thread thread in threads)
    {
        thread.Join();
    }

    BigInteger sum = 0;
    foreach (BigInteger result in results)
    {
        sum += result;
    }
    Console.WriteLine("Sum: " + sum);
}

static void CalculateSum(in int[] array, int threadIndex, BigInteger[] results)
{
    int chunkSize = array.Length / results.Length;
    int startIndex = threadIndex * chunkSize;
    int endIndex = (threadIndex == results.Length - 1) ? array.Length : (threadIndex + 1) * chunkSize;
    BigInteger sum = 0;

    for (int i = startIndex; i < endIndex; i++)
    {
        sum += array[i];
    }

    results[threadIndex] = sum;
}

static void LINQSum(int[] array)
{
    BigInteger sum = array.AsParallel().Sum(e => (long)e);
    Console.WriteLine("Sum: " + sum);
}

static int[] GenerateRandomArray(int size)
{
    Random random = new();
    int[] array = new int[size];
    for (int i = 0; i < size; i++)
    {
        array[i] = random.Next();
    }
    return array;
}