using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

//static content
namespace TestThread
{
    class Program92
    {
        //because the contents are partitioned when the parallel loop starts,
        //any items that the Task adds after that point are not processed
        static void Main92(string[] args)
        {
            // create some source data
            List<int> sourceData = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                sourceData.Add(i);
            }
            // start a task that adds items to the list
            Task.Factory.StartNew(() =>
            {
                int counter = 10;
                while (true)
                {
                    Thread.Sleep(250);
                    Console.WriteLine("Adding item {0}", counter);
                    sourceData.Add(counter++);
                }
            });
            // run a parallel loop on the list
            Parallel.ForEach(sourceData, item =>
            {
                Console.WriteLine("Processing item {0}", item);
            });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
