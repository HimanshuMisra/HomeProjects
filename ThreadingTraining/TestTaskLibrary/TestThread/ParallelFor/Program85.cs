using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//A simple parallel loop that requires synchronization
namespace TestThread
{
    class Program85
    {
        static void Main85(string[] args)
        {
            //Because there are 100 steps in the parallel loop, 
            //the Interlocked class will be called 100 times
            int total = 0;
            Parallel.For(0, 100, index =>
            {
                Interlocked.Add(ref total, index);
            });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
     
    }
}
