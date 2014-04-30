using System;
using System.Threading.Tasks;
using System.Threading;

//Unhandled Exceptions in Continuation Chains
namespace TestThread
{
    //The second-generation Task throws an exception, but the third-generation Task still runs.
    class Program58
    {
        class BankAccount
        {
            public int Balance
            {
                get;
                set;
            }
        }
        static void Main58(string[] args)
        {
            // create a first generation task
            Task gen1 = new Task(() =>
            {
                // write out a message
                Console.WriteLine("First generation task");
            });
            // create a second-generation task
            Task gen2 = gen1.ContinueWith(antecedent =>
            {
                // write out a message
                Console.WriteLine("Second generation task - throws exception");
                throw new Exception();
            });
            // create a third-generation task
            Task gen3 = gen2.ContinueWith(antecedent =>
            {
                // write out a message
                Console.WriteLine("Third generation task");
            });
            // start the first gen task
            gen1.Start();
            // wait for the last task in the chain to complete
            gen3.Wait();
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
