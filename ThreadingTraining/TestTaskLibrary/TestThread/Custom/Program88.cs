using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

//Using a Chunking Partitioner
namespace TestThread
{
    //PLINQ has three partitioning strategies for assigning input elements to threads
    //Range Paritioning : if sequence is indexable (if it’s an array or implements IList<T>
    //chunk partitioning : default, Partitioner.Create 
    //Hash partitioning (slowest) : GroupBy, Join, GroupJoin, Intersect, Except, Union, and Distinct

    class Program88
    {
        //Rather than using a Parallel.For() loop that invokes the delegate for each index value
        //we invoke a delegate for each chunk
        static void Main88(string[] args)
        {
            // create the results array
            double[] resultData = new double[10000000];

            // created a partioner that will chunk the data
            //each Tuple represents a chunk or range of index values.
            //The Tuple.Item1 value is the inclusive start index of the range, and the Tuple.Item2 value is the exclusive end index of the range
            OrderablePartitioner<Tuple<int, int>> chunkPart = 
                Partitioner.Create(0,                   //a start index
                                    resultData.Length,  //an end index 
                                    10000);             //optionally the range of index values that each chunk should represent
            
            // perform the loop in chunks
            Parallel.ForEach(chunkPart, chunkRange =>
            {
                // iterate through all of the values in the chunk range
                for (int i = chunkRange.Item1; i < chunkRange.Item2; i++)
                {
                    resultData[i] = Math.Pow(i, 2);
                }
            });
            //By breaking 10,000,000 index values into chunks of 10,000, we reduce the number of times that the delegate is invoked to 1,000
            
            /* If you do not specify the size of each chunk, then the default will be used; 
             * number of items divided by three times the number of processor cores available. 
             * For example, for 1,000 index values on a four-way machine, 1000 / (3 × 4) values= toal values per chunk. 
             * The default may be calculated differently in future releases
             */
            
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}