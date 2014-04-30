using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//Handling Exceptions in a PLINQ Query
namespace TestThread
{
    class Program99
    {
        //Any exceptions thrown in a PLINQ query will be packaged in a System.AggregateException
       static void Main99(string[] args)
        {
           //PLINQ may continue to process data after an exception has been thrown, which means that you
            //should not use exceptions to manually terminate a PLINQ query.

            // create some source data
            int[] sourceData = new int[100];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = i;
            }
            // define the query and force parallelism
            IEnumerable<double> results =
            sourceData.AsParallel()
            .Select(item =>
            {
                if (item == 45)
                {
                    throw new Exception();
                }
                return Math.Pow(item, 2);
            });
            // enumerate the results
            try
            {
                foreach (double d in results)
                {
                    Console.WriteLine("Result {0}", d);
                }
            }
            catch (AggregateException aggException)
            {
                aggException.Handle(exception =>
                {
                    Console.WriteLine("Handled exception of type: {0}",
                    exception.GetType());
                    return true;
                });
            }
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }   
    }
}
