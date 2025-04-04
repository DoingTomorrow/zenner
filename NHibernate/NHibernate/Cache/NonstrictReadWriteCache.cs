// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.NonstrictReadWriteCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache.Access;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Cache
{
  public class NonstrictReadWriteCache : ICacheConcurrencyStrategy
  {
    private ICache cache;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (NonstrictReadWriteCache));

    public string RegionName => this.cache.RegionName;

    public ICache Cache
    {
      get => this.cache;
      set => this.cache = value;
    }

    public object Get(CacheKey key, long txTimestamp)
    {
      if (NonstrictReadWriteCache.log.IsDebugEnabled)
        NonstrictReadWriteCache.log.Debug((object) ("Cache lookup: " + (object) key));
      object obj = this.cache.Get((object) key);
      if (obj != null)
        NonstrictReadWriteCache.log.Debug((object) "Cache hit");
      else
        NonstrictReadWriteCache.log.Debug((object) "Cache miss");
      return obj;
    }

    public bool Put(
      CacheKey key,
      object value,
      long txTimestamp,
      object version,
      IComparer versionComparator,
      bool minimalPut)
    {
      if (txTimestamp == long.MinValue)
        return false;
      if (minimalPut && this.cache.Get((object) key) != null)
      {
        if (NonstrictReadWriteCache.log.IsDebugEnabled)
          NonstrictReadWriteCache.log.Debug((object) ("item already cached: " + (object) key));
        return false;
      }
      if (NonstrictReadWriteCache.log.IsDebugEnabled)
        NonstrictReadWriteCache.log.Debug((object) ("Caching: " + (object) key));
      this.cache.Put((object) key, value);
      return true;
    }

    public ISoftLock Lock(CacheKey key, object version) => (ISoftLock) null;

    public void Remove(CacheKey key)
    {
      if (NonstrictReadWriteCache.log.IsDebugEnabled)
        NonstrictReadWriteCache.log.Debug((object) ("Removing: " + (object) key));
      this.cache.Remove((object) key);
    }

    public void Clear()
    {
      if (NonstrictReadWriteCache.log.IsDebugEnabled)
        NonstrictReadWriteCache.log.Debug((object) "Clearing");
      this.cache.Clear();
    }

    public void Destroy()
    {
      try
      {
        this.cache.Destroy();
      }
      catch (Exception ex)
      {
        NonstrictReadWriteCache.log.Warn((object) "Could not destroy cache", ex);
      }
    }

    public void Evict(CacheKey key)
    {
      if (NonstrictReadWriteCache.log.IsDebugEnabled)
        NonstrictReadWriteCache.log.Debug((object) ("Invalidating: " + (object) key));
      this.cache.Remove((object) key);
    }

    public bool Update(CacheKey key, object value, object currentVersion, object previousVersion)
    {
      this.Evict(key);
      return false;
    }

    public bool Insert(CacheKey key, object value, object currentVersion) => false;

    public void Release(CacheKey key, ISoftLock @lock)
    {
      if (NonstrictReadWriteCache.log.IsDebugEnabled)
        NonstrictReadWriteCache.log.Debug((object) ("Invalidating (again): " + (object) key));
      this.cache.Remove((object) key);
    }

    public bool AfterUpdate(CacheKey key, object value, object version, ISoftLock @lock)
    {
      this.Release(key, @lock);
      return false;
    }

    public bool AfterInsert(CacheKey key, object value, object version) => false;
  }
}
