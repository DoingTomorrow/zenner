// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.CacheConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class CacheConfiguration : ICacheConfiguration
  {
    private readonly FluentSessionFactoryConfiguration fc;

    public CacheConfiguration(FluentSessionFactoryConfiguration parent)
    {
      this.fc = parent;
      this.Queries = (IQueryCacheConfiguration) new QueryCacheConfiguration(this);
    }

    internal Configuration Configuration => this.fc.Configuration;

    public ICacheConfiguration Through<TProvider>() where TProvider : ICacheProvider
    {
      this.fc.Configuration.SetProperty("cache.use_second_level_cache", "true");
      this.fc.Configuration.SetProperty("cache.provider_class", typeof (TProvider).AssemblyQualifiedName);
      return (ICacheConfiguration) this;
    }

    public ICacheConfiguration PrefixingRegionsWith(string regionPrefix)
    {
      this.fc.Configuration.SetProperty("cache.region_prefix", regionPrefix);
      return (ICacheConfiguration) this;
    }

    public ICacheConfiguration UsingMinimalPuts()
    {
      this.fc.Configuration.SetProperty("cache.use_minimal_puts", true.ToString().ToLowerInvariant());
      return (ICacheConfiguration) this;
    }

    public IFluentSessionFactoryConfiguration WithDefaultExpiration(int seconds)
    {
      this.fc.Configuration.SetProperty("cache.default_expiration", seconds.ToString());
      return (IFluentSessionFactoryConfiguration) this.fc;
    }

    public IQueryCacheConfiguration Queries { get; private set; }
  }
}
