// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.FluentConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Diagnostics;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Context;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Cfg
{
  public class FluentConfiguration
  {
    private const string ExceptionMessage = "An invalid or incomplete configuration was used while creating a SessionFactory. Check PotentialReasons collection, and InnerException for more detail.";
    private const string ExceptionDatabaseMessage = "Database was not configured through Database method.";
    private const string ExceptionMappingMessage = "No mappings were configured through the Mappings method.";
    private const string CollectionTypeFactoryClassKey = "collectiontype.factory_class";
    private const string ProxyFactoryFactoryClassKey = "proxyfactory.factory_class";
    private const string DefaultProxyFactoryFactoryClassName = "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle";
    private const string CurrentSessionContextClassKey = "current_session_context_class";
    private readonly Configuration cfg;
    private readonly IList<Action<Configuration>> configAlterations = (IList<Action<Configuration>>) new List<Action<Configuration>>();
    private readonly IDiagnosticMessageDispatcher dispatcher = (IDiagnosticMessageDispatcher) new DefaultDiagnosticMessageDispatcher();
    private readonly List<Action<MappingConfiguration>> mappingsBuilders = new List<Action<MappingConfiguration>>();
    private bool dbSet;
    private bool mappingsSet;
    private IDiagnosticLogger logger = (IDiagnosticLogger) new NullDiagnosticsLogger();
    private readonly CacheSettingsBuilder cache = new CacheSettingsBuilder();

    internal FluentConfiguration()
      : this(new Configuration())
    {
    }

    internal FluentConfiguration(Configuration cfg) => this.cfg = cfg;

    internal Configuration Configuration => this.cfg;

    public FluentConfiguration Diagnostics(Action<DiagnosticsConfiguration> diagnosticsSetup)
    {
      DiagnosticsConfiguration diagnosticsConfiguration = new DiagnosticsConfiguration(this.dispatcher, (Action<IDiagnosticLogger>) (new_logger => this.logger = new_logger));
      diagnosticsSetup(diagnosticsConfiguration);
      return this;
    }

    public FluentConfiguration Database(Func<IPersistenceConfigurer> config)
    {
      return this.Database(config());
    }

    public FluentConfiguration Database(IPersistenceConfigurer config)
    {
      config.ConfigureProperties(this.Configuration);
      this.dbSet = true;
      return this;
    }

    public FluentConfiguration Cache(Action<CacheSettingsBuilder> cacheExpression)
    {
      cacheExpression(this.cache);
      return this;
    }

    public FluentConfiguration CollectionTypeFactory(string collectionTypeFactoryClass)
    {
      this.Configuration.SetProperty("collectiontype.factory_class", collectionTypeFactoryClass);
      return this;
    }

    public FluentConfiguration CollectionTypeFactory(Type collectionTypeFactoryClass)
    {
      return this.CollectionTypeFactory(collectionTypeFactoryClass.AssemblyQualifiedName);
    }

    public FluentConfiguration CollectionTypeFactory<TCollectionTypeFactory>() where TCollectionTypeFactory : ICollectionTypeFactory
    {
      return this.CollectionTypeFactory(typeof (TCollectionTypeFactory));
    }

    public FluentConfiguration ProxyFactoryFactory(string proxyFactoryFactoryClass)
    {
      this.Configuration.SetProperty("proxyfactory.factory_class", proxyFactoryFactoryClass);
      return this;
    }

    public FluentConfiguration ProxyFactoryFactory(Type proxyFactoryFactory)
    {
      return this.ProxyFactoryFactory(proxyFactoryFactory.AssemblyQualifiedName);
    }

    public FluentConfiguration ProxyFactoryFactory<TProxyFactoryFactory>() where TProxyFactoryFactory : IProxyFactoryFactory
    {
      return this.ProxyFactoryFactory(typeof (TProxyFactoryFactory));
    }

    public FluentConfiguration CurrentSessionContext(string currentSessionContextClass)
    {
      this.Configuration.SetProperty("current_session_context_class", currentSessionContextClass);
      return this;
    }

    public FluentConfiguration CurrentSessionContext<TSessionContext>() where TSessionContext : ICurrentSessionContext
    {
      return this.CurrentSessionContext(typeof (TSessionContext).AssemblyQualifiedName);
    }

    public FluentConfiguration Mappings(Action<MappingConfiguration> mappings)
    {
      this.mappingsBuilders.Add(mappings);
      this.mappingsSet = true;
      return this;
    }

    public FluentConfiguration ExposeConfiguration(Action<Configuration> config)
    {
      if (config != null)
        this.configAlterations.Add(config);
      return this;
    }

    public ISessionFactory BuildSessionFactory()
    {
      try
      {
        return this.BuildConfiguration().BuildSessionFactory();
      }
      catch (Exception ex)
      {
        throw this.CreateConfigurationException(ex);
      }
    }

    public Configuration BuildConfiguration()
    {
      try
      {
        MappingConfiguration mappingConfiguration = new MappingConfiguration(this.logger);
        foreach (Action<MappingConfiguration> mappingsBuilder in this.mappingsBuilders)
          mappingsBuilder(mappingConfiguration);
        mappingConfiguration.Apply(this.Configuration);
        if (this.cache.IsDirty)
          this.Configuration.AddProperties(this.cache.Create());
        foreach (Action<Configuration> configAlteration in (IEnumerable<Action<Configuration>>) this.configAlterations)
          configAlteration(this.Configuration);
        return this.Configuration;
      }
      catch (Exception ex)
      {
        throw this.CreateConfigurationException(ex);
      }
    }

    private FluentConfigurationException CreateConfigurationException(Exception innerException)
    {
      FluentConfigurationException configurationException = new FluentConfigurationException("An invalid or incomplete configuration was used while creating a SessionFactory. Check PotentialReasons collection, and InnerException for more detail.", innerException);
      if (!this.dbSet)
        configurationException.PotentialReasons.Add("Database was not configured through Database method.");
      if (!this.mappingsSet)
        configurationException.PotentialReasons.Add("No mappings were configured through the Mappings method.");
      return configurationException;
    }
  }
}
