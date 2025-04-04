// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.CacheConfigurationProperties
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class CacheConfigurationProperties : ICacheConfigurationProperties
  {
    private readonly Configuration cfg;

    public CacheConfigurationProperties(Configuration cfg) => this.cfg = cfg;

    public bool UseMinimalPuts
    {
      set => this.cfg.SetProperty("cache.use_minimal_puts", value.ToString().ToLowerInvariant());
    }

    public bool UseQueryCache
    {
      set => this.cfg.SetProperty("cache.use_query_cache", value.ToString().ToLowerInvariant());
    }

    public string RegionsPrefix
    {
      set => this.cfg.SetProperty("cache.region_prefix", value);
    }

    public int DefaultExpiration
    {
      set => this.cfg.SetProperty("cache.default_expiration", value.ToString());
    }

    public void Provider<TProvider>() where TProvider : ICacheProvider
    {
      this.UseSecondLevelCache = true;
      this.cfg.SetProperty("cache.provider_class", typeof (TProvider).AssemblyQualifiedName);
    }

    public void QueryCache<TFactory>() where TFactory : IQueryCache
    {
      this.UseSecondLevelCache = true;
      this.UseQueryCache = true;
      this.cfg.SetProperty("cache.query_cache_factory", typeof (TFactory).AssemblyQualifiedName);
    }

    private bool UseSecondLevelCache
    {
      set
      {
        this.cfg.SetProperty("cache.use_second_level_cache", value.ToString().ToLowerInvariant());
      }
    }
  }
}
