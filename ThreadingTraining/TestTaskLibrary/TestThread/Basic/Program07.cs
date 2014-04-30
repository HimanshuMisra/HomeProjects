using System;
using System.Threading.Tasks;
using System.Threading;

//Cancelling a Task
namespace TestThread
{
    class Program07
    {
        static void Main7(string[] args)
        {
            // create the cancellation token source
            CancellationTokenSource tokenSource= new CancellationTokenSource();
            // create the cancellation token
            CancellationToken token = tokenSource.Token;
            // create the task
            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task cancel detected");
                        throw new OperationCanceledException(token);
                    }
                    else
                    {
                        Console.WriteLine("Int value {0}", i);
                    }
                }
            }, token);
            //Passing the cancellation token to the Task constructor allows the .NET Framework 
            //to avoid starting tasks that rely on tokens that have already been cancelled.

            // wait for input before we start the task
            Console.WriteLine("Press enter to start task");
            Console.WriteLine("Press enter again to cancel task");
            Console.ReadLine();
            // start the task
            task.Start();
            // read a line from the console.
            Console.ReadLine();
            // cancel the task
            Console.WriteLine("Cancelling task");
            tokenSource.Cancel();
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }       
}
/*
 * A CancellationToken is a struct that represents a ‘potential request for cancellation’. 
 * A CancellationTokenSource is a class that provides the mechanism for initiating a cancellation request and it has a Token property for obtaining an associated token.
 It would have been natural to combine these two classes into one, but this design allows the two key operations (initiating a cancellation request vs. observing and responding to cancellation) to be cleanly separated. 
 * In particular, methods that take only a CancellationToken can observe a cancellation request but cannot initiate one.
 */


