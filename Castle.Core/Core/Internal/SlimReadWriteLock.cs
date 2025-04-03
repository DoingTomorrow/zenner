// Decompiled with JetBrains decompiler
// Type: Castle.Core.Internal.SlimReadWriteLock
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Threading;

#nullable disable
namespace Castle.Core.Internal
{
  internal class SlimReadWriteLock : Lock
  {
    private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

    public override IUpgradeableLockHolder ForReadingUpgradeable()
    {
      return this.ForReadingUpgradeable(true);
    }

    public override ILockHolder ForReading() => this.ForReading(true);

    public override ILockHolder ForWriting() => this.ForWriting(true);

    public override IUpgradeableLockHolder ForReadingUpgradeable(bool waitForLock)
    {
      return (IUpgradeableLockHolder) new SlimUpgradeableReadLockHolder(this.locker, waitForLock, this.locker.IsUpgradeableReadLockHeld || this.locker.IsWriteLockHeld);
    }

    public override ILockHolder ForReading(bool waitForLock)
    {
      return this.locker.IsReadLockHeld || this.locker.IsUpgradeableReadLockHeld || this.locker.IsWriteLockHeld ? NoOpLock.Lock : (ILockHolder) new SlimReadLockHolder(this.locker, waitForLock);
    }

    public override ILockHolder ForWriting(bool waitForLock)
    {
      return this.locker.IsWriteLockHeld ? NoOpLock.Lock : (ILockHolder) new SlimWriteLockHolder(this.locker, waitForLock);
    }

    public bool IsReadLockHeld => this.locker.IsReadLockHeld;

    public bool IsUpgradeableReadLockHeld => this.locker.IsUpgradeableReadLockHeld;

    public bool IsWriteLockHeld => this.locker.IsWriteLockHeld;
  }
}
