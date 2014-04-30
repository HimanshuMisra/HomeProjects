using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System;

public class DivideCompletedEventArgs : AsyncCompletedEventArgs
{
    public DivideCompletedEventArgs(Exception error, bool cancelled,
      object state)
        : base(error, cancelled, state)
    {

    }
    public int Result
    {
        get;
        internal set;
    }
}
