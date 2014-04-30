using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.ComponentModel;


public class MainWindow
{

    static void Main1(string[] args)
    {
        MyComp obj = new MyComp();
        IAsyncResult result = obj.BeginDivide(10, 2, iar =>
        {
            try
            {
                int value = obj.EndDivide(iar);
                Console.WriteLine("Value is {0}", value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed {0}", ex.Message);
            }
            finally
            {
                if (iar is IDisposable)
                {
                    ((IDisposable)iar).Dispose();
                }
            }
        }, null);

        Console.ReadLine();
    }
    static void Main2(string[] args)
    {
        MyComp2 obj = new MyComp2();
        obj.DivideCompleted += (s, e) =>
        {
            if (e.Error != null)
            {
                Console.WriteLine("Exception {0}", e.Error.Message);
            }
            else
            {
                Console.WriteLine("Result {0}", e.Result);
            }
        };

        obj.DivideAsync(10, 2);

        Console.ReadLine();
    }  
    

    static void Main3(string[] args)
    {
        MyComp3 obj = new MyComp3();
        Task<int> asyncCall = obj.TaskDivideAsync(10, 0);
        asyncCall.ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Console.WriteLine("Errored {0}",
                  t.Exception.Flatten().InnerExceptions[0].Message);
            }
            else
            {
                Console.WriteLine("Result {0}", t.Result);
            }
        });

        Task<int> syncCall = obj.TaskDivideAsync(10, 2);
        Console.WriteLine("Sync call {0}", syncCall.Result);

        Console.ReadLine();
    }
    public static void Main()
    {
        fun2();
       
        Console.ReadLine();
    }
    public static async void fun2()
    {
        MyComp4 o = new MyComp4();
        await o.TaskDivideAsync(10, 2);
    }
    public static void Main5()
    {
        fun();
        Console.ReadLine();
    }
    public static async void fun()
    {
        ASCIIEncoding uniencoding = new ASCIIEncoding();
        string filename = @"e:\abc.txt";
        byte[] result;

        using (FileStream SourceStream = File.Open(filename, FileMode.Open))
        {
            result = new byte[SourceStream.Length];
            await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
        }

        Console.WriteLine(uniencoding.GetString(result));
    }
}