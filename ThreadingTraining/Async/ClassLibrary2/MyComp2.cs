using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.ComponentModel;

//The tax isn’t as bad as the APM version but it’s still not as good as it could be.
    public class MyComp2
    {

        public  event EventHandler<DivideCompletedEventArgs> DivideCompleted;

        public void DivideAsync(int nominator, int denominator, object state = null)
        {
            ThreadPool.QueueUserWorkItem(cb =>
            {
                Thread.Sleep(10000);
                int? result = null;
                Exception exception = null;

                try
                {
                    result = nominator / denominator;
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                DivideCompletedEventArgs args =
                  new DivideCompletedEventArgs(exception, false, state)
                  {
                      Result = result ?? 0
                  };

                var handler = DivideCompleted;

                if (handler != null)
                {
                    handler(null, args);
                }
            });
        }
    }

