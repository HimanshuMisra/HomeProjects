using System;
using System.Threading.Tasks;
using System.Threading;

//Lock-free updates
//Convergent Isolation with Interlocked.CompareExchange()

//The CompareExchange() method checks to see if a variable has a given value and, 
//if it does, changes the value of variable.
namespace TestThread
{
    class Program33
    {
        class BankAccount
        {
            public int Balance = 0;
        }
        static void Main33(string[] args)
        {
            BankAccount account = new BankAccount();
            // create an array of tasks
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                // create a new task
                tasks[i] = new Task(() =>
                {
                    // get a local copy of the shared data
                    int startBalance = account.Balance;
                    // create a local working copy of the shared data
                    int localBalance = startBalance;
                    // enter a loop for 1000 balance updates
                    for (int j = 0; j < 1000; j++)
                    {
                        // update the local balance
                        localBalance++;
                    }
                    // check to see if the shared data has changed since we started
                    // and if not, then update with our local value
                    int sharedData = Interlocked.CompareExchange(
                    ref account.Balance, localBalance, startBalance);
                    if (sharedData == startBalance)
                    {
                        Console.WriteLine("Shared data updated OK");
                    }
                    else
                    {
                        Console.WriteLine("Shared data changed");
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
/*
 * Take a “snapshot” of x into a local variable
 * Calculate the new value
 * Write the calculated value back if the snapshot is still up-to-date using Interlocked.CompareExchange
 * If the snapshot was stale, spin and return to step 1
*/
