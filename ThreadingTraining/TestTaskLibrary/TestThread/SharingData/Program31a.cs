using System;
using System.Threading.Tasks;
using System.Threading;

//Applying the Monitor instead of lock
namespace TestThread
{
    class Program31a
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static void Main31a(string[] args)
        {
            // create the bank account instance
            BankAccount account = new BankAccount();
            // create an array of tasks
            Task[] tasks = new Task[10];
            // create the lock object
            object lockObj = new object();
            for (int i = 0; i < 10; i++)
            {
                // create a new task
                tasks[i] = new Task(() =>
                {
                    // enter a loop for 1000 balance updates
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockAcquired=false;
                        try {
                            Monitor.Enter(lockObj, ref lockAcquired);
                            // update the balance
                            account.Balance = account.Balance + 1;
                        } finally {
                            if (lockAcquired) Monitor.Exit(lockObj);
                        }   
                    }
                });
                // start the new task
                tasks[i].Start();
            }
            // wait for all of the tasks to complete
            Task.WaitAll(tasks);
            // write out the counter value
            Console.WriteLine("Expected value {0}, Balance: {1}",
            10000, account.Balance);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
