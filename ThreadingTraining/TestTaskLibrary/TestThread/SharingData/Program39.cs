﻿using System;
using System.Threading.Tasks;
using System.Threading;

//ReaderWriterLockSlim  : Avoiding Lock Recursion by Using an Upgradable Read Lock
namespace TestThread
{
    //Acquiring the lock on a primitive when you already have a lock is called lock recursion.
    //ReaderWriterLockSlim class doesn’t support lock recursion (potential to create deadlocks)
    //you should use an upgradable read lock, which allows you to read the shared data, perform your test, and safely acquire exclusive write access if you need it.
    class Program39
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static void Main39(string[] args)
        {
            // create the reader-writer lock
            ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
            // create a cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // create some shared data
            int sharedData = 0;
            // create an array of tasks
            Task[] readerTasks = new Task[5];
            for (int i = 0; i < readerTasks.Length; i++)
            {
                // create a new task
                readerTasks[i] = new Task(() =>
                {
                    while (true)
                    {
                        // acqure the read lock
                        rwlock.EnterReadLock();
                        // we now have the lock
                        Console.WriteLine("Read lock acquired - count: {0}",
                        rwlock.CurrentReadCount);
                        // read the shared data
                        Console.WriteLine("Shared data value {0}", sharedData);
                        // wait - slow things down to make the example clear
                        tokenSource.Token.WaitHandle.WaitOne(1000);
                        // release the read lock
                        rwlock.ExitReadLock();
                        Console.WriteLine("Read lock released - count {0}",
                        rwlock.CurrentReadCount);
                        // check for cancellation
                        tokenSource.Token.ThrowIfCancellationRequested();
                    }
                }, tokenSource.Token);
                // start the new task
                readerTasks[i].Start();
            }
            //Only one holder of the upgradable lock is allowed at a time, which means you should partition your
            //requests for locks carefully to have as few requests as possible for upgradable and write locks.
            Task[] writerTasks = new Task[2];
            for (int i = 0; i < writerTasks.Length; i++)
            {
                writerTasks[i] = new Task(() =>
                {
                    while (true)
                    {
                        // acquire the upgradeable lock
                        rwlock.EnterUpgradeableReadLock();
                        // simulate a branch that will require a write
                        if (true)
                        {
                            // acquire the write lock
                            rwlock.EnterWriteLock();
                            // print out a message with the details of the lock
                            Console.WriteLine("Write Lock acquired - waiting readers {0},writers {1}, upgraders {2}",
                            rwlock.WaitingReadCount, rwlock.WaitingWriteCount,
                            rwlock.WaitingUpgradeCount);
                            // modify the shared data
                            sharedData++;
                            // wait - slow down the example to make things clear
                            tokenSource.Token.WaitHandle.WaitOne(1000);
                            // release the write lock
                            rwlock.ExitWriteLock();
                        }
                        // release the upgradable lock
                        rwlock.ExitUpgradeableReadLock();
                        // check for cancellation
                        tokenSource.Token.ThrowIfCancellationRequested();
                    }
                }, tokenSource.Token);
                // start the new task
                writerTasks[i].Start();
            }
            // prompt the user
            Console.WriteLine("Press enter to cancel tasks");
            // wait for the user to press enter
            Console.ReadLine();
            // cancel the tasks
            tokenSource.Cancel();
            try
            {
                // wait for the tasks to complete
                Task.WaitAll(readerTasks);
            }
            catch (AggregateException agex)
            {
                agex.Handle(ex => true);
            }
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
