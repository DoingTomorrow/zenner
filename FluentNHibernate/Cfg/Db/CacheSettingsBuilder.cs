// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.CacheSettingsBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Cache;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class CacheSettingsBuilder
  {
    protected const string ProviderClassKey = "cache.provider_class";
    protected const string CacheUseMininmalPutsKey = "cache.use_minimal_puts";
    protected const string CacheUseQueryCacheKey = "cache.use_query_cache";
    protected const string CacheUseSecondLevelCacheKey = "cache.use_second_level_cache";
    protected const string CacheQueryCacheFactoryKey = "cache.query_cache_factory";
    protected const string CacheRegionPrefixKey = "cache.region_prefix";
    private readonly IDictionary<string, string> settings = (IDictionary<string, string>) new Dictionary<string, string>();
    private bool nextBool = true;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public CacheSettingsBuilder Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public CacheSettingsBuilder ProviderClass(string providerclass)
    {
      this.settings.Add("cache.provider_class", providerclass);
      this.IsDirty = true;
      return this;
    }

    public CacheSettingsBuilder ProviderClass<T>() where T : ICacheProvider
    {
      return this.ProviderClass(typeof (T).AssemblyQualifiedName);
    }

    public CacheSettingsBuilder UseMinimalPuts()
    {
      this.settings.Add("cache.use_minimal_puts", this.nextBool.ToString().ToLowerInvariant());
      this.nextBool = true;
      this.IsDirty = true;
      return this;
    }

    public CacheSettingsBuilder UseQueryCache()
    {
      this.settings.Add("cache.use_query_cache", this.nextBool.ToString().ToLowerInvariant());
      this.nextBool = true;
      this.IsDirty = true;
      return this;
    }

    public CacheSettingsBuilder UseSecondLevelCache()
    {
      this.settings.Add("cache.use_second_level_cache", this.nextBool.ToString().ToLowerInvariant());
      this.nextBool = true;
      this.IsDirty = true;
      return this;
    }

    public CacheSettingsBuilder QueryCacheFactory(string factory)
    {
      this.settings.Add("cache.query_cache_factory", factory);
      this.IsDirty = true;
      return this;
    }

    public CacheSettingsBuilder QueryCacheFactory<T>() where T : IQueryCacheFactory
    {
      return this.QueryCacheFactory(typeof (T).AssemblyQualifiedName);
    }

    public CacheSettingsBuilder RegionPrefix(string prefix)
    {
      this.settings.Add("cache.region_prefix", prefix);
      this.IsDirty = true;
      return this;
    }

    protected internal IDictionary<string, string> Create() => this.settings;

    internal bool IsDirty { get; set; }
  }
}
