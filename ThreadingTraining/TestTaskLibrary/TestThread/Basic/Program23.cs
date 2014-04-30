using System;
using System.Threading.Tasks;
using System.Threading;

//Lazy Task Execution
namespace TestThread
{
    class Program23
    {
        static void Main23(string[] args)
        {
            //You can combine lazy variables with tasks to create a task that is not executed until the lazy variable is read.
            
            // define the function
            Func<string> taskBody = new Func<string>(() =>
            {
                Console.WriteLine("Task body working...");
                return "Task Result";
            });
            // create the lazy variable
            Lazy<Task<string>> lazyData = new Lazy<Task<string>>(() =>Task<string>.Factory.StartNew(taskBody));
            Console.WriteLine("Calling lazy variable");
            Console.WriteLine("Result from task: {0}", lazyData.Value.Result);
            // do the same thing in a single statement
            Lazy<Task<string>> lazyData2 = new Lazy<Task<string>>(
            () => Task<string>.Factory.StartNew(() =>
            {
                Console.WriteLine("Task body working...");
                return "Task Result";
            }));
            Console.WriteLine("Calling second lazy variable");
            Console.WriteLine("Result from task: {0}", lazyData2.Value.Result);
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
