// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.ReaderWriterLockSlimFactoryOverride
// Assembly: AutoMapper.Net4, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: 30ECE8B3-1802-489A-86AE-267466F9FF1F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.Net4.dll

using System;
using System.Threading;

#nullable disable
namespace AutoMapper.Internal
{
  public class ReaderWriterLockSlimFactoryOverride : IReaderWriterLockSlimFactory
  {
    public IReaderWriterLockSlim Create()
    {
      return (IReaderWriterLockSlim) new ReaderWriterLockSlimFactoryOverride.ReaderWriterLockSlimProxy(new ReaderWriterLockSlim());
    }

    private class ReaderWriterLockSlimProxy : IReaderWriterLockSlim, IDisposable
    {
      private readonly ReaderWriterLockSlim _readerWriterLockSlim;

      public ReaderWriterLockSlimProxy(ReaderWriterLockSlim readerWriterLockSlim)
      {
        this._readerWriterLockSlim = readerWriterLockSlim;
      }

      public void Dispose() => this._readerWriterLockSlim.Dispose();

      public void EnterWriteLock() => this._readerWriterLockSlim.EnterWriteLock();

      public void ExitWriteLock() => this._readerWriterLockSlim.ExitWriteLock();

      public void EnterUpgradeableReadLock()
      {
        this._readerWriterLockSlim.EnterUpgradeableReadLock();
      }

      public void ExitUpgradeableReadLock() => this._readerWriterLockSlim.ExitUpgradeableReadLock();

      public void EnterReadLock() => this._readerWriterLockSlim.EnterReadLock();

      public void ExitReadLock() => this._readerWriterLockSlim.ExitReadLock();
    }
  }
}
