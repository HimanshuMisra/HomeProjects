using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

//it’s not cheap to go via this route . I end up with the Begin method the End method and the IAsyncResult that maintains the context between them.

    class MyComp
    {
        public int EndDivide(IAsyncResult result)
        {   
            MyAsyncResult internalResult = (MyAsyncResult)result;

            if (internalResult.Exception != null)
            {
                throw internalResult.Exception;
            }
            return (internalResult.Result);
        }
        public IAsyncResult BeginDivide(int nominator, int denominator,
          AsyncCallback callback, object state)
        {
            MyAsyncResult result = new MyAsyncResult(callback, state);

            ThreadPool.QueueUserWorkItem(cb =>
            {
                Thread.Sleep(10000);

                try
                {
                    result.Result = nominator / denominator;
                }
                catch (Exception ex)
                {
                    result.Exception = ex;
                }
                result.Complete();
            });

            return (result);
        }  
    }
