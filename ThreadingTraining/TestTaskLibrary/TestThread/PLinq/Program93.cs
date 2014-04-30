using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

//A ParallelInvoke
namespace TestThread
{
    //PLINQ is only for local collections: it doesn’t work with LINQ to SQL or Entity Framework
    class Program93
    {
        static void Main93(string[] args)
        {
            int[] sourceData = new int[100];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = i;
            }
            IEnumerable<int> results =
            from item in sourceData.AsParallel()
            where item % 2 == 0
            select item;
            foreach (int item in results)
            {
                Console.WriteLine("Item {0}", item);
            }
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }   
    }
}
