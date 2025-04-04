// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.ThreadPoolTimer
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Threading;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class ThreadPoolTimer : ITimer, IDisposable
  {
    private Timer m_timer;
    private readonly object m_lock = new object();

    internal ThreadPoolTimer()
    {
      this.m_timer = new Timer(new TimerCallback(this.TimeoutCallback), (object) null, 0, 0);
    }

    private void TimeoutCallback(object state) => this.Elapsed((object) this, EventArgs.Empty);

    public void Dispose()
    {
      if (this.m_timer == null)
        return;
      lock (this.m_lock)
      {
        if (this.m_timer == null)
          return;
        this.m_timer.Dispose();
        this.m_timer = (Timer) null;
      }
    }

    public void SetTimeout(TimeSpan timeout)
    {
      lock (this.m_lock)
      {
        if (timeout.TotalMilliseconds > 4294967294.0)
          timeout = TimeSpan.FromMilliseconds(4294967294.0);
        if (this.m_timer == null)
          return;
        this.m_timer.Change(timeout, new TimeSpan(-1L));
      }
    }

    public event EventHandler Elapsed = (param0, param1) => { };
  }
}
