using System;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Contexts;

//Using Declarative Synchronization
namespace TestThread
{
    /*
     The problem with using the Synchronization attribute is that every field and method of your class,
even if they don’t modify shared data, becomes synchronized using a single lock, and this can cause a
performance problem. Declarative synchronization is a heavy-handed approach to avoiding data races
and should be used with caution. */

    class Program37
    {
        [Synchronization]
        class BankAccount : ContextBoundObject
        {
            private int balance = 0;
            public void IncrementBalance()
            {
                balance++;
            }
            public int GetBalance()
            {
                return balance;
            }
        }
        static void Main37(string[] args)
        {
            // create the bank account instance
            BankAccount account = new BankAccount();
            // create an array of tasks
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                // create a new task
                tasks[i] = new Task(() =>
                {
                    // enter a loop for 1000 balance updates
                    for (int j = 0; j < 1000; j++)
                    {
                        // update the balance
                        account.IncrementBalance();
                    }
                });
                // start the new task
                tasks[i].Start();
            }
            // wait for all of the tasks to complete
            Task.WaitAll(tasks);
            // write out the counter value
            Console.WriteLine("Expected value {0}, Balance: {1}",
            10000, account.GetBalance());
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
