using System;
using System.Threading.Tasks;
using System.Threading;

//Convergent Isolation with Interlocked.CompareExchange()

//The CompareExchange() method checks to see if a variable has a given value and, 
//if it does, changes the value of variable.
namespace TestThread
{
    class Program33a
    {
        static int data; //<-- shared data

        static void MultiplyXBy(int factor)
        {
            var spinWait = new SpinWait();
            while (true)
            {
                int snapshot1 = data;
                int calc = snapshot1 * factor;
                int snapshot2 = Interlocked.CompareExchange(ref data, calc, snapshot1);
                if (snapshot1 == snapshot2) return;   // No one preempted us.
                spinWait.SpinOnce();
            }
        }

        static void Main33a()
        {
             Task[] tasks = new Task[10];
             for (int i = 0; i < 10; i++)
             {
                 int temp = i;
                 tasks[i] = new Task(() => MultiplyXBy(temp));
                 tasks[i].Start();
             }

            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
