using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//A Fully Buffered PLINQ Query
namespace TestThread
{
    class Program101
    {
        //FullyBuffered: All of the results are produced before any of them are passed to the consumer.
        //all of the results are generated before any of them are made available for enumeration
       static void Main101(string[] args)
        {
            // create some source data
            int[] sourceData = new int[200];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = i;
            }
            // define a fully buffered query
            IEnumerable<double> results =
            sourceData.AsParallel()
            //.WithMergeOptions(ParallelMergeOptions.FullyBuffered) //full
            //.WithMergeOptions(ParallelMergeOptions.NotBuffered)  //<-- output depends on how many threads
            //.WithMergeOptions(ParallelMergeOptions.Default) 
            //.WithMergeOptions(ParallelMergeOptions.AutoBuffered) //"chunks" 
            .Select(item =>
            {
                double resultItem = Math.Pow(item, 2);
                Console.WriteLine("Produced result {0}", resultItem);
                return resultItem;
            });
            // enumerate the query results
            foreach (double d in results)
            {
                Console.WriteLine("Enumeration got result {0}", d);
            }
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }   
    }
}
