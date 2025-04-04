// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.ICacheConcurrencyStrategy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache.Access;
using System.Collections;

#nullable disable
namespace NHibernate.Cache
{
  public interface ICacheConcurrencyStrategy
  {
    object Get(CacheKey key, long txTimestamp);

    bool Put(
      CacheKey key,
      object value,
      long txTimestamp,
      object version,
      IComparer versionComparer,
      bool minimalPut);

    ISoftLock Lock(CacheKey key, object version);

    void Evict(CacheKey key);

    bool Update(CacheKey key, object value, object currentVersion, object previousVersion);

    bool Insert(CacheKey key, object value, object currentVersion);

    void Release(CacheKey key, ISoftLock @lock);

    bool AfterUpdate(CacheKey key, object value, object version, ISoftLock @lock);

    bool AfterInsert(CacheKey key, object value, object version);

    void Remove(CacheKey key);

    void Clear();

    void Destroy();

    string RegionName { get; }

    ICache Cache { get; set; }
  }
}
