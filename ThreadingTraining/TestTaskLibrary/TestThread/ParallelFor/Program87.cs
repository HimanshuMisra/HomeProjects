using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

//A Parallel Loop with a Very Small Body
namespace TestThread
{
    //a delegate must be invoked to process every item in a parallel loop
    //The work required by the runtime to invoke the delegate is huge in proportion to the 
    // work required to perform the trivial loop body.
    class Program87
    {
        delegate void ProcessValue(int value);
        static double[] resultData = new double[10000000];

        static void Main87(string[] args)
        {
            // When using lambda expressions, it is easy to forget that the compiler is creating an anonymous delegate
            Parallel.For(0, resultData.Length, (int index) =>
            {
                // compuute the result for the current index
                resultData[index] = Math.Pow(index, 2);
            });
            // perform the loop again, but make the delegate explicit
            Parallel.For(0, resultData.Length, delegate(int index)
            {
                resultData[index] = Math.Pow((double)index, 2);
            });
            // perform the loop once more, but this time using
            // a declared delegate and action
            ProcessValue pdel = new ProcessValue(computeResultValue);
            Action<int> paction = new Action<int>(pdel);
            Parallel.For(0, resultData.Length, paction);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
        static void computeResultValue(int indexValue)
        {
            resultData[indexValue] = Math.Pow(indexValue, 2);
        }
    }
}