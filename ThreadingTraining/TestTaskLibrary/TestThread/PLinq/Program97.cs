using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//Limiting the Degree of Parallelism for a Query
namespace TestThread
{
    class Program97
    {
        //You can request an upper limit to the number of Tasks that will be used to perform your LINQ query

        static void Main97(string[] args)
        {
            // create some source data
            int[] sourceData = new int[10];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = i;
            }
            // define the query and force parallelism
            IEnumerable<double> results =
            sourceData.AsParallel()
            .WithDegreeOfParallelism(2)
            .Where(item => item % 2 == 0)
            .Select(item => Math.Pow(item, 2));
            // enumerate the results
            foreach (double d in results)
            {
                Console.WriteLine("Result {0}", d);
            }
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }   
    }
}
