using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace PLINQDemo
{
    class Program94
    {
        static void Main94(string[] args)
        {
            IEnumerable<int> numbers = Enumerable.Range(1, 1000);

            // Remove AsParallel() Method in PLINQ query to see the difference in speed
            IEnumerable<int> results = from n in numbers.AsParallel()
                                       .WithDegreeOfParallelism(1)
                                       where IsDivisibleByFive(n)
                                       select n;

            Stopwatch sw = Stopwatch.StartNew();
            IList<int> resultsList = results.ToList();
            Console.WriteLine("{0} items", resultsList.Count());
            sw.Stop();

            Console.WriteLine("It Took {0} ms", sw.ElapsedMilliseconds);

            Console.WriteLine("\nFinished...");
            Console.ReadKey(true);
        }

        static bool IsDivisibleByFive(int i)
        {
            Thread.SpinWait(2000000);

            return i % 5 == 0;
        }
    }
}