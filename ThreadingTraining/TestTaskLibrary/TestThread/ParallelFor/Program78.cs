﻿using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//Processing a Collection Using Parallel.ForEach()
namespace TestThread
{
    class Program78
    {
        static void Main78(string[] args)
        {
            // create a collection of strings
            List<string> dataList = new List<string> {"the", "quick", "brown", "fox", "jumps", "etc"};
            // process the elements of the collection
            // using a parallel foreach loop
            Parallel.ForEach(dataList, item =>
            {
                Console.WriteLine("Item {0} has {1} characters",
                item, item.Length);
            });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }

     
    }
}
