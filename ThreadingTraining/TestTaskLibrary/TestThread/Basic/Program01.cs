using System;
using System.Threading.Tasks;

//Hello Task
namespace TestThread
{
    class Program01
    {

        static void Main01(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Hello World");
                Console.ReadLine();
            });
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
