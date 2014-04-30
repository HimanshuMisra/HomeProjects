using System;
using System.Threading.Tasks;

//Applying the lock Keyword
namespace TestThread.SharingData
{
    //Microsoft’s Framework Class Library (FCL) guarantees that all static methods are thread safe.
    //On the other hand, the FCL does not guarantee that instance methods are thread safe
    class Program31
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static void Main31(string[] args)
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
                        lock (lockObj)
                        {
                            // update the balance
                            account.Balance = account.Balance + 1;
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
