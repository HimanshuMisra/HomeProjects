using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;

//Using Custom dynamic Partitioning
namespace TestThread
{
    class Program104a
    {
        static void Main104a(string[] args)
        {
            var nums = Enumerable.Range(0, 10000).ToArray();
            OrderableListPartitioner<int> partitioner = new OrderableListPartitioner<int>(nums);

            // Use with Parallel.ForEach
            Parallel.ForEach(partitioner, (i) => Console.WriteLine(i));
            
            // Use with PLINQ
            var query = from num in partitioner.AsParallel()
                        where num % 2 == 0
                        select num;

            foreach (var v in query)
                Console.WriteLine(v);
        
            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }   
    }
    //
    // An orderable dynamic partitioner for lists
    //
    class OrderableListPartitioner<TSource> : OrderablePartitioner<TSource>
    {
        private readonly IList<TSource> m_input;

        public OrderableListPartitioner(IList<TSource> input)
            : base(true, false, true)
        {
            m_input = input;
        }

        // Must override to return true.
        public override bool SupportsDynamicPartitions
        {
            get
            {
                return true;
            }
        }

        public override IList<IEnumerator<KeyValuePair<long, TSource>>>
            GetOrderablePartitions(int partitionCount)
        {
            Console.WriteLine("GetOrderablePartitions {0}", partitionCount);

            var dynamicPartitions = GetOrderableDynamicPartitions();
            //var partitions =new IEnumerator<KeyValuePair<long, TSource>>[partitionCount];
            IList<IEnumerator<KeyValuePair<long, TSource>>> partitionsList
            = new List<IEnumerator<KeyValuePair<long, TSource>>>();
            
            for (int i = 0; i < partitionCount; i++)
            {
                //partitions[i] = dynamicPartitions.GetEnumerator();
                partitionsList.Add(dynamicPartitions.GetEnumerator());
            }
            //return partitions;
            return partitionsList;
        }

        public override IEnumerable<KeyValuePair<long, TSource>>
            GetOrderableDynamicPartitions()
        {
            return new ListDynamicPartitions(m_input);
        }

        private class ListDynamicPartitions
            : IEnumerable<KeyValuePair<long, TSource>>
        {
            private IList<TSource> m_input;
            private int m_pos = 0;

            internal ListDynamicPartitions(IList<TSource> input)
            {
                m_input = input;
            }

            public IEnumerator<KeyValuePair<long, TSource>> GetEnumerator()
            {
                while (true)
                {
                    // Each task gets the next item in the list. The index is 
                    // incremented in a thread-safe manner to avoid races.
                    int elemIndex = Interlocked.Increment(ref m_pos) - 1;

                    if (elemIndex >= m_input.Count)
                    {
                        yield break;
                    }

                    yield return new KeyValuePair<long, TSource>(
                        elemIndex, m_input[elemIndex]);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return
                   ((IEnumerable<KeyValuePair<long, TSource>>)this)
                   .GetEnumerator();
            }
        }
    }

}
