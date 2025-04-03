// Decompiled with JetBrains decompiler
// Type: Castle.Core.Internal.Lock
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

#nullable disable
namespace Castle.Core.Internal
{
  public abstract class Lock
  {
    public abstract IUpgradeableLockHolder ForReadingUpgradeable();

    public abstract ILockHolder ForReading();

    public abstract ILockHolder ForWriting();

    public abstract IUpgradeableLockHolder ForReadingUpgradeable(bool waitForLock);

    public abstract ILockHolder ForReading(bool waitForLock);

    public abstract ILockHolder ForWriting(bool waitForLock);

    public static Lock Create() => (Lock) new SlimReadWriteLock();
  }
}
