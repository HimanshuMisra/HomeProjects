﻿using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

//Using the ConcurrentBag Class
namespace TestThread
{
    //The ConcurrentBag class implements an unordered collection, such that the order in which items are
    //added does not guarantee the order in which they will be returned.
    class Program43
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static void Main43(string[] args)
        {
            // create a shared collection
            ConcurrentBag<int> sharedBag = new ConcurrentBag<int>();
            // populate the collection with items to process
            for (int i = 0; i < 1000; i++)
            {
                sharedBag.Add(i);
            }
            // define a counter for the number of processed items
            int itemCount = 0;
            // create tasks to process the list
            Task[] tasks = new Task[10];
            for (int i = 0; i < tasks.Length; i++)
            {
                // create the new task
                tasks[i] = new Task(() =>
                {
                    while (sharedBag.Count > 0)
                    {
                        // define a variable for the dequeue requests
                        int queueElement;
                        // take an item from the queue
                        bool gotElement = sharedBag.TryTake(out queueElement);
                        Console.WriteLine(queueElement);
                        // increment the count of items processed
                        if (gotElement)
                        {
                            Interlocked.Increment(ref itemCount);
                        }
                    }
                });
                // start the new task
                tasks[i].Start();
            }
            // wait for the tasks to complete
            Task.WaitAll(tasks);
            // report on the number of items processed
            Console.WriteLine("Items processed: {0}", itemCount);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
