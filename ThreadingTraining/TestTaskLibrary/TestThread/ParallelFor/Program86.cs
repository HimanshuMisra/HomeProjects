using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//A Parallel for Loop with TLS
namespace TestThread
{
    class Program86
    {
        //When you use a parallel loop, your data is broken down into a series of blocks (known as chunks or
        //partitions), which are processed concurrently.
        static void Main86(string[] args)
        {
            //The benefit of using TLS in a loop is that you don’t need to synchronize access 
            //to the variable in your loop body.
            int total = 0;
            Parallel.For(
            0,          //1. start index
            100,        //2. end index
            () => 0,    //3. a delegate that initializes an instance of the thread-local type (in this case int with 0)
            (int index, ParallelLoopState loopState, int tlsValue) => //4. Action is called once per index value and must return a result that is the same type as the thread-local variable.
            {
                tlsValue += index;
                return tlsValue;
            },
            value => Interlocked.Add(ref total, value));    //5. Action that is passed the TLS value and is called once per data partition.
            Console.WriteLine("Total: {0}", total);

            //Interlocked class is only used once per data partition. //
            //In this test the default practitioner usually breaks up the data into four partitions
            //resulting in 96 fewer calls to Interlocked than the original loop.
            
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
        //The exact savings depends on the source data and
        //the partition strategy used; the greater the number of iterations, the greater the savings in overhead.
     
    }
}
