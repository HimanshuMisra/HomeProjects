using System;
using System.Threading.Tasks;
using System.Threading;

//Using Interlocked.Increment()
namespace TestThread
{
    class Program32
    {
        //All methods on the Interlocked class generates full fence
        class BankAccount
        {
            public int Balance = 0;
        }
        static void Main32(string[] args)
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
                        Interlocked.Increment(ref account.Balance);
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
