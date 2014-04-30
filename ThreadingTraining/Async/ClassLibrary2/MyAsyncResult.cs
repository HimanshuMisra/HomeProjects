using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

class MyAsyncResult : IAsyncResult, IDisposable
{
    public MyAsyncResult(AsyncCallback callback, object state)
    {
        this._callback = callback;
        this._asyncState = state;
        this._waitHandle = new Lazy<ManualResetEventSlim>(true);
    }
    ~MyAsyncResult()
    {
        Dispose(false);
    }
    internal int Result
    {
        get;
        set;
    }
    internal Exception Exception
    {
        get;
        set;
    }
    public object AsyncState
    {
        get { return (_asyncState); }
    }
    public WaitHandle AsyncWaitHandle
    {
        get
        {
            return (this._waitHandle.Value.WaitHandle);
        }
    }
    public bool CompletedSynchronously
    {
        get { return (false); }
    }
    public bool IsCompleted
    {
        get
        {
            return (this._isCompleted);
        }
        internal set
        {
            this._isCompleted = value;
        }
    }
    internal void Complete()
    {
        this._isCompleted = true;

        if (this._waitHandle.IsValueCreated)
        {
            this._waitHandle.Value.Set();
        }
        if (this._callback != null)
        {
            this._callback(this);
        }
    }
    void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this._waitHandle.IsValueCreated)
            {
                this._waitHandle.Value.Dispose();
            }
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    object _asyncState;
    AsyncCallback _callback;
    volatile Exception _exception;
    volatile bool _isCompleted;
    Lazy<ManualResetEventSlim> _waitHandle;
}  

