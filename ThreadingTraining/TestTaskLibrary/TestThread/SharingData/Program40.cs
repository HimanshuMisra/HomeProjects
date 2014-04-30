using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

//Trouble with Data
namespace TestThread
{
    /*
     * The example gives rise to two kinds of data race. The first is where the counter value exceeds 1,000,
which happens because the steps in the Queue.Dequeue() method are not synchronized so Tasks are
reading the same value several times from the head of the queue. The second is a
System.InvalidOperationException, thrown when calls to Queue.Dequeue() are made when the queue is
empty; this happens because the check to see if there are items left in the queue (sharedQueue.Count >
0) and the request to take an item from the queue (sharedQueue.Dequeue()) are not protected in a critical
region
     * */
    class Program40
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static void Main40(string[] args)
        {
            // create a shared collection
            Queue<int> sharedQueue = new Queue<int>();
            // populate the collection with items to process
            for (int i = 0; i < 1000; i++)
            {
                sharedQueue.Enqueue(i);
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
                    while (sharedQueue.Count > 0)
                    {
                        // take an item from the queue
                        int item = sharedQueue.Dequeue();
                        // increment the count of items processed
                        Interlocked.Increment(ref itemCount);
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
