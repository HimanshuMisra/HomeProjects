using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

//Setting Options for a Parallel Loop
namespace TestThread
{
    class Program80
    {
        static void Main80(string[] args)
        {
            // create a ParallelOptions instance
            // Setting this property to -1 means no concurrency limit is set.
            ParallelOptions options= new ParallelOptions() { MaxDegreeOfParallelism = 1 };
            // perform a parallel for loop
            Parallel.For(0, 10, options, index =>
            {
                Console.WriteLine("For Index {0} started", index);
                Thread.Sleep(500);
                Console.WriteLine("For Index {0} finished", index);
            });
            // create an array of ints to process
            int[] dataElements = new int[] { 0, 2, 4, 6, 8 };
            // perform a parallel foreach loop
            Parallel.ForEach(dataElements, options, index =>
            {
                Console.WriteLine("ForEach Index {0} started", index);
                Thread.Sleep(500);
                Console.WriteLine("ForEach Index {0} finished", index);
            });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }

     
    }
}
