using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//Forcing Parallel Execution of a Query
namespace TestThread
{
    //PLINQ analyses your query to determine the best way to execute
    //it and may, as a consequence, decide that sequential execution will offer better performance
    class Program96
    {
        static void Main96(string[] args)
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
            .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
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
