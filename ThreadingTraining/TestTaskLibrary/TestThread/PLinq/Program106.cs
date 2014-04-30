using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

//Combining Ordered and Unordered Queries
namespace TestThread
{
    class Program106
    {
        //AsOrdered as performance hit
        //You can control ordering from one part of a query to the next by combing the AsOrdered() and
        //AsUnordered() extension methods.
        static void Main106(string[] args)
        {
            // create some source data
            int[] sourceData = new int[10000];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = i;
            }
            // define a query that has an ordered subquery
            var result =
            sourceData.AsParallel().AsOrdered()
            .Take(10).AsUnordered()
            .Select(item => new
            {
                sourceValue = item,
                resultValue = Math.Pow(item, 2)
            });
            foreach (var v in result)
            {
                Console.WriteLine("Source {0}, Result {1}",
                v.sourceValue, v.resultValue);
            }
        }
    }
}
