// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.SimpleAsyncResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  internal sealed class SimpleAsyncResult : IAsyncResult
  {
    private readonly object _asyncState;
    private bool _completedSynchronously;
    private volatile bool _isCompleted;

    public SimpleAsyncResult(object asyncState) => this._asyncState = asyncState;

    public object AsyncState => this._asyncState;

    public WaitHandle AsyncWaitHandle => (WaitHandle) null;

    public bool CompletedSynchronously => this._completedSynchronously;

    public bool IsCompleted => this._isCompleted;

    public void MarkCompleted(bool completedSynchronously, AsyncCallback callback)
    {
      this._completedSynchronously = completedSynchronously;
      this._isCompleted = true;
      if (callback == null)
        return;
      callback((IAsyncResult) this);
    }
  }
}
