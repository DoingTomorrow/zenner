// Decompiled with JetBrains decompiler
// Type: NLog.Internal.TimeoutContinuation
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Threading;

#nullable disable
namespace NLog.Internal
{
  internal class TimeoutContinuation : IDisposable
  {
    private AsyncContinuation _asyncContinuation;
    private Timer _timeoutTimer;

    public TimeoutContinuation(AsyncContinuation asyncContinuation, TimeSpan timeout)
    {
      this._asyncContinuation = asyncContinuation;
      this._timeoutTimer = new Timer(new TimerCallback(this.TimerElapsed), (object) null, timeout, TimeSpan.FromMilliseconds(-1.0));
    }

    public void Function(Exception exception)
    {
      try
      {
        this.StopTimer();
        AsyncContinuation asyncContinuation = Interlocked.Exchange<AsyncContinuation>(ref this._asyncContinuation, (AsyncContinuation) null);
        if (asyncContinuation == null)
          return;
        asyncContinuation(exception);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Exception in asynchronous handler.");
        if (!ex.MustBeRethrown())
          return;
        throw;
      }
    }

    public void Dispose()
    {
      this.StopTimer();
      GC.SuppressFinalize((object) this);
    }

    private void StopTimer()
    {
      lock (this)
      {
        Timer timeoutTimer = this._timeoutTimer;
        if (timeoutTimer == null)
          return;
        this._timeoutTimer = (Timer) null;
        timeoutTimer.WaitForDispose(TimeSpan.Zero);
      }
    }

    private void TimerElapsed(object state)
    {
      this.Function((Exception) new TimeoutException("Timeout."));
    }
  }
}
