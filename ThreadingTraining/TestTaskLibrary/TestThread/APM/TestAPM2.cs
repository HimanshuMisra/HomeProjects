// (code has been slightly revised)
using System;
using System.IO;
using System.Threading;

public static class ProgramAPM2
{
    // The array is static so it can be accessed by Main and ReadIsDone
    private static Byte[] s_data = new Byte[100];

    public static void Main45()
    {
        ReadMultipleFiles(@"C:\Windows\System32\config.NT", @"C:\point.cs");
        APMCallbackUsingAnonymousMethod();
        // Show the ID of the thread executing Main
        Console.WriteLine("Main thread ID={0}",
           Thread.CurrentThread.ManagedThreadId);

        //ReadMultipleFiles(@"C:\Windows\System32\Config.NT", @"c:\Point.cs");
        // Open the file indicating asynchronous I/O
        FileStream fs = new FileStream(@"C:\Windows\System32\config.NT", FileMode.Open,
           FileAccess.Read, FileShare.Read, 1024,
           FileOptions.Asynchronous);

        // Initiate an asynchronous read operation against the FileStream
        // Pass the FileStream (fs) to the callback method (ReadIsDone)
        fs.BeginRead(s_data, 0, s_data.Length, ReadIsDone, fs);

        // Executing some other code here would be useful...
        // For this demo, I'll just suspend the primary thread
        Console.ReadLine();
    }

    private static void ReadIsDone(IAsyncResult ar)
    {
        // Show the ID of the thread executing ReadIsDone
        Console.WriteLine("ReadIsDone thread ID={0}",
           Thread.CurrentThread.ManagedThreadId);

        // Extract the FileStream (state) out of the IAsyncResult object
        FileStream fs = (FileStream)ar.AsyncState;

        // Get the result
        Int32 bytesRead = fs.EndRead(ar);

        // No other operations to do, close the file
        fs.Close();

        // Now, it is OK to access the byte array and show the result.
        Console.WriteLine("Number of bytes read={0}", bytesRead);
        Console.WriteLine(BitConverter.ToString(s_data, 0, bytesRead));
    }

    private static void APMCallbackUsingAnonymousMethod()
    {
        // Show the ID of the thread executing Main
        Console.WriteLine("Main thread ID={0}",
           Thread.CurrentThread.ManagedThreadId);

        // Open the file indicating asynchronous I/O
        FileStream fs = new FileStream(@"C:\Windows\System32\config.NT", FileMode.Open,
           FileAccess.Read, FileShare.Read, 1024,
           FileOptions.Asynchronous);

        Byte[] data = new Byte[100];

        // Initiate an asynchronous read operation against the FileStream
        // Pass the FileStream (fs) to the callback method (ReadIsDone)
        fs.BeginRead(data, 0, data.Length,
           delegate(IAsyncResult ar)
           {
               // Show the ID of the thread executing ReadIsDone
               Console.WriteLine("ReadIsDone thread ID={0}",
                  Thread.CurrentThread.ManagedThreadId);

               // Get the result
               Int32 bytesRead = fs.EndRead(ar);

               // No other operations to do, close the file
               fs.Close();

               // Now, it is OK to access the byte array and show the result.
               Console.WriteLine("Number of bytes read={0}", bytesRead);
               Console.WriteLine(BitConverter.ToString(data, 0, bytesRead));

           }, null);

        // Executing some other code here would be useful...
        // For this demo, I'll just suspend the primary thread
        Console.ReadLine();
    }

    private static void ReadMultipleFiles(params String[] pathnames)
    {
        for (Int32 n = 0; n < pathnames.Length; n++)
        {
            // Open the file indicating asynchronous I/O
            Stream stream = new FileStream(pathnames[n], FileMode.Open,
               FileAccess.Read, FileShare.Read, 1024,
               FileOptions.Asynchronous);

            // Initiate an asynchronous read operation against the Stream
            new AsyncStreamRead(stream, 100,
               delegate(Byte[] data)
               {
                   // Process the data.
                   Console.WriteLine("Number of bytes read={0}", data.Length);
                   Console.WriteLine(BitConverter.ToString(data));
               });
        }

        // All streams have been opened and all read requests have been
        // queued; they are all executing concurrently and they will be
        // processed as they complete!
        // The primary thread could do other stuff here if it wants to...
        // For this demo, I'll just suspend the primary thread
        Console.ReadLine();
    }

    private delegate void StreamBytesRead(Byte[] streamData);

    private sealed class AsyncStreamRead
    {
        private Stream m_stream;
        private Byte[] m_data;
        StreamBytesRead m_callback;

        public AsyncStreamRead(Stream stream, Int32 numBytes,
           StreamBytesRead callback)
        {
            m_stream = stream;
            m_data = new Byte[numBytes];
            m_callback = callback;

            // Initiate an asynchronous read operation against the Stream
            stream.BeginRead(m_data, 0, numBytes, ReadIsDone, null);
        }

        // Called when IO operation completes
        private void ReadIsDone(IAsyncResult ar)
        {
            Int32 numBytesRead = m_stream.EndRead(ar);

            // No other operations to do, close the stream
            m_stream.Close();

            // Resize the array to save space
            Array.Resize(ref m_data, numBytesRead);

            // Call the application's callback method
            m_callback(m_data);
        }
    }
}


