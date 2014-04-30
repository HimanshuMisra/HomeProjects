using System;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

//Orphaned Locks
namespace TestThread
{
    //An orphaned lock is one that has been acquired but, because of an exception or 
    //poor programming, will never be released.
    class Program48
    {
        static void Main48(string[] args)
        {
            // create the sync primitive
            Mutex mutex = new Mutex();
            // create a cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // create a task that acquires and releases the mutex
            Task task1 = new Task(() =>
            {
                while (true)
                {
                    mutex.WaitOne();
                    Console.WriteLine("Task 1 acquired mutex");
                    // wait for 500ms
                    tokenSource.Token.WaitHandle.WaitOne(500);
                    // exit the mutex
                    mutex.ReleaseMutex();
                    Console.WriteLine("Task 1 released mutex");
                }
            }, tokenSource.Token);
            // create a task that acquires and then abandons the mutex
            Task task2 = new Task(() =>
            {
                // wait for 2 seconds to let the other task run
                tokenSource.Token.WaitHandle.WaitOne(2000);
                // acquire the mutex
                mutex.WaitOne();
                Console.WriteLine("Task 2 acquired mutex");
                // abandon the mutex
                throw new Exception("Abandoning Mutex");
            }, tokenSource.Token);
            // start the tasks
            task1.Start();
            task2.Start();
            // put the main thread to sleep
            tokenSource.Token.WaitHandle.WaitOne(3000);
            // wait for task 2
            try
            {
                task2.Wait();
            }
            catch (AggregateException ex)
            {
                ex.Handle((inner) =>
                {
                    Console.WriteLine(inner);
                    return true;
                });
            }
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
