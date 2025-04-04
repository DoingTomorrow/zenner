// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.IFluentSessionFactoryConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  public interface IFluentSessionFactoryConfiguration
  {
    IFluentSessionFactoryConfiguration Named(string sessionFactoryName);

    IDbIntegrationConfiguration Integrate { get; }

    ICacheConfiguration Caching { get; }

    IFluentSessionFactoryConfiguration GenerateStatistics();

    IFluentSessionFactoryConfiguration Using(EntityMode entityMode);

    IFluentSessionFactoryConfiguration ParsingHqlThrough<TQueryTranslator>() where TQueryTranslator : IQueryTranslatorFactory;

    IProxyConfiguration Proxy { get; }

    ICollectionFactoryConfiguration GeneratingCollections { get; }

    IMappingsConfiguration Mapping { get; }
  }
}
