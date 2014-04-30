using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

//Using an Orderable Partitioner
namespace TestThread
{

    class Program89
    {
        //The OrdereablePartitioner class extends Partitioner.
        //if you need to preserve order, you need to use OrderablePartitioner
        static void Main89(string[] args)
        {
            // create the source data
            IList<string> sourceData
            = new List<string>() { "an", "apple", "a", "day","keeps", "the", "doctor", "away" };
            // create an array to hold the results
            string[] resultData = new string[sourceData.Count];
            // create an orderable partitioner
            OrderablePartitioner<string> op = Partitioner.Create(sourceData);
            // perform the parallel loop
            Parallel.ForEach(op, (string item, ParallelLoopState loopState, long index) =>
            {
                // process the item
                if (item == "apple") item = "apricot";
                // use the index to set the result in the array
                resultData[index] = item;
            });
            // print out the contents of the result array
            for (int i = 0; i < resultData.Length; i++)
            {
                Console.WriteLine("Item {0} is {1}", i, resultData[i]);
            }
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}