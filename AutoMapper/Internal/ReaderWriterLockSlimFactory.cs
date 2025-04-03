// Decompiled with JetBrains decompiler
// Type: AutoMapper.Internal.ReaderWriterLockSlimFactory
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;

#nullable disable
namespace AutoMapper.Internal
{
  public class ReaderWriterLockSlimFactory : IReaderWriterLockSlimFactory
  {
    public IReaderWriterLockSlim Create()
    {
      return (IReaderWriterLockSlim) new ReaderWriterLockSlimFactory.NoOpReaderWriterLock();
    }

    public class NoOpReaderWriterLock : IReaderWriterLockSlim, IDisposable
    {
      public void Dispose()
      {
      }

      public void EnterWriteLock()
      {
      }

      public void ExitWriteLock()
      {
      }

      public void EnterUpgradeableReadLock()
      {
      }

      public void ExitUpgradeableReadLock()
      {
      }

      public void EnterReadLock()
      {
      }

      public void ExitReadLock()
      {
      }
    }
  }
}
