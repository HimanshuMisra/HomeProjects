using System;
using System.Threading.Tasks;
using System.Threading;

//Processing antecedent exceptions with the ContinueWhenAll() method is simply a matter
//of checking each antecedent,
namespace TestThread
{
    class Program60
    {
        static void Main60(string[] args)
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
            Task.Factory.ContinueWhenAll(tasks, antecedents =>
            {
                foreach (Task t in antecedents)
                {
                    if (t.Status == TaskStatus.Faulted)
                    {
                        // ...process or propagate...
                    }
                }
                //...task contination code...
            });

            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
