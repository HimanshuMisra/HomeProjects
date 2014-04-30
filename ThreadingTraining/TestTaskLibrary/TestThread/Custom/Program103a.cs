using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace TestThread.ParallelFor
{
    class Program109a
    {
        //Range partition preallocates an equal number of elements to each worker
        // if some threads  finish early, they sit idle while the remaining threads continue working. 

        //If the input sequence is indexable (if it’s an array or implements IList<T>), 
        //PLINQ chooses range partitioning.Can call ToList or ToArray on the input sequence(but performance)

        //Chunk partitioning periodically grabs elements 
        //if some threads  finish early,  it will end up getting more chunks.

        //For linked lists or other collections whose length is not known uses chunk partitioning.

        //prime number calculator might perform poorly with range partitioning. 
        //range partitioning would do well is in calculating the sum of the square roots

        //In a nutshell, range partitioning is faster with long sequences for which every element takes 
        //a similar amount of CPU time to process. Otherwise, chunk partitioning is usually faster

        
        //ParallelEnumerable.Range  uses Range Partition
        //Enumerable.Range uses chunk partition
        //Hash Used for Join, GroupJoin, GroupBy, Distinct, Except, Union and Intersect (slowest)
        //Stripped  Round Robin, designed for Skip&Take
        static void Main103a()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            f1();
            s.Stop();
            Console.WriteLine("chunk prime" + s.ElapsedMilliseconds);
            s = new Stopwatch();
            s.Start();
            f2();
            s.Stop();
            Console.WriteLine("Range prime" + s.ElapsedMilliseconds);
            s = new Stopwatch();
            s.Start();
            f3();
            s.Stop();
            Console.WriteLine("chunk Sqr " + s.ElapsedMilliseconds);
            s = new Stopwatch();
            s.Start();
            f4();
            s.Stop();
            Console.WriteLine("Range sqr " + s.ElapsedMilliseconds);
        }
        static void f1()
        {
            //non indexable
            IEnumerable<int> million = Enumerable.Range(3, 10000000);
            DoPrime(million);
        }
        static void f2()
        {
            //indexable
            IEnumerable<int> million = ParallelEnumerable.Range(3, 10000000); //<--implements IList<T>
            DoPrime(million);
        }
        static void f3()
        {
            //non index
            IEnumerable<int> million = Enumerable.Range(1, 10000000);
            DoSquare(million);
        }
        static void f4()
        {
            //indexable
            IEnumerable<int> million = ParallelEnumerable.Range(3, 10000000);
            DoSquare(million);
        }
        static void DoPrime(IEnumerable<int> col)
        {
            var primeNumberQuery =
                from n in col.AsParallel()
                where Enumerable.Range(2, (int)Math.Sqrt(n)).All(i => n % i > 0)
                select n;

            int[] primes = primeNumberQuery.ToArray();

        }
        static void DoSquare(IEnumerable<int> col)
        {
            var sqrQuery =
             from n in col.AsParallel()
             select n * n;

            int[] sqr = sqrQuery.ToArray();
        }
        static void f5()
        {
            //When the partitioner is configured to load-balance, chunk partitioning is used, and the elements are handed off to each partition in small chunks as they are requested. This approach helps ensure that all partitions have elements to process until the entire loop or query is completed. 

            //the partitioner does incur the synchronization overhead each time the thread needs to get another chunk. The amount of synchronization incurred in these cases is inversely proportional to the size of the chunks.

            //In contrast, static partitioning can assign the elements to each partitioner all at once by using either range or chunk partitioning.
            var million = ParallelEnumerable.Range(3, 10000000).ToArray();

            // Create a load-balancing partitioner. Or specify false for static partitioning.
            Partitioner<int> customPartitioner = Partitioner.Create(million, true);


            var sqrQuery =
           from n in million.AsParallel()
           select Math.Sqrt(n);

            double[] sqr = sqrQuery.ToArray();
            //            foreach (int i in primeNumberQuery)
            //                Console.WriteLine(i);
        }
    }
}
