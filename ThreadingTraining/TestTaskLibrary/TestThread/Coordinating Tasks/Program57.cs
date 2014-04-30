using System;
using System.Threading.Tasks;
using System.Threading;

//Cancelling Continuations
namespace TestThread
{
    class Program57
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static void Main57(string[] args)
        {
            // create a cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // create the antecedent task
            Task task = new Task(() =>
            {
                // write out a message
                Console.WriteLine("Antecedent running");
                // wait indefinately on the token wait handle
                tokenSource.Token.WaitHandle.WaitOne();
                // handle the cancellation exception
                tokenSource.Token.ThrowIfCancellationRequested();
            }, tokenSource.Token);
            // create a selective continuation
            Task neverScheduled = task.ContinueWith(antecedent =>
            {
                Console.WriteLine("This task will never be scheduled");
            }, tokenSource.Token); //created with the same CancellationToken as the antecedent and so is never scheduled to be run
            // create a bad selective contination
            Task badSelective = task.ContinueWith(antecedent =>
            {
                // write out a message
                Console.WriteLine("This task will never be scheduled");
            }, tokenSource.Token, TaskContinuationOptions.OnlyOnCanceled,
            TaskScheduler.Current); //it is created using the same CancellationToken as the antecedent, so the options and the token can never be in a state where the Task will be scheduled.
            // create a good selective contiuation
            Task continuation = task.ContinueWith(antecedent =>
            {
                // Tasks that rely on the OnlyOnCanceled value should not share a CancellationToken with their antecedent.
                Console.WriteLine("Continuation running");
            }, TaskContinuationOptions.OnlyOnCanceled);
            // start the task
            task.Start();
            // prompt the user so they can cancel the token
            Console.WriteLine("Press enter to cancel token");
            Console.ReadLine();
            // cancel the token source
            tokenSource.Cancel();
            // wait for the good continuation to complete
            continuation.Wait();
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
