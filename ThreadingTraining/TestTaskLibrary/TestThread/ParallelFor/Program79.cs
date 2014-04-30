using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//Creating a Stepped Loop
namespace TestThread
{
    //for (int i = 0; i < 10; i +=2) {
    //increments the loop counter by two per iteration to create a stepped index
    class Program79
    {
        static IEnumerable<int> SteppedIterator(int startIndex, int endEndex, int stepSize)
        {
            for (int i = startIndex; i < endEndex; i += stepSize)
            {
                yield return i;
            }
        }
        static void Main79(string[] args)
        {
            Parallel.ForEach(SteppedIterator(0, 10, 2), index =>
            {
                Console.WriteLine("Index value: {0}", index);
            });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }


    }
}
