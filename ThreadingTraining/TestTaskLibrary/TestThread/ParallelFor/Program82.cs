using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//Using Break() in a Parallel Loop
namespace TestThread
{
    class Program82
    {
        //If you call Break() in the tenth iteration, no further
        //iterations will be started except those that are required to process the first to ninth items.
        static void Main82(string[] args)
        {
            ParallelLoopResult res = Parallel.For(0, 100,(int index, ParallelLoopState loopState) =>
                {
                    // calculate the square of the index
                    double sqr = Math.Pow(index, 2);
                    // if the square value is > 100 then break
                    if (sqr > 100)
                    {
                        Console.WriteLine("Breaking on index {0}", index);
                        loopState.Break();
                    }
                    else
                    {
                        // write out the value
                        Console.WriteLine("Square value of {0} is {1}", index, sqr);
                    }
                });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
     
    }
}
