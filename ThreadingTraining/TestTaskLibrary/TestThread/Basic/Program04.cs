using System;
using System.Threading.Tasks;

//Creating Several Tasks Using Task State

namespace TestThread
{
    class Program04
    {
        static void Main4(string[] args)
        {
            string[] messages = { "First task", "Second task","Third task", "Fourth task" };
            foreach (string msg in messages)
            {
                //The only way to pass state to a Task constructor is using Action<object>, 
                //so you must convert or cast explicitly if you need to access the members of a specific type.
                Task myTask = new Task(obj => printMessage((string)obj), msg);
                myTask.Start();
            }
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
        static void printMessage(string message)
        {
            Console.WriteLine("Message: {0}", message);
        }
    }       
}
