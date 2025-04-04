// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.AsyncResultWrapper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Diagnostics;
using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  [DebuggerNonUserCode]
  internal static class AsyncResultWrapper
  {
    private static Func<AsyncVoid> MakeVoidDelegate(Action action)
    {
      return (Func<AsyncVoid>) (() =>
      {
        action();
        return new AsyncVoid();
      });
    }

    private static EndInvokeDelegate<AsyncVoid> MakeVoidDelegate(EndInvokeDelegate endDelegate)
    {
      return (EndInvokeDelegate<AsyncVoid>) (ar =>
      {
        endDelegate(ar);
        return new AsyncVoid();
      });
    }

    public static IAsyncResult Begin<TResult>(
      AsyncCallback callback,
      object state,
      BeginInvokeDelegate beginDelegate,
      EndInvokeDelegate<TResult> endDelegate)
    {
      return AsyncResultWrapper.Begin<TResult>(callback, state, beginDelegate, endDelegate, (object) null);
    }

    public static IAsyncResult Begin<TResult>(
      AsyncCallback callback,
      object state,
      BeginInvokeDelegate beginDelegate,
      EndInvokeDelegate<TResult> endDelegate,
      object tag)
    {
      return AsyncResultWrapper.Begin<TResult>(callback, state, beginDelegate, endDelegate, tag, -1);
    }

    public static IAsyncResult Begin<TResult>(
      AsyncCallback callback,
      object state,
      BeginInvokeDelegate beginDelegate,
      EndInvokeDelegate<TResult> endDelegate,
      object tag,
      int timeout)
    {
      AsyncResultWrapper.WrappedAsyncResult<TResult> wrappedAsyncResult = new AsyncResultWrapper.WrappedAsyncResult<TResult>(beginDelegate, endDelegate, tag);
      wrappedAsyncResult.Begin(callback, state, timeout);
      return (IAsyncResult) wrappedAsyncResult;
    }

    public static IAsyncResult Begin(
      AsyncCallback callback,
      object state,
      BeginInvokeDelegate beginDelegate,
      EndInvokeDelegate endDelegate)
    {
      return AsyncResultWrapper.Begin(callback, state, beginDelegate, endDelegate, (object) null);
    }

    public static IAsyncResult Begin(
      AsyncCallback callback,
      object state,
      BeginInvokeDelegate beginDelegate,
      EndInvokeDelegate endDelegate,
      object tag)
    {
      return AsyncResultWrapper.Begin(callback, state, beginDelegate, endDelegate, tag, -1);
    }

    public static IAsyncResult Begin(
      AsyncCallback callback,
      object state,
      BeginInvokeDelegate beginDelegate,
      EndInvokeDelegate endDelegate,
      object tag,
      int timeout)
    {
      return AsyncResultWrapper.Begin<AsyncVoid>(callback, state, beginDelegate, AsyncResultWrapper.MakeVoidDelegate(endDelegate), tag, timeout);
    }

    public static IAsyncResult BeginSynchronous<TResult>(
      AsyncCallback callback,
      object state,
      Func<TResult> func)
    {
      return AsyncResultWrapper.BeginSynchronous<TResult>(callback, state, func, (object) null);
    }

    public static IAsyncResult BeginSynchronous<TResult>(
      AsyncCallback callback,
      object state,
      Func<TResult> func,
      object tag)
    {
      AsyncResultWrapper.WrappedAsyncResult<TResult> wrappedAsyncResult = new AsyncResultWrapper.WrappedAsyncResult<TResult>((BeginInvokeDelegate) ((asyncCallback, asyncState) =>
      {
        SimpleAsyncResult simpleAsyncResult = new SimpleAsyncResult(asyncState);
        simpleAsyncResult.MarkCompleted(true, asyncCallback);
        return (IAsyncResult) simpleAsyncResult;
      }), (EndInvokeDelegate<TResult>) (_ => func()), tag);
      wrappedAsyncResult.Begin(callback, state, -1);
      return (IAsyncResult) wrappedAsyncResult;
    }

    public static IAsyncResult BeginSynchronous(
      AsyncCallback callback,
      object state,
      Action action)
    {
      return AsyncResultWrapper.BeginSynchronous(callback, state, action, (object) null);
    }

    public static IAsyncResult BeginSynchronous(
      AsyncCallback callback,
      object state,
      Action action,
      object tag)
    {
      return AsyncResultWrapper.BeginSynchronous<AsyncVoid>(callback, state, AsyncResultWrapper.MakeVoidDelegate(action), tag);
    }

    public static TResult End<TResult>(IAsyncResult asyncResult)
    {
      return AsyncResultWrapper.End<TResult>(asyncResult, (object) null);
    }

    public static TResult End<TResult>(IAsyncResult asyncResult, object tag)
    {
      return AsyncResultWrapper.WrappedAsyncResult<TResult>.Cast(asyncResult, tag).End();
    }

    public static void End(IAsyncResult asyncResult)
    {
      AsyncResultWrapper.End(asyncResult, (object) null);
    }

    public static void End(IAsyncResult asyncResult, object tag)
    {
      AsyncResultWrapper.End<AsyncVoid>(asyncResult, tag);
    }

    [DebuggerNonUserCode]
    private sealed class WrappedAsyncResult<TResult> : IAsyncResult
    {
      private const int AsyncStateNone = 0;
      private const int AsyncStateBeginUnwound = 1;
      private const int AsyncStateCallbackFired = 2;
      private int _asyncState;
      private readonly BeginInvokeDelegate _beginDelegate;
      private readonly object _beginDelegateLockObj = new object();
      private readonly EndInvokeDelegate<TResult> _endDelegate;
      private readonly SingleEntryGate _endExecutedGate = new SingleEntryGate();
      private readonly SingleEntryGate _handleCallbackGate = new SingleEntryGate();
      private readonly object _tag;
      private IAsyncResult _innerAsyncResult;
      private AsyncCallback _originalCallback;
      private volatile bool _timedOut;
      private Timer _timer;

      public WrappedAsyncResult(
        BeginInvokeDelegate beginDelegate,
        EndInvokeDelegate<TResult> endDelegate,
        object tag)
      {
        this._beginDelegate = beginDelegate;
        this._endDelegate = endDelegate;
        this._tag = tag;
      }

      public object AsyncState => this._innerAsyncResult.AsyncState;

      public WaitHandle AsyncWaitHandle => (WaitHandle) null;

      public bool CompletedSynchronously { get; private set; }

      public bool IsCompleted => this._timedOut || this._innerAsyncResult.IsCompleted;

      public void Begin(AsyncCallback callback, object state, int timeout)
      {
        this._originalCallback = callback;
        lock (this._beginDelegateLockObj)
        {
          this._innerAsyncResult = this._beginDelegate(new AsyncCallback(this.HandleAsynchronousCompletion), state);
          this.CompletedSynchronously = Interlocked.Exchange(ref this._asyncState, 1) == 2 || this._innerAsyncResult.CompletedSynchronously;
          if (!this.CompletedSynchronously)
          {
            if (timeout > -1)
              this.CreateTimer(timeout);
          }
        }
        if (!this.CompletedSynchronously || callback == null)
          return;
        callback((IAsyncResult) this);
      }

      public static AsyncResultWrapper.WrappedAsyncResult<TResult> Cast(
        IAsyncResult asyncResult,
        object tag)
      {
        if (asyncResult == null)
          throw new ArgumentNullException(nameof (asyncResult));
        if (asyncResult is AsyncResultWrapper.WrappedAsyncResult<TResult> wrappedAsyncResult && object.Equals(wrappedAsyncResult._tag, tag))
          return wrappedAsyncResult;
        throw Error.AsyncCommon_InvalidAsyncResult(nameof (asyncResult));
      }

      private void CreateTimer(int timeout)
      {
        this._timer = new Timer(new TimerCallback(this.HandleTimeout), (object) null, timeout, -1);
      }

      public TResult End()
      {
        if (!this._endExecutedGate.TryEnter())
          throw Error.AsyncCommon_AsyncResultAlreadyConsumed();
        if (this._timedOut)
          throw new TimeoutException();
        this.WaitForBeginToCompleteAndDestroyTimer();
        return this._endDelegate(this._innerAsyncResult);
      }

      private void ExecuteAsynchronousCallback(bool timedOut)
      {
        this.WaitForBeginToCompleteAndDestroyTimer();
        if (!this._handleCallbackGate.TryEnter())
          return;
        this._timedOut = timedOut;
        if (this._originalCallback == null)
          return;
        this._originalCallback((IAsyncResult) this);
      }

      private void HandleAsynchronousCompletion(IAsyncResult asyncResult)
      {
        if (Interlocked.Exchange(ref this._asyncState, 2) != 1)
          return;
        this.ExecuteAsynchronousCallback(false);
      }

      private void HandleTimeout(object state) => this.ExecuteAsynchronousCallback(true);

      private void WaitForBeginToCompleteAndDestroyTimer()
      {
        lock (this._beginDelegateLockObj)
        {
          if (this._timer != null)
            this._timer.Dispose();
          this._timer = (Timer) null;
        }
      }
    }
  }
}
