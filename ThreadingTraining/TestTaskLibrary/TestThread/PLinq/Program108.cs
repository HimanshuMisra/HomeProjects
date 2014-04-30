using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

//Basics of aggregation in linq
namespace TestThread
{
    class Program108
    {
        // Aggregate as a generalized version of Sum, Average, Min, and Max 
        //lets you plug in a custom accumulation algorithm for implementing unusual aggregations. 
        static void Main108(string[] args)
        {
            int[] numbers = { 2, 3, 4 };
            int sum = numbers.Aggregate(0, // seed, from which accumulation starts
                                (total, n) => total + n);   // an expression to update the accumulated value, given a fresh element


            int sum2 = numbers.Aggregate((total, n) => total + n);   //first element becomes the implicit seed

            int x = numbers.Aggregate(0, (prod, n) => prod * n);   // 0*1*2*3 = 0
            int y = numbers.Aggregate((prod, n) => prod * n);   //   1*2*3 = 6
        }
    }
}
