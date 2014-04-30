using System;
using System.Threading.Tasks;
using System.Threading;

//Using TLS
namespace TestThread
{
    //ThreadLocal<T> is new to Framework 4.0. It provides thread-local storage for both static and instance fields — and allows you to specify default values.

    //create a ThreadLocal<int> with a default value of 3 for each thread:
    //static ThreadLocal<int> _x = new ThreadLocal<int> (() => 3);

   class Program29
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static ThreadLocal<int> tls;

        static void Main29(string[] args)
        {
            // create the bank account instance
            BankAccount account = new BankAccount();
            // create an array of tasks
            Task<int>[] tasks = new Task<int>[10];
            // create the thread local storage
            tls = new ThreadLocal<int>();
            for (int i = 0; i < 10; i++)
            {
                // create a new task
                tasks[i] = new Task<int>((stateObject) =>
                {
                    // get the state object and use it
                    // to set the TLS data
                    tls.Value = (int)stateObject;
                    // enter a loop for 1000 balance updates
                    for (int j = 0; j < 1000; j++)
                    {
                        // update the TLS balance
                        //tls.Value++;
                        fun();
                    }
                    // return the updated balance
                    return tls.Value;
                }, account.Balance);
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
        static void fun()
        {
            tls.Value++;
        }
   }

}
