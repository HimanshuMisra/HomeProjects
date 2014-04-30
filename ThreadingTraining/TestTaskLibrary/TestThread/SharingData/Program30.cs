using System;
using System.Threading.Tasks;
using System.Threading;

//A TLS Value Factory That Produced Unexpected Results
namespace TestThread
{
    //ThreadLocal provides an overloaded constructor so you can supply a factory delegate that will
    //initialize the isolated data value. This factory delegate is lazily initialized, meaning that it will not be
    //called until the Task calls the ThreadLocal.Value property for the first time.

   class Program30
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
       //Remember also that TLS works on threads and not Tasks, so the value
        //factory will only be called the first time a thread performs one of your Tasks.

        //Started ten Tasks, but the value factory was called to initialize the TLS only four times.(on quad core)
        //As a consequence, the final balance from one Task was carried over to be the initial balance for
        //another, and we ended up with an overall balance that was unexpected.
        static void Main30(string[] args)
        {
            // create the bank account instance
            BankAccount account = new BankAccount();
            // create an array of tasks
            Task<int>[] tasks = new Task<int>[10];
            // create the thread local storage
            ThreadLocal<int> tls = new ThreadLocal<int>(() =>
            {
                Console.WriteLine("Value factory called for value: {0}",
                account.Balance);
                return account.Balance;
            });
            for (int i = 0; i < 10; i++)
            {
                // create a new task
                tasks[i] = new Task<int>(() =>
                {
                    // enter a loop for 1000 balance updates
                    for (int j = 0; j < 1000; j++)
                    {
                        // update the TLS balance
                        tls.Value++;
                    }
                    // return the updated balance
                    return tls.Value;
                });
                // start the new task
                tasks[i].Start();
            }
            // get the result from each task and add it to
            // the balance
            for (int i = 0; i < 10; i++)
            {
                account.Balance += tasks[i].Result;
            }
            // write out the counter value
            Console.WriteLine("Expected value {0}, Balance: {1}",
            10000, account.Balance);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
