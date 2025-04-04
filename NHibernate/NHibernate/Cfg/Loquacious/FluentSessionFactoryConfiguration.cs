// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.FluentSessionFactoryConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class FluentSessionFactoryConfiguration : IFluentSessionFactoryConfiguration
  {
    private readonly Configuration configuration;

    public FluentSessionFactoryConfiguration(Configuration configuration)
    {
      this.configuration = configuration;
      this.Integrate = (IDbIntegrationConfiguration) new DbIntegrationConfiguration(configuration);
      this.Caching = (ICacheConfiguration) new CacheConfiguration(this);
      this.Proxy = (IProxyConfiguration) new ProxyConfiguration(this);
      this.GeneratingCollections = (ICollectionFactoryConfiguration) new CollectionFactoryConfiguration(this);
      this.Mapping = (IMappingsConfiguration) new MappingsConfiguration(this);
    }

    internal Configuration Configuration => this.configuration;

    public IFluentSessionFactoryConfiguration Named(string sessionFactoryName)
    {
      this.configuration.SetProperty("session_factory_name", sessionFactoryName);
      return (IFluentSessionFactoryConfiguration) this;
    }

    public IDbIntegrationConfiguration Integrate { get; private set; }

    public ICacheConfiguration Caching { get; private set; }

    public IFluentSessionFactoryConfiguration GenerateStatistics()
    {
      this.configuration.SetProperty("generate_statistics", "true");
      return (IFluentSessionFactoryConfiguration) this;
    }

    public IFluentSessionFactoryConfiguration Using(EntityMode entityMode)
    {
      this.configuration.SetProperty("default_entity_mode", EntityModeHelper.ToString(entityMode));
      return (IFluentSessionFactoryConfiguration) this;
    }

    public IFluentSessionFactoryConfiguration ParsingHqlThrough<TQueryTranslator>() where TQueryTranslator : IQueryTranslatorFactory
    {
      this.configuration.SetProperty("query.factory_class", typeof (TQueryTranslator).AssemblyQualifiedName);
      return (IFluentSessionFactoryConfiguration) this;
    }

    public IProxyConfiguration Proxy { get; private set; }

    public ICollectionFactoryConfiguration GeneratingCollections { get; private set; }

    public IMappingsConfiguration Mapping { get; private set; }
  }
}
