// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.PersistenceConfiguration`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public abstract class PersistenceConfiguration<TThisConfiguration, TConnectionString> : 
    IPersistenceConfigurer
    where TThisConfiguration : PersistenceConfiguration<TThisConfiguration, TConnectionString>
    where TConnectionString : ConnectionStringBuilder, new()
  {
    protected const string DialectKey = "dialect";
    protected const string AltDialectKey = "hibernate.dialect";
    protected const string DefaultSchemaKey = "default_schema";
    protected const string UseOuterJoinKey = "use_outer_join";
    protected const string MaxFetchDepthKey = "max_fetch_depth";
    protected const string UseReflectionOptimizerKey = "use_reflection_optimizer";
    protected const string QuerySubstitutionsKey = "query.substitutions";
    protected const string ShowSqlKey = "show_sql";
    protected const string FormatSqlKey = "format_sql";
    protected const string CollectionTypeFactoryClassKey = "collectiontype.factory_class";
    protected const string ConnectionProviderKey = "connection.provider";
    protected const string DefaultConnectionProviderClassName = "NHibernate.Connection.DriverConnectionProvider";
    protected const string DriverClassKey = "connection.driver_class";
    protected const string ConnectionStringKey = "connection.connection_string";
    protected const string IsolationLevelKey = "connection.isolation";
    protected const string AdoNetBatchSizeKey = "adonet.batch_size";
    protected const string CurrentSessionContextClassKey = "current_session_context_class";
    private readonly Dictionary<string, string> values = new Dictionary<string, string>();
    private bool nextBoolSettingValue = true;
    private readonly TConnectionString connectionString;
    private readonly CacheSettingsBuilder cache = new CacheSettingsBuilder();

    protected PersistenceConfiguration()
    {
      this.values["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider";
      this.connectionString = new TConnectionString();
    }

    protected virtual IDictionary<string, string> CreateProperties()
    {
      if (this.connectionString.IsDirty)
        this.Raw("connection.connection_string", this.connectionString.Create());
      if (this.cache.IsDirty)
      {
        foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) this.cache.Create())
          this.Raw(keyValuePair.Key, keyValuePair.Value);
      }
      return (IDictionary<string, string>) this.values;
    }

    private static IEnumerable<string> OverridenDefaults(IDictionary<string, string> settings)
    {
      if (settings["connection.provider"] != "NHibernate.Connection.DriverConnectionProvider")
        yield return "connection.provider";
    }

    private static IEnumerable<string> KeysToPreserve(
      Configuration nhibernateConfig,
      IDictionary<string, string> settings)
    {
      Configuration configuration = new Configuration();
      return nhibernateConfig.Properties.Except<KeyValuePair<string, string>>((IEnumerable<KeyValuePair<string, string>>) configuration.Properties).Select<KeyValuePair<string, string>, string>((System.Func<KeyValuePair<string, string>, string>) (k => k.Key)).Except<string>(PersistenceConfiguration<TThisConfiguration, TConnectionString>.OverridenDefaults(settings));
    }

    public Configuration ConfigureProperties(Configuration nhibernateConfig)
    {
      IDictionary<string, string> properties = this.CreateProperties();
      IEnumerable<string> keepers = PersistenceConfiguration<TThisConfiguration, TConnectionString>.KeysToPreserve(nhibernateConfig, properties);
      foreach (KeyValuePair<string, string> keyValuePair in properties.Where<KeyValuePair<string, string>>((System.Func<KeyValuePair<string, string>, bool>) (s => !keepers.Contains<string>(s.Key))))
        nhibernateConfig.SetProperty(keyValuePair.Key, keyValuePair.Value);
      return nhibernateConfig;
    }

    public IDictionary<string, string> ToProperties() => this.CreateProperties();

    protected void ToggleBooleanSetting(string settingKey)
    {
      string lowerInvariant = this.nextBoolSettingValue.ToString().ToLowerInvariant();
      this.values[settingKey] = lowerInvariant;
      this.nextBoolSettingValue = true;
    }

    public TThisConfiguration DoNot
    {
      get
      {
        this.nextBoolSettingValue = false;
        return (TThisConfiguration) this;
      }
    }

    public TThisConfiguration Dialect(string dialect)
    {
      this.values[nameof (dialect)] = dialect;
      this.values["hibernate.dialect"] = dialect;
      return (TThisConfiguration) this;
    }

    public TThisConfiguration Dialect<T>() where T : NHibernate.Dialect.Dialect
    {
      return this.Dialect(typeof (T).AssemblyQualifiedName);
    }

    public TThisConfiguration DefaultSchema(string schema)
    {
      this.values["default_schema"] = schema;
      return (TThisConfiguration) this;
    }

    public TThisConfiguration UseOuterJoin()
    {
      this.ToggleBooleanSetting("use_outer_join");
      return (TThisConfiguration) this;
    }

    public TThisConfiguration MaxFetchDepth(int maxFetchDepth)
    {
      this.values["max_fetch_depth"] = maxFetchDepth.ToString();
      return (TThisConfiguration) this;
    }

    public TThisConfiguration UseReflectionOptimizer()
    {
      this.ToggleBooleanSetting("use_reflection_optimizer");
      return (TThisConfiguration) this;
    }

    public TThisConfiguration QuerySubstitutions(string substitutions)
    {
      this.values["query.substitutions"] = substitutions;
      return (TThisConfiguration) this;
    }

    public TThisConfiguration ShowSql()
    {
      this.ToggleBooleanSetting("show_sql");
      return (TThisConfiguration) this;
    }

    public TThisConfiguration FormatSql()
    {
      this.ToggleBooleanSetting("format_sql");
      return (TThisConfiguration) this;
    }

    public TThisConfiguration Provider(string provider)
    {
      this.values["connection.provider"] = provider;
      return (TThisConfiguration) this;
    }

    public TThisConfiguration Provider<T>() where T : IConnectionProvider
    {
      return this.Provider(typeof (T).AssemblyQualifiedName);
    }

    public TThisConfiguration Driver(string driverClass)
    {
      this.values["connection.driver_class"] = driverClass;
      return (TThisConfiguration) this;
    }

    public TThisConfiguration Driver<T>() where T : IDriver
    {
      return this.Driver(typeof (T).AssemblyQualifiedName);
    }

    public TThisConfiguration ConnectionString(
      Action<TConnectionString> connectionStringExpression)
    {
      connectionStringExpression(this.connectionString);
      return (TThisConfiguration) this;
    }

    public TThisConfiguration ConnectionString(string value)
    {
      this.connectionString.Is(value);
      return (TThisConfiguration) this;
    }

    public TThisConfiguration Raw(string key, string value)
    {
      this.values[key] = value;
      return (TThisConfiguration) this;
    }

    public TThisConfiguration AdoNetBatchSize(int size)
    {
      this.values["adonet.batch_size"] = size.ToString();
      return (TThisConfiguration) this;
    }

    public TThisConfiguration IsolationLevel(System.Data.IsolationLevel connectionIsolation)
    {
      return this.IsolationLevel(connectionIsolation.ToString());
    }

    public TThisConfiguration IsolationLevel(string connectionIsolation)
    {
      this.values["connection.isolation"] = connectionIsolation;
      return (TThisConfiguration) this;
    }
  }
}
