using System;
using System.Threading.Tasks;
using System.Threading;

//Local Variable Evaluation
namespace TestThread
{
    class Program25
    {
        static void Main25(string[] args)
        {
            Task[] tasks = new Task[5];
            // create and start the "bad" tasks
            for (int i = 0; i < 5; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    // All of the Tasks end up with the same value because of the way 
                    //that the C# variable scoping rules are applied to lambda expressions.
                    Console.WriteLine("Task {0} has counter value: {1}",
                    Task.CurrentId, i);
                });
                // The delegate may have not been invoked yet.
                // Thread.Sleep(1000);
            }
            
            Task.WaitAll(tasks); 
            Console.WriteLine("--------------------");

            // create and start the "good" tasks
            for (int i = 0; i < 5; i++)
            {
                tasks[i] = Task.Factory.StartNew((stateObj) =>
                {
                    // The simplest way to fix this problem is to pass the loop counter in as a state object to the Task.
                    int loopValue = (int)stateObj;
                    // write out a message that uses the loop counter
                    Console.WriteLine("Task {0} has counter value: {1}",
                    Task.CurrentId, loopValue);
                }, i);
            }
            Task.WaitAll(tasks);
            
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
