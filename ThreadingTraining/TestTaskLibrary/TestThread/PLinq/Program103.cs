using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//Using Parallel Ranges and Repeating Sequences
namespace TestThread
{
    class Program103
    {
        static void Main103(string[] args)
        {
            // use PLINQ to process a parallel range
            //Range() method generates a sequence of stepped integer values
            IEnumerable<double> result1 = from e in ParallelEnumerable.Range(0, 10)
            where e % 2 == 0
            select Math.Pow(e, 2);
            // use PLINQ to process a parallel repeating sequence
            ///Repeat() method generates a sequence that contains the same value repeated over and over.
            IEnumerable<double> result2 =
            ParallelEnumerable.Repeat(10, 100)
            .Select(item => Math.Pow(item, 2));

           Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }   
    }
}
