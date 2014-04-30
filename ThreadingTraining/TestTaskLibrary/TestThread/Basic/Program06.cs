using System;
using System.Threading.Tasks;

//Getting a Result with the Task Factory

namespace TestThread
{
    class Program06
    {
        static void Main6(string[] args)
        {
            /*
 *  For performance reasons, TaskFactory's StartNew method should be the preferred mechanism 
 *  for creating and scheduling computational tasks, but for scenarios where creation and scheduling must be separated, the constructors may be used
 */
            Task<int> task1 = Task.Factory.StartNew<int>(() =>
            {
                int sum = 0;
                for (int i = 0; i < 100; i++)
                {
                    sum += i;
                }
                return sum;
            });
            // write out the result
            Console.WriteLine("Result 1: {0}", task1.Result);
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();

        }
    }       
}
