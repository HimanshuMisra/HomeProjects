﻿using System;
using System.Threading.Tasks;
using System.Threading;

//Using the SpinLock Primitive
namespace TestThread
{
    //lets you lock without incurring the cost of a context switch, at the expense of keeping a thread spinning (uselessly busy)
    class Program34
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static void Main34(string[] args)
        {
            BankAccount account = new BankAccount();
            // create the spinlock
            SpinLock spinlock = new SpinLock();
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
                        bool lockAcquired = false;
                        try
                        {
                            spinlock.Enter(ref lockAcquired);
                            // update the balance
                            account.Balance = account.Balance + 1;
                        }
                        finally
                        {
                            if (lockAcquired) spinlock.Exit();
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
