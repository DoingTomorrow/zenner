// Decompiled with JetBrains decompiler
// Type: Castle.Core.Internal.SlimUpgradeableReadLockHolder
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Threading;

#nullable disable
namespace Castle.Core.Internal
{
  internal class SlimUpgradeableReadLockHolder : IUpgradeableLockHolder, ILockHolder, IDisposable
  {
    private readonly ReaderWriterLockSlim locker;
    private bool lockAcquired;
    private SlimWriteLockHolder writerLock;
    private bool wasLockAlreadyHeld;

    public SlimUpgradeableReadLockHolder(
      ReaderWriterLockSlim locker,
      bool waitForLock,
      bool wasLockAlreadyHelf)
    {
      this.locker = locker;
      if (wasLockAlreadyHelf)
      {
        this.lockAcquired = true;
        this.wasLockAlreadyHeld = true;
      }
      else if (waitForLock)
      {
        locker.EnterUpgradeableReadLock();
        this.lockAcquired = true;
      }
      else
        this.lockAcquired = locker.TryEnterUpgradeableReadLock(0);
    }

    public void Dispose()
    {
      if (this.writerLock != null && this.writerLock.LockAcquired)
      {
        this.writerLock.Dispose();
        this.writerLock = (SlimWriteLockHolder) null;
      }
      if (!this.LockAcquired)
        return;
      if (!this.wasLockAlreadyHeld)
        this.locker.ExitUpgradeableReadLock();
      this.lockAcquired = false;
    }

    public ILockHolder Upgrade() => this.Upgrade(true);

    public ILockHolder Upgrade(bool waitForLock)
    {
      if (this.locker.IsWriteLockHeld)
        return NoOpLock.Lock;
      this.writerLock = new SlimWriteLockHolder(this.locker, waitForLock);
      return (ILockHolder) this.writerLock;
    }

    public bool LockAcquired => this.lockAcquired;
  }
}
