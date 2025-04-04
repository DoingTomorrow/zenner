// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.ReadOnlyCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache.Access;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Cache
{
  public class ReadOnlyCache : ICacheConcurrencyStrategy
  {
    private ICache cache;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ReadOnlyCache));

    public string RegionName => this.cache.RegionName;

    public ICache Cache
    {
      get => this.cache;
      set => this.cache = value;
    }

    public object Get(CacheKey key, long timestamp)
    {
      object obj = this.cache.Get((object) key);
      if (obj != null && ReadOnlyCache.log.IsDebugEnabled)
        ReadOnlyCache.log.Debug((object) ("Cache hit: " + (object) key));
      return obj;
    }

    public ISoftLock Lock(CacheKey key, object version)
    {
      ReadOnlyCache.log.Error((object) ("Application attempted to edit read only item: " + (object) key));
      throw new InvalidOperationException("ReadOnlyCache: Can't write to a readonly object " + key.EntityOrRoleName);
    }

    public bool Put(
      CacheKey key,
      object value,
      long timestamp,
      object version,
      IComparer versionComparator,
      bool minimalPut)
    {
      if (timestamp == long.MinValue)
        return false;
      if (minimalPut && this.cache.Get((object) key) != null)
      {
        if (ReadOnlyCache.log.IsDebugEnabled)
          ReadOnlyCache.log.Debug((object) ("item already cached: " + (object) key));
        return false;
      }
      if (ReadOnlyCache.log.IsDebugEnabled)
        ReadOnlyCache.log.Debug((object) ("Caching: " + (object) key));
      this.cache.Put((object) key, value);
      return true;
    }

    public void Release(CacheKey key, ISoftLock @lock)
    {
      ReadOnlyCache.log.Error((object) ("Application attempted to edit read only item: " + (object) key));
    }

    public void Clear() => this.cache.Clear();

    public void Remove(CacheKey key) => this.cache.Remove((object) key);

    public void Destroy()
    {
      try
      {
        this.cache.Destroy();
      }
      catch (Exception ex)
      {
        ReadOnlyCache.log.Warn((object) "Could not destroy cache", ex);
      }
    }

    public bool AfterUpdate(CacheKey key, object value, object version, ISoftLock @lock)
    {
      ReadOnlyCache.log.Error((object) ("Application attempted to edit read only item: " + (object) key));
      throw new InvalidOperationException("ReadOnlyCache: Can't write to a readonly object " + key.EntityOrRoleName);
    }

    public bool AfterInsert(CacheKey key, object value, object version) => true;

    public void Evict(CacheKey key)
    {
    }

    public bool Insert(CacheKey key, object value, object currentVersion) => false;

    public bool Update(CacheKey key, object value, object currentVersion, object previousVersion)
    {
      ReadOnlyCache.log.Error((object) ("Application attempted to edit read only item: " + (object) key));
      throw new InvalidOperationException("ReadOnlyCache: Can't write to a readonly object " + key.EntityOrRoleName);
    }
  }
}
