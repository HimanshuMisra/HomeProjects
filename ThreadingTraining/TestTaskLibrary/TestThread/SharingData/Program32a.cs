using System;
using System.Threading.Tasks;
using System.Threading;

//Torn read
namespace TestThread
{
    /*
     * Reading and writing 64-bit fields is nonatomic on 32-bit environments because it requires 
     * two separate instructions: one for each 32-bit memory location. So, if thread X reads a 
     * 64-bit value while thread Y is updating it, thread X may end up with a bitwise combination 
     * of the old and new values (a torn read).
    */

    class Atomicity
    {
        static int _x, _y;
        static long _z; 

        public static void Test()
        {
            long myLocal;
            //A statement is intrinsically atomic if it executes as a single indivisible instruction on the underlying processor.
            _x = 3;             // Atomic
            _z = 3;             // Nonatomic on 32-bit environs (_z is 64 bits)
            myLocal = _z;       // Nonatomic on 32-bit environs (_z is 64 bits)
            _y += _x;           // Nonatomic (read AND write operation)
            _x++;               // Nonatomic (read AND write operation)
            //The compiler implements unary operators of the kind x++ by reading a variable, processing it, and then writing it back.
        }
    }
    class Program32a
    {
        static void Main32(string[] args)
        {
            // create an array of tasks
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                // create a new task
                tasks[i] = new Task(() =>
                {
                    Atomicity.Test();
                });
                // start the new task
                tasks[i].Start();
            }
            // wait for all of the tasks to complete
            Task.WaitAll(tasks);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
