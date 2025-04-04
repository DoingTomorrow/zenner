// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ConfigurationExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.Loquacious;
using NHibernate.Context;
using NHibernate.Hql;
using NHibernate.Linq.Functions;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg
{
  public static class ConfigurationExtensions
  {
    public static IFluentSessionFactoryConfiguration SessionFactory(this Configuration configuration)
    {
      return (IFluentSessionFactoryConfiguration) new FluentSessionFactoryConfiguration(configuration);
    }

    public static Configuration SessionFactoryName(
      this Configuration configuration,
      string sessionFactoryName)
    {
      configuration.SetProperty("session_factory_name", sessionFactoryName);
      return configuration;
    }

    public static Configuration Cache(
      this Configuration configuration,
      Action<ICacheConfigurationProperties> cacheProperties)
    {
      cacheProperties((ICacheConfigurationProperties) new CacheConfigurationProperties(configuration));
      return configuration;
    }

    public static Configuration CollectionTypeFactory<TCollecionsFactory>(
      this Configuration configuration)
    {
      configuration.SetProperty("collectiontype.factory_class", typeof (TCollecionsFactory).AssemblyQualifiedName);
      return configuration;
    }

    public static Configuration Proxy(
      this Configuration configuration,
      Action<IProxyConfigurationProperties> proxyProperties)
    {
      proxyProperties((IProxyConfigurationProperties) new ProxyConfigurationProperties(configuration));
      return configuration;
    }

    public static Configuration HqlQueryTranslator<TQueryTranslator>(
      this Configuration configuration)
      where TQueryTranslator : IQueryTranslatorFactory
    {
      configuration.SetProperty("query.factory_class", typeof (TQueryTranslator).AssemblyQualifiedName);
      return configuration;
    }

    public static Configuration LinqToHqlGeneratorsRegistry<TLinqToHqlGeneratorsRegistry>(
      this Configuration configuration)
      where TLinqToHqlGeneratorsRegistry : ILinqToHqlGeneratorsRegistry
    {
      configuration.SetProperty("linqtohql.generatorsregistry", typeof (TLinqToHqlGeneratorsRegistry).AssemblyQualifiedName);
      return configuration;
    }

    public static Configuration CurrentSessionContext<TCurrentSessionContext>(
      this Configuration configuration)
      where TCurrentSessionContext : ICurrentSessionContext
    {
      configuration.SetProperty("current_session_context_class", typeof (TCurrentSessionContext).AssemblyQualifiedName);
      return configuration;
    }

    public static Configuration Mappings(
      this Configuration configuration,
      Action<IMappingsConfigurationProperties> mappingsProperties)
    {
      mappingsProperties((IMappingsConfigurationProperties) new MappingsConfigurationProperties(configuration));
      return configuration;
    }

    public static Configuration DataBaseIntegration(
      this Configuration configuration,
      Action<IDbIntegrationConfigurationProperties> dataBaseIntegration)
    {
      dataBaseIntegration((IDbIntegrationConfigurationProperties) new DbIntegrationConfigurationProperties(configuration));
      return configuration;
    }

    public static Configuration EntityCache<TEntity>(
      this Configuration configuration,
      Action<IEntityCacheConfigurationProperties<TEntity>> entityCacheConfiguration)
      where TEntity : class
    {
      EntityCacheConfigurationProperties<TEntity> configurationProperties = new EntityCacheConfigurationProperties<TEntity>();
      entityCacheConfiguration((IEntityCacheConfigurationProperties<TEntity>) configurationProperties);
      if (configurationProperties.Strategy.HasValue)
        configuration.SetCacheConcurrencyStrategy(typeof (TEntity).FullName, EntityCacheUsageParser.ToString(configurationProperties.Strategy.Value), configurationProperties.RegionName);
      foreach (KeyValuePair<string, IEntityCollectionCacheConfigurationProperties> collection in (IEnumerable<KeyValuePair<string, IEntityCollectionCacheConfigurationProperties>>) configurationProperties.Collections)
        configuration.SetCollectionCacheConcurrencyStrategy(collection.Key, EntityCacheUsageParser.ToString(collection.Value.Strategy), collection.Value.RegionName);
      return configuration;
    }

    public static Configuration TypeDefinition<TDef>(
      this Configuration configuration,
      Action<ITypeDefConfigurationProperties> typeDefConfiguration)
      where TDef : class
    {
      if (typeDefConfiguration == null)
        return configuration;
      TypeDefConfigurationProperties<TDef> configurationProperties = new TypeDefConfigurationProperties<TDef>();
      typeDefConfiguration((ITypeDefConfigurationProperties) configurationProperties);
      if (string.IsNullOrEmpty(configurationProperties.Alias))
        return configuration;
      ConfigurationExtensions.GetMappings(configuration).AddTypeDef(configurationProperties.Alias, typeof (TDef).AssemblyQualifiedName, configurationProperties.Properties.ToTypeParameters());
      return configuration;
    }

    public static Configuration AddNamedQuery(
      this Configuration configuration,
      string queryIdentifier,
      Action<INamedQueryDefinitionBuilder> namedQueryDefinition)
    {
      if (configuration == null)
        throw new ArgumentNullException(nameof (configuration));
      if (queryIdentifier == null)
        throw new ArgumentNullException(nameof (queryIdentifier));
      if (namedQueryDefinition == null)
        throw new ArgumentNullException(nameof (namedQueryDefinition));
      NamedQueryDefinitionBuilder definitionBuilder = new NamedQueryDefinitionBuilder();
      namedQueryDefinition((INamedQueryDefinitionBuilder) definitionBuilder);
      configuration.NamedQueries.Add(queryIdentifier, definitionBuilder.Build());
      return configuration;
    }

    private static NHibernate.Cfg.Mappings GetMappings(Configuration configuration)
    {
      NHibernate.Dialect.Dialect dialect = NHibernate.Dialect.Dialect.GetDialect(configuration.Properties);
      return configuration.CreateMappings(dialect);
    }
  }
}
