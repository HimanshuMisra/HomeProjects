using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//Using Stop() in a Parallel Loop
namespace TestThread
{
    class Program81
    {
        //■Stop – This method notifies that all the iterations will be stopped.
        static void Main81(string[] args)
        {
            List<string> dataItems = new List<string>() { "an", "apple", "a", "day",
"keeps", "the", "doctor", "away","ffff","ggggg","hhhh" };
            Parallel.ForEach(dataItems, (string item, ParallelLoopState state) =>
            {
                if (item.Contains("k"))
                {
                    Console.WriteLine("Hit: {0}", item);
                    state.Stop();
                }
                else
                {
                    Console.WriteLine("Miss: {0}", item);
                }
            });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }

     
    }
}
