// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.QueryCacheConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class QueryCacheConfiguration : IQueryCacheConfiguration
  {
    private readonly CacheConfiguration cc;

    public QueryCacheConfiguration(CacheConfiguration cc) => this.cc = cc;

    public ICacheConfiguration Through<TFactory>() where TFactory : IQueryCache
    {
      this.cc.Configuration.SetProperty("cache.use_second_level_cache", "true");
      this.cc.Configuration.SetProperty("cache.use_query_cache", "true");
      this.cc.Configuration.SetProperty("cache.query_cache_factory", typeof (TFactory).AssemblyQualifiedName);
      return (ICacheConfiguration) this.cc;
    }
  }
}
