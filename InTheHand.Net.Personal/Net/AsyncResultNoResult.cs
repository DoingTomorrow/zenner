// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.AsyncResultNoResult
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace InTheHand.Net
{
  internal class AsyncResultNoResult : IAsyncResult
  {
    private const int c_StatePending = 0;
    private const int c_StateCompletedSynchronously = 1;
    private const int c_StateCompletedAsynchronously = 2;
    private readonly AsyncCallback m_AsyncCallback;
    private readonly object m_AsyncState;
    private int m_CompletedState;
    private ManualResetEvent m_AsyncWaitHandle;
    private Exception m_exception;

    public AsyncResultNoResult(AsyncCallback asyncCallback, object state)
    {
      this.m_AsyncCallback = asyncCallback;
      this.m_AsyncState = state;
    }

    public void SetAsCompleted(Exception exception, bool completedSynchronously)
    {
      AsyncResultCompletion completion = AsyncResultNoResult.ConvertCompletion(completedSynchronously);
      this.SetAsCompleted(exception, completion);
    }

    protected static AsyncResultCompletion ConvertCompletion(bool completedSynchronously)
    {
      return !completedSynchronously ? AsyncResultCompletion.IsAsync : AsyncResultCompletion.IsSync;
    }

    public void SetAsCompleted(Exception exception, AsyncResultCompletion completion)
    {
      bool flag = completion == AsyncResultCompletion.IsSync;
      this.m_exception = exception;
      if (Interlocked.Exchange(ref this.m_CompletedState, flag ? 1 : 2) != 0)
        throw new InvalidOperationException("You can set a result only once");
      if (this.m_AsyncWaitHandle != null)
        this.m_AsyncWaitHandle.Set();
      if (this.m_AsyncCallback == null)
        return;
      if (completion != AsyncResultCompletion.MakeAsync)
        this.m_AsyncCallback((IAsyncResult) this);
      else
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.CallbackRunner));
    }

    private void CallbackRunner(object state) => this.m_AsyncCallback((IAsyncResult) this);

    [DebuggerNonUserCode]
    public void EndInvoke()
    {
      if (!this.IsCompleted)
      {
        this.AsyncWaitHandle.WaitOne();
        this.AsyncWaitHandle.Close();
        this.m_AsyncWaitHandle = (ManualResetEvent) null;
      }
      if (this.m_exception != null)
        throw this.m_exception;
    }

    public object AsyncState => this.m_AsyncState;

    public bool CompletedSynchronously => this.m_CompletedState == 1;

    public WaitHandle AsyncWaitHandle
    {
      get
      {
        if (this.m_AsyncWaitHandle == null)
        {
          bool isCompleted = this.IsCompleted;
          ManualResetEvent manualResetEvent = new ManualResetEvent(isCompleted);
          if (Interlocked.CompareExchange<ManualResetEvent>(ref this.m_AsyncWaitHandle, manualResetEvent, (ManualResetEvent) null) != null)
            manualResetEvent.Close();
          else if (!isCompleted && this.IsCompleted)
            this.m_AsyncWaitHandle.Set();
        }
        return (WaitHandle) this.m_AsyncWaitHandle;
      }
    }

    public bool IsCompleted => this.m_CompletedState != 0;
  }
}
