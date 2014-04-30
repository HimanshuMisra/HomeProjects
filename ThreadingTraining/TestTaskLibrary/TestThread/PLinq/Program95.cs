using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//Preserving Order in a Parallel Query
namespace TestThread
{
    class Program95
    {
        //AsOrderable() preserves the order of the query results so that the nth result is the result of processing the nth item in
        //the source data. The OrderBy() method sorts the items rather than preserve their natural order.
        static void Main95(string[] args)
        {
            // create some source data
            int[] sourceData = new int[10];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = i;
            }
            // without the AsOrdered() method
            IEnumerable<double> results = from item in sourceData.AsParallel().AsOrdered()
                                          select Math.Pow(item, 2);
            foreach (double d in results)
            {
                Console.WriteLine("Unordered result: {0}", d);
            }
            
            // preserve order with the AsOrdered() method
            results = from item in sourceData.AsParallel().AsOrdered()
                                                    select Math.Pow(item, 2);
            // enumerate the results of the parallel query
            foreach (double d in results)
            {
                Console.WriteLine("Ordered result: {0}", d);
            }
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }   
    }
}
