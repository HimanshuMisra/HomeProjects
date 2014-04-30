using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//Trying to Take Concurrently
namespace TestThread
{
    //This sequence causes an exception to be thrown : 
    //The consumer checks the IsCompleted property and gets false.
    //The producer calls the CompleteAdding() method.
    //The consumer calls Take().
    //instead use the TryTake() method
    class Program73
    {
        static void Main73(string[] args)
        {
            // create a blocking collection
            BlockingCollection<int> blockingCollection
            = new BlockingCollection<int>();
            // create and start a producer
            Task.Factory.StartNew(() =>
            {
                // put items into the collectioon
                for (int i = 0; i < 1000; i++)
                {
                    blockingCollection.Add(i);
                }
                // mark the collection as complete
                blockingCollection.CompleteAdding();
            });
            // create and start a producer
            Task.Factory.StartNew(() =>
            {
                while (!blockingCollection.IsCompleted)
                {
                    // take an item from the collection
                    int item = blockingCollection.Take();
                    // print out the item
                    Console.WriteLine("Item {0}", item);
                }
            });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}