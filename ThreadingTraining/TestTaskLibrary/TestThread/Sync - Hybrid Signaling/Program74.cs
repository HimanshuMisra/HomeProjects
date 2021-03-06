﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//Using BlockingCollection as IEnumerable
namespace TestThread
{
    //If the collection is used, the foreach loop will immediately exit if it is executed before the first item
    //has been added to the collection by the producer.
    class Program74
    {
        static void Main74(string[] args)
        {
            // create a blocking collection
            BlockingCollection<int> blockingCollection
            = new BlockingCollection<int>();
            // create and start a producer
            Task.Factory.StartNew(() =>
            {
                // put the producer to sleep
                System.Threading.Thread.Sleep(500);
                for (int i = 0; i < 100; i++)
                {
                    // add the item to the collection
                    blockingCollection.Add(i);
                }
                // mark the collection as finished
                blockingCollection.CompleteAdding();
            });
            // create and start a consumer
            Task consumer = Task.Factory.StartNew(() =>
            {
                // use a foreach loop to consume the blocking collection
                //foreach (int i in blockingCollection.GetConsumingEnumerable())
                foreach (int i in blockingCollection)
                {
                    Console.WriteLine("Item {0}", i);
                }
                Console.WriteLine("Collection is fully consumed");
            });
            // wait for the consumer to finish
            consumer.Wait();
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}