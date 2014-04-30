using System;
using System.Threading.Tasks;
using System.Threading;

//Custom Escalation Policy
namespace TestThread
{
    class Program22
    {
        static void Main22(string[] args)
        {
            //If you don’t catch AggregateException when you call a trigger method, the .NET Framework will escalate
            //the exceptions. By default, this means that the unhandled exceptions will be thrown again when your
            //Task is finalized and cause your program to be terminated. Because you don’t know when the finalizer
            //will be called, you won’t be able to predict when this will happen.
            //The .NET Framework calls the event handler each time an unhandled exception is escalated.

            TaskScheduler.UnobservedTaskException
        += (object sender, UnobservedTaskExceptionEventArgs excArgs) =>
        {
            Console.WriteLine("Exception.Message: {0}\n",
                excArgs.Exception.Message);
            Console.WriteLine("Exception.InnerException.Message: {0}\n",
                excArgs.Exception.InnerException.Message);
            excArgs.SetObserved();
        };
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            LaunchTask(tokenSource.Token);
            Thread.Sleep(5000);

            tokenSource.Cancel();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("Enter to finish");
            Console.ReadLine();
        }

        static void LaunchTask(CancellationToken token)
        {
            Task t = Task.Factory.StartNew((s) =>
            {
                for (int i = 0; i < 2048; i++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        while (true)
                            token.WaitHandle.WaitOne(200);
                    }, token);
                }
                throw new Exception("Oops!");
            },token);
        }


    }
}
