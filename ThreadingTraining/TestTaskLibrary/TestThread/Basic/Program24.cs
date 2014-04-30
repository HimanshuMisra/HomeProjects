using System;
using System.Threading.Tasks;
using System.Threading;

//Task Dependency Deadlock
namespace TestThread
{
    class Program24
    {
        static void Main24(string[] args)
        {
            //If two or more Tasks depend on each other to complete, none can move forward without the others, so deadlock
            // define an array to hold the Tasks
            Task<int>[] tasks = new Task<int>[2];
            // create and start the first task
            tasks[0] = Task.Factory.StartNew(() =>
            {
                // get the result of the other task,
                // add 100 to it and return it as the result
                return tasks[1].Result + 100;
            });
            // create and start the second task
            tasks[1] = Task.Factory.StartNew(() =>
            {
                // get the result of the other task,
                // add 100 to it and return it as the result
                return tasks[0].Result + 100;
            });
            // wait for the tasks to complete
            Task.WaitAll(tasks);
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
