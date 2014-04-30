using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

//A Parallel For Loop
namespace TestThread
{
    class Program77
    {
        static void Main77(string[] args)
        {
            Console.WriteLine("MTID={0}",
                Thread.CurrentThread.ManagedThreadId);

            Stopwatch sw = Stopwatch.StartNew();
            NonParallelMethod();
            sw.Stop();
            Console.WriteLine("It Took {0} ms",
                sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            ParallelMethod();

            sw.Stop();
            Console.WriteLine("It Took {0} ms",
                sw.ElapsedMilliseconds);

            Console.WriteLine("\nFinished...");
            Console.ReadKey(true);
        }

        static void NonParallelMethod()
        {
            for (int i = 0; i < 16; i++)
            {
                Console.WriteLine("TID={0}, i={1}",
                    Thread.CurrentThread.ManagedThreadId,
                    i);

                SimulateProcessing();
            }
        }

        static void ParallelMethod()
        {
            Parallel.For(0, 16, i =>
            {
                Console.WriteLine("TID={0}, i={1}",
                    Thread.CurrentThread.ManagedThreadId,
                    i);

                SimulateProcessing();
            });
        }

        static void SimulateProcessing()
        {
            Thread.SpinWait(80000000);
        }
    }
}
