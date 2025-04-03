// Decompiled with JetBrains decompiler
// Type: Castle.Core.Internal.MonitorUpgradeableLockHolder
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Threading;

#nullable disable
namespace Castle.Core.Internal
{
  internal class MonitorUpgradeableLockHolder : IUpgradeableLockHolder, ILockHolder, IDisposable
  {
    private readonly object locker;
    private bool lockAcquired;

    public MonitorUpgradeableLockHolder(object locker, bool waitForLock)
    {
      this.locker = locker;
      if (waitForLock)
      {
        Monitor.Enter(locker);
        this.lockAcquired = true;
      }
      else
        this.lockAcquired = Monitor.TryEnter(locker, 0);
    }

    public void Dispose()
    {
      if (!this.LockAcquired)
        return;
      Monitor.Exit(this.locker);
      this.lockAcquired = false;
    }

    public ILockHolder Upgrade() => NoOpLock.Lock;

    public ILockHolder Upgrade(bool waitForLock) => NoOpLock.Lock;

    public bool LockAcquired => this.lockAcquired;
  }
}
