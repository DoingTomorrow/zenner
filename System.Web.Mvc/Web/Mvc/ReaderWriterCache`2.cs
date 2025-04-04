// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ReaderWriterCache`2
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace System.Web.Mvc
{
  internal abstract class ReaderWriterCache<TKey, TValue>
  {
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();

    protected ReaderWriterCache()
      : this((IEqualityComparer<TKey>) null)
    {
    }

    protected ReaderWriterCache(IEqualityComparer<TKey> comparer)
    {
      this._cache = new Dictionary<TKey, TValue>(comparer);
    }

    protected Dictionary<TKey, TValue> Cache => this._cache;

    protected TValue FetchOrCreateItem(TKey key, Func<TValue> creator)
    {
      this._readerWriterLock.EnterReadLock();
      try
      {
        TValue obj;
        if (this._cache.TryGetValue(key, out obj))
          return obj;
      }
      finally
      {
        this._readerWriterLock.ExitReadLock();
      }
      TValue obj1 = creator();
      this._readerWriterLock.EnterWriteLock();
      try
      {
        TValue obj2;
        if (this._cache.TryGetValue(key, out obj2))
          return obj2;
        this._cache[key] = obj1;
        return obj1;
      }
      finally
      {
        this._readerWriterLock.ExitWriteLock();
      }
    }
  }
}
