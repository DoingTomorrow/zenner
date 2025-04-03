// Decompiled with JetBrains decompiler
// Type: Castle.Core.Internal.SlimReadLockHolder
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Threading;

#nullable disable
namespace Castle.Core.Internal
{
  internal class SlimReadLockHolder : ILockHolder, IDisposable
  {
    private readonly ReaderWriterLockSlim locker;
    private bool lockAcquired;

    public SlimReadLockHolder(ReaderWriterLockSlim locker, bool waitForLock)
    {
      this.locker = locker;
      if (waitForLock)
      {
        locker.EnterReadLock();
        this.lockAcquired = true;
      }
      else
        this.lockAcquired = locker.TryEnterReadLock(0);
    }

    public void Dispose()
    {
      if (!this.LockAcquired)
        return;
      this.locker.ExitReadLock();
      this.lockAcquired = false;
    }

    public bool LockAcquired => this.lockAcquired;
  }
}
