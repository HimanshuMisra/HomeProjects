using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//Using the ParallelLoopResult Structure
namespace TestThread
{
    class Program83
    {
        static void Main83(string[] args)
        {
            // run a parallel loop in which one of
            // the iterations calls Stop()
            ParallelLoopResult loopResult =
            Parallel.For(0, 10, (int index, ParallelLoopState loopState) =>
            {
                if (index == 2)
                {
                    loopState.Stop();
                }
            });
            // get the details from the loop result
            Console.WriteLine("Loop Result");
            Console.WriteLine("IsCompleted: {0}", loopResult.IsCompleted);
            //LowestBreakIteration Return the index of the lowest iteration in which Break() was called.
            Console.WriteLine("BreakValue: {0}", loopResult.LowestBreakIteration.HasValue);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
     
    }
}
