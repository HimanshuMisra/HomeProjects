using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//Optimizing PLINQ using ForAll
namespace TestThread
{
    class Program102
    {
        //PLINQcollates the results into a single output sequence.
        //Sometimes you end up running some function once over each element in that sequence
        //The ForAll method runs a delegate over every output element of a ParallelQuery.

        //The ForAll method runs a delegate over every output element of a ParallelQuery. 
        // It hooks right into PLINQ’s internals, bypassing the steps of collating and enumerating the results. 
        static void Main102(string[] args)
        {
            // create some source data
            int[] sourceData = new int[50];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = i;
            }
            // filter the data and call ForAll()
            sourceData.AsParallel()
            .Where(item => item % 2 == 0)
            .ForAll(item => Console.WriteLine("Item {0} Result {1}",
            item, Math.Pow(item, 2)));

            //the main limitation is that you can’t return a result from the Action which ForAll() invokes.
           Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }   
    }
}
