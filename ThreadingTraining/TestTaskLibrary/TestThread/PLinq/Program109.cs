using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

//PLINQ Custom Aggregation
namespace TestThread
{
    class Program109
    {
        static void Main109(string[] args)
        {
            string text = "Let’s suppose this is a really long string";
            var letterFrequencies = new int[26];
            foreach (char c in text)
            {
                int index = char.ToUpper(c) - 'A';
                if (index >= 0 && index <= 26) letterFrequencies[index]++;
            };

            Parallel.ForEach(text,(c)=>
            {
                int index = char.ToUpper(c) - 'A';
                if (index >= 0 && index <= 26) 
                    letterFrequencies[index]++; // concurrency issues on the shared array. 
                //locking around accessing that array would all but kill the potential for parallelization.
            });

            // sequential version using Aggregate
            int[] result = text.Aggregate(new int[26],  // Create the "accumulator"
                (letterFrequencies1, c) =>              // The accumulator, in this case, is an array named letterFrequencies1
                {
                    int index = char.ToUpper(c) - 'A';
                    if (index >= 0 && index <= 26) letterFrequencies1[index]++;
                    return letterFrequencies1;
                });

            //now the parallel version, using PLINQ’s special overload:
            result = text.AsParallel().Aggregate( () => new int[26],             // Create a new local accumulator
                (localFrequencies, c) =>       // Aggregate into the local accumulator
                {
                    int index = char.ToUpper(c) - 'A';
                    if (index >= 0 && index <= 26) localFrequencies[index]++;
                    return localFrequencies;
                },
                // You must also supply a function to indicate how to combine the local and main accumulators.
                (mainFreq, localFreq) =>
                  mainFreq.Zip(localFreq, (f1, f2) => f1 + f2).ToArray(),

                finalResult => finalResult);     // Perform any final transformationon the end result.
        }
        //In the parallel aggregation, we need to do something that we didn't need to in the sequential aggregation: combine the intermediate results (i.e. accumulators).
        void Ex2()
        {
            // create some source data
            int[] sourceData = new int[10000];
            for (int i = 0; i < sourceData.Length; i++)
            {
                sourceData[i] = i;
            }
            // perform a custom aggregation
            double aggregateResult = sourceData.AsParallel().Aggregate(
            // Create a new local accumulator
            0.0,
            // Aggregate into the local accumulator
            (subtotal, item) => subtotal += Math.Pow(item, 2),
                // 3rd function - process the overall total and the per-Task total
            (total, subtotal) => total + subtotal,
                //  Perform any final transformation on the end result
            total => total / 2);
            // write out the result
            Console.WriteLine("Total: {0}", aggregateResult);
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}
