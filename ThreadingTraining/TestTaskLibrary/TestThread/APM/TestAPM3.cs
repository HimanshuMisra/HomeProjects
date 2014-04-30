using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public sealed class APM3
{
    public static void Mainapm3()
    {
        byte[] buffer = new byte[100];
        string filename = String.Concat(Environment.SystemDirectory, "\\ntdll.dll");
        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read,
         FileShare.Read, 1024, FileOptions.Asynchronous);

        Task<int> t = Task<int>.Factory.FromAsync(
            fs.BeginRead, fs.EndRead, buffer, 0, buffer.Length, null, 
            TaskCreationOptions.AttachedToParent);
        
       
        
            t.ContinueWith(completedTask =>
        {
            fs.Close();
            Console.WriteLine(BitConverter.ToString(buffer));
        });

        //IAsyncResult result = fs.BeginRead(buffer, 0, buffer.Length, null, null);
        //numBytes = fs.EndRead(result);

        Console.ReadLine();
        }
}

