// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.ReadWriteCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache.Access;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Cache
{
  public class ReadWriteCache : ICacheConcurrencyStrategy
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ReadWriteCache));
    private readonly object _lockObject = new object();
    private ICache cache;
    private int _nextLockId;

    public string RegionName => this.cache.RegionName;

    public ICache Cache
    {
      get => this.cache;
      set => this.cache = value;
    }

    private int NextLockId()
    {
      if (this._nextLockId == int.MaxValue)
        this._nextLockId = int.MinValue;
      return this._nextLockId++;
    }

    public object Get(CacheKey key, long txTimestamp)
    {
      lock (this._lockObject)
      {
        if (ReadWriteCache.log.IsDebugEnabled)
          ReadWriteCache.log.Debug((object) ("Cache lookup: " + (object) key));
        ReadWriteCache.ILockable lockable = (ReadWriteCache.ILockable) this.cache.Get((object) key);
        if (lockable != null && lockable.IsGettable(txTimestamp))
        {
          if (ReadWriteCache.log.IsDebugEnabled)
            ReadWriteCache.log.Debug((object) ("Cache hit: " + (object) key));
          return ((CachedItem) lockable).Value;
        }
        if (ReadWriteCache.log.IsDebugEnabled)
        {
          if (lockable == null)
            ReadWriteCache.log.Debug((object) ("Cache miss: " + (object) key));
          else
            ReadWriteCache.log.Debug((object) ("Cached item was locked: " + (object) key));
        }
        return (object) null;
      }
    }

    public ISoftLock Lock(CacheKey key, object version)
    {
      lock (this._lockObject)
      {
        if (ReadWriteCache.log.IsDebugEnabled)
          ReadWriteCache.log.Debug((object) ("Invalidating: " + (object) key));
        try
        {
          this.cache.Lock((object) key);
          ReadWriteCache.ILockable lockable = (ReadWriteCache.ILockable) this.cache.Get((object) key);
          long timeout = this.cache.NextTimestamp() + (long) this.cache.Timeout;
          CacheLock cacheLock = lockable == null ? new CacheLock(timeout, this.NextLockId(), version) : lockable.Lock(timeout, this.NextLockId());
          this.cache.Put((object) key, (object) cacheLock);
          return (ISoftLock) cacheLock;
        }
        finally
        {
          this.cache.Unlock((object) key);
        }
      }
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
      lock (this._lockObject)
      {
        if (ReadWriteCache.log.IsDebugEnabled)
          ReadWriteCache.log.Debug((object) ("Caching: " + (object) key));
        try
        {
          this.cache.Lock((object) key);
          ReadWriteCache.ILockable lockable = (ReadWriteCache.ILockable) this.cache.Get((object) key);
          if (lockable == null || lockable.IsPuttable(txTimestamp, version, versionComparator))
          {
            this.cache.Put((object) key, (object) new CachedItem(value, this.cache.NextTimestamp(), version));
            if (ReadWriteCache.log.IsDebugEnabled)
              ReadWriteCache.log.Debug((object) ("Cached: " + (object) key));
            return true;
          }
          if (ReadWriteCache.log.IsDebugEnabled)
          {
            if (lockable.IsLock)
              ReadWriteCache.log.Debug((object) ("Item was locked: " + (object) key));
            else
              ReadWriteCache.log.Debug((object) ("Item was already cached: " + (object) key));
          }
          return false;
        }
        finally
        {
          this.cache.Unlock((object) key);
        }
      }
    }

    private void DecrementLock(object key, CacheLock @lock)
    {
      @lock.Unlock(this.cache.NextTimestamp());
      this.cache.Put(key, (object) @lock);
    }

    public void Release(CacheKey key, ISoftLock clientLock)
    {
      lock (this._lockObject)
      {
        if (ReadWriteCache.log.IsDebugEnabled)
          ReadWriteCache.log.Debug((object) ("Releasing: " + (object) key));
        try
        {
          this.cache.Lock((object) key);
          ReadWriteCache.ILockable lockable = (ReadWriteCache.ILockable) this.cache.Get((object) key);
          if (this.IsUnlockable(clientLock, lockable))
            this.DecrementLock((object) key, (CacheLock) lockable);
          else
            this.HandleLockExpiry((object) key);
        }
        finally
        {
          this.cache.Unlock((object) key);
        }
      }
    }

    internal void HandleLockExpiry(object key)
    {
      ReadWriteCache.log.Warn((object) ("An item was expired by the cache while it was locked (increase your cache timeout): " + key));
      long num = this.cache.NextTimestamp() + (long) this.cache.Timeout;
      CacheLock cacheLock = new CacheLock(num, this.NextLockId(), (object) null);
      cacheLock.Unlock(num);
      this.cache.Put(key, (object) cacheLock);
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
        ReadWriteCache.log.Warn((object) "Could not destroy cache", ex);
      }
    }

    public bool AfterUpdate(CacheKey key, object value, object version, ISoftLock clientLock)
    {
      lock (this._lockObject)
      {
        if (ReadWriteCache.log.IsDebugEnabled)
          ReadWriteCache.log.Debug((object) ("Updating: " + (object) key));
        try
        {
          this.cache.Lock((object) key);
          ReadWriteCache.ILockable myLock = (ReadWriteCache.ILockable) this.cache.Get((object) key);
          if (this.IsUnlockable(clientLock, myLock))
          {
            CacheLock @lock = (CacheLock) myLock;
            if (@lock.WasLockedConcurrently)
            {
              this.DecrementLock((object) key, @lock);
            }
            else
            {
              this.cache.Put((object) key, (object) new CachedItem(value, this.cache.NextTimestamp(), version));
              if (ReadWriteCache.log.IsDebugEnabled)
                ReadWriteCache.log.Debug((object) ("Updated: " + (object) key));
            }
            return true;
          }
          this.HandleLockExpiry((object) key);
          return false;
        }
        finally
        {
          this.cache.Unlock((object) key);
        }
      }
    }

    public bool AfterInsert(CacheKey key, object value, object version)
    {
      lock (this._lockObject)
      {
        if (ReadWriteCache.log.IsDebugEnabled)
          ReadWriteCache.log.Debug((object) ("Inserting: " + (object) key));
        try
        {
          this.cache.Lock((object) key);
          if ((ReadWriteCache.ILockable) this.cache.Get((object) key) != null)
            return false;
          this.cache.Put((object) key, (object) new CachedItem(value, this.cache.NextTimestamp(), version));
          if (ReadWriteCache.log.IsDebugEnabled)
            ReadWriteCache.log.Debug((object) ("Inserted: " + (object) key));
          return true;
        }
        finally
        {
          this.cache.Unlock((object) key);
        }
      }
    }

    public void Evict(CacheKey key)
    {
    }

    public bool Insert(CacheKey key, object value, object currentVersion) => false;

    public bool Update(CacheKey key, object value, object currentVersion, object previousVersion)
    {
      return false;
    }

    private bool IsUnlockable(ISoftLock clientLock, ReadWriteCache.ILockable myLock)
    {
      return myLock != null && myLock.IsLock && clientLock != null && ((CacheLock) clientLock).Id == ((CacheLock) myLock).Id;
    }

    public interface ILockable
    {
      CacheLock Lock(long timeout, int id);

      bool IsLock { get; }

      bool IsGettable(long txTimestamp);

      bool IsPuttable(long txTimestamp, object newVersion, IComparer comparator);
    }
  }
}
