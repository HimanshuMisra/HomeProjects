using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//A Custom Task Scheduler
//However, the ability to customize the TaskScheduler was not provided with PLINQ, 
//since PLINQ requires very strict guarantees from the TaskScheduler, and the fear was that exposing this would be very problematic.

namespace TestThread
{
    class ParallelSchedulerDemo3
    {
        static void Main3(string[] args)
        {
            var tasks = new Task[4];
            var scheduler = new SimpleScheduler();

            using (scheduler)//Automatically invoke dispose when you exit using.
            {

                Task<string> taskS1 = new Task<string>(() =>
                { Write("Running 1 seconds"); Thread.Sleep(1000); return "String value 1.."; });
                tasks[0] = taskS1;

                Task<string> taskS2 = new Task<string>(() =>
                { Write("Running 2 seconds"); Thread.Sleep(2000); return "String value 2.."; });
                tasks[1] = taskS2;

                Task<string> taskS3 = new Task<string>(() =>
                { Write("Running 3 seconds"); Thread.Sleep(3000); return "String value 3.."; });
                tasks[2] = taskS3;

                Task<string> taskS4 = new Task<string>(() =>
                { Write("Running 4 seconds"); Thread.Sleep(4000); return "String value 4.."; });
                tasks[3] = taskS4;

                foreach (var t in tasks)
                {
                    t.Start(scheduler);
                }


                Write("Press any key to quit..");
                Console.ReadKey();

            }
        }
        static void Write(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString() + " on Thread " + Thread.CurrentThread.ManagedThreadId.ToString() + " -- " + msg);

        }
    }

    public sealed class SimpleScheduler : TaskScheduler, IDisposable
    {
        private BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
        private Thread _main = null;

        public SimpleScheduler()
        {
            _main = new Thread(new ThreadStart(this.Main));
        }

        private void Main()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId.ToString());

            //GetConsumingEnumerable returns a Task each time a Task is added to the underlying BlockingCollection. 
            //The Foreach loop breaks when CompleteAdding is invoked on the BlockingCollection.
            foreach (var t in _tasks.GetConsumingEnumerable())
            {
                TryExecuteTask(t);
            }
        }

        /// <summary>
        /// Overriding GetScheduledTasks is required for debugger support.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks.ToArray<Task>();
        }


        protected override void QueueTask(Task task)
        {
            _tasks.Add(task);

            if (!_main.IsAlive) { _main.Start(); }//Start thread if not done so already
        }

        //Attempts to execute the specified task on the current thread.
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }


        #region IDisposable Members

        public void Dispose()
        {
            _tasks.CompleteAdding(); //Drops you out of the thread
        }

        #endregion
    }
}