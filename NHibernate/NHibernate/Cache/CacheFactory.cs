// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.CacheFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cache
{
  public static class CacheFactory
  {
    public const string ReadOnly = "read-only";
    public const string ReadWrite = "read-write";
    public const string NonstrictReadWrite = "nonstrict-read-write";
    public const string Transactional = "transactional";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (CacheFactory));

    public static ICacheConcurrencyStrategy CreateCache(
      string usage,
      string name,
      bool mutable,
      Settings settings,
      IDictionary<string, string> properties)
    {
      if (usage == null || !settings.IsSecondLevelCacheEnabled)
        return (ICacheConcurrencyStrategy) null;
      string cacheRegionPrefix = settings.CacheRegionPrefix;
      if (cacheRegionPrefix != null)
        name = cacheRegionPrefix + (object) '.' + name;
      if (CacheFactory.log.IsDebugEnabled)
        CacheFactory.log.Debug((object) string.Format("cache for: {0} usage strategy: {1}", (object) name, (object) usage));
      ICacheConcurrencyStrategy cache1;
      switch (usage)
      {
        case "read-only":
          if (mutable)
            CacheFactory.log.Warn((object) ("read-only cache configured for mutable: " + name));
          cache1 = (ICacheConcurrencyStrategy) new ReadOnlyCache();
          break;
        case "read-write":
          cache1 = (ICacheConcurrencyStrategy) new ReadWriteCache();
          break;
        case "nonstrict-read-write":
          cache1 = (ICacheConcurrencyStrategy) new NonstrictReadWriteCache();
          break;
        default:
          throw new MappingException("cache usage attribute should be read-write, read-only, nonstrict-read-write, or transactional");
      }
      ICache cache2;
      try
      {
        cache2 = settings.CacheProvider.BuildCache(name, properties);
      }
      catch (CacheException ex)
      {
        throw new HibernateException("Could not instantiate cache implementation", (Exception) ex);
      }
      cache1.Cache = cache2;
      return cache1;
    }
  }
}
