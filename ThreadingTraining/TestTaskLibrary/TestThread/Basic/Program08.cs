using System;
using System.Threading.Tasks;
using System.Threading;

//Monitoring Cancellation with a Delegate

namespace TestThread
{
    class Program08
    {
        static void Main8(string[] args)
        {
            //You can register a delegate with a CancellationToken, which will be 
            //invoked when the CancellationTokenSource.Cancel() method is called.
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
            // register a cancellation delegate
            token.Register(() =>
            {
                Console.WriteLine(">>>>>> Delegate Invoked\n");
            });
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
