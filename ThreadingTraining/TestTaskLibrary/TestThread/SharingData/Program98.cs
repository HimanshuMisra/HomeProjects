using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//Forcing Sequential Execution
namespace TestThread
{
    class Program98
    {
        //this technique can be useful if you want to enable and
        //disable parallelism in subqueries
        static void Main98(string[] args)
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
            .Select(item => Math.Pow(item, 2))
            .AsSequential()
            .Select(item => item * 2);
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
