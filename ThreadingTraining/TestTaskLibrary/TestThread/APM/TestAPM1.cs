using System;
using System.IO;
using System.Threading;

public sealed class APM1
{
    public static void MainAPM1()
    {
        byte[] buffer = new byte[100];
        string filename = String.Concat(Environment.SystemDirectory, "\\ntdll.dll");
        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read,
         FileShare.Read, 1024, FileOptions.Asynchronous);

        IAsyncResult result = fs.BeginRead(buffer, 0, buffer.Length, null, null);
        while (!result.IsCompleted)
        {
            // do some work here if the call isn't completed
            // you know, execute a code block or something
            Thread.Sleep(100);
        }
        int numBytes = fs.EndRead(result);
        fs.Close();
        Console.WriteLine("Read {0}  Bytes:", numBytes);
        Console.WriteLine(BitConverter.ToString(buffer));
    }
}

