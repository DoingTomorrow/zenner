// Decompiled with JetBrains decompiler
// Type: AutoMapper.ThreadSafeList`1
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using AutoMapper.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace AutoMapper
{
  public class ThreadSafeList<T> : IEnumerable<T>, IEnumerable, IDisposable where T : class
  {
    private static readonly IReaderWriterLockSlimFactory ReaderWriterLockSlimFactory = PlatformAdapter.Resolve<IReaderWriterLockSlimFactory>();
    private IReaderWriterLockSlim _lock = ThreadSafeList<T>.ReaderWriterLockSlimFactory.Create();
    private readonly IList<T> _propertyMaps = (IList<T>) new List<T>();
    private bool _disposed;

    public void Add(T propertyMap)
    {
      this._lock.EnterWriteLock();
      try
      {
        this._propertyMaps.Add(propertyMap);
      }
      finally
      {
        this._lock.ExitWriteLock();
      }
    }

    public T GetOrCreate(Predicate<T> predicate, Func<T> creatorFunc)
    {
      this._lock.EnterUpgradeableReadLock();
      try
      {
        T obj = this._propertyMaps.FirstOrDefault<T>((Func<T, bool>) (pm => predicate(pm)));
        if ((object) obj == null)
        {
          this._lock.EnterWriteLock();
          try
          {
            obj = creatorFunc();
            this._propertyMaps.Add(obj);
          }
          finally
          {
            this._lock.ExitWriteLock();
          }
        }
        return obj;
      }
      finally
      {
        this._lock.ExitUpgradeableReadLock();
      }
    }

    public void Clear()
    {
      this._lock.EnterWriteLock();
      try
      {
        this._propertyMaps.Clear();
      }
      finally
      {
        this._lock.ExitWriteLock();
      }
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumeratorImpl();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumeratorImpl();

    private IEnumerator<T> GetEnumeratorImpl()
    {
      this._lock.EnterReadLock();
      try
      {
        return (IEnumerator<T>) this._propertyMaps.ToList<T>().GetEnumerator();
      }
      finally
      {
        this._lock.ExitReadLock();
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing && this._lock != null)
        this._lock.Dispose();
      this._lock = (IReaderWriterLockSlim) null;
      this._disposed = true;
    }
  }
}
