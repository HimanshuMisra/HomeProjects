using System;
using System.Threading.Tasks;
using System.Threading;

//Propagating Exceptions Along a Continuation Chain - ContinueWhenAny
namespace TestThread
{
    //Handling exceptions when using the ContinueWhenAny() method is more difficult. 
    //The best way to avoid unhandled  exceptions in this situation is to combine a selective ContinueWhenAny() 
    //continuation with a ContinueWhenAll() that exists purely to process exceptions
    class Program61
    {
        static void Main61(string[] args)
        {
            // create an array of tasks
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                // create a new task
                tasks[i] = new Task(() =>
                {
                    //do something
                });
            }
            Task.Factory.ContinueWhenAny(tasks, antecedent =>
            {
                // ...task continuaton code...
            }, TaskContinuationOptions.NotOnFaulted);
            Task.Factory.ContinueWhenAll(tasks, antecedents =>
            {
                foreach (Task t in antecedents)
                {
                    if (t.Status == TaskStatus.Faulted)
                    {
                        // ...process exceptions...
                    }
                }
            });
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
