using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//A Custom Task Scheduler
namespace TestThread
{
    //more than 10 schedulers in ParallelExtensionsExtras
    //https://blogs.msdn.com/b/pfxteam/archive/2010/04/09/9990424.aspx
    class ParallelSchedulerDemo2
    {
        static void Main72a()
        {
            ParallelOptions options = new ParallelOptions();
            // Construct and associate a custom task scheduler
            options.TaskScheduler = new TwoThreadTaskScheduler();

            try
            {
                Parallel.For(
                        0,
                        10,
                        options,
                        (i, localState) =>
                        {
                            Console.WriteLine("i={0}, Task={1}, Thread={2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
                            Console.ReadLine();
                        }
                    );

            }
            // No exception is expected in this example, but if one is still thrown from a task,
            // it will be wrapped in AggregateException and propagated to the main thread.
            catch (AggregateException e)
            {
                Console.WriteLine("An iteration has thrown an exception. THIS WAS NOT EXPECTED.\n{0}", e);
            }
        }

        // This scheduler schedules all tasks on (at most) two threads
        sealed class TwoThreadTaskScheduler : TaskScheduler, IDisposable
        {
            // The runtime decides how many tasks to create for the given set of iterations, loop options, and scheduler's max concurrency level.
            // Tasks will be queued in this collection
            private BlockingCollection<Task> _tasks = new BlockingCollection<Task>();

            // Maintain an array of threads. (Feel free to bump up _n.)
            private readonly int _n = 2;
            private Thread[] _threads;

            public TwoThreadTaskScheduler()
            {
                _threads = new Thread[_n];

                // Create unstarted threads based on the same inline delegate
                for (int i = 0; i < _n; i++)
                {
                    _threads[i] = new Thread(() =>
                    {
                        // The following loop blocks until items become available in the blocking collection.
                        // Then one thread is unblocked to consume that item.
                        foreach (var task in _tasks.GetConsumingEnumerable())
                        {
                            TryExecuteTask(task);
                        }
                    });

                    // Start each thread
                    _threads[i].IsBackground = true;
                    _threads[i].Start();
                }
            }

            // This method is invoked by the runtime to schedule a task
            protected override void QueueTask(Task task)
            {
                _tasks.Add(task);
            }

            // etermines whether the provided Task can be executed synchronously in this call, and if it can, executes it.
            // By returning false, we direct all tasks to be queued up.
            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                return false;
            }

            public override int MaximumConcurrencyLevel { get { return _n; } }

            protected override IEnumerable<Task> GetScheduledTasks()
            {
                return _tasks.ToArray();
            }

            // Dispose is not thread-safe with other members.
            // It may only be used when no more tasks will be queued
            // to the scheduler.  This implementation will block
            // until all previously queued tasks have completed.
            public void Dispose()
            {
                if (_threads != null)
                {
                    _tasks.CompleteAdding();

                    for (int i = 0; i < _n; i++)
                    {
                        _threads[i].Join();
                        _threads[i] = null;
                    }
                    _threads = null;
                    _tasks.Dispose();
                    _tasks = null;
                }
            }


        }
    }

}