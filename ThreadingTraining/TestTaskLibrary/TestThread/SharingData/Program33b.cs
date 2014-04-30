using System;
using System.Threading.Tasks;
using System.Threading;

//Convergent Isolation with Interlocked.CompareExchange()

//The CompareExchange() method checks to see if a variable has a given value and, 
//if it does, changes the value of variable.
namespace TestThread
{
    class Stock
    {
        public int Rate{ get; set; }
    }
    class Program33b
    {
        static Stock stock= new Stock();

        static void LockFreeUpdate<T>(ref T field, Func<T, T> updateFunction) where T : class
        {
            var spinWait = new SpinWait();
            while (true)
            {
                T snapshot1 = field;
                T calc = updateFunction(snapshot1);
                T snapshot2 = Interlocked.CompareExchange(ref field, calc, snapshot1);
                if (snapshot1 == snapshot2) return;
                spinWait.SpinOnce();
            }
        }

        static void Main33b(string[] args)
        {
            LockFreeUpdate(ref stock, s => { s.Rate += 100; return s; });
            Console.WriteLine(stock.Rate);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
