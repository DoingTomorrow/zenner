// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.SettingsFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.AdoNet.Util;
using NHibernate.Cache;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq.Functions;
using NHibernate.Transaction;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Cfg
{
  [Serializable]
  public sealed class SettingsFactory
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SettingsFactory));
    private static readonly string DefaultCacheProvider = typeof (NoCacheProvider).AssemblyQualifiedName;

    public Settings BuildSettings(IDictionary<string, string> properties)
    {
      Settings settings = new Settings();
      NHibernate.Dialect.Dialect dialect;
      try
      {
        dialect = NHibernate.Dialect.Dialect.GetDialect(properties);
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> defaultProperty in (IEnumerable<KeyValuePair<string, string>>) dialect.DefaultProperties)
          dictionary[defaultProperty.Key] = defaultProperty.Value;
        foreach (KeyValuePair<string, string> property in (IEnumerable<KeyValuePair<string, string>>) properties)
          dictionary[property.Key] = property.Value;
        properties = (IDictionary<string, string>) dictionary;
      }
      catch (HibernateException ex)
      {
        SettingsFactory.log.Warn((object) ("No dialect set - using GenericDialect: " + ex.Message));
        dialect = (NHibernate.Dialect.Dialect) new GenericDialect();
      }
      settings.Dialect = dialect;
      settings.LinqToHqlGeneratorsRegistry = LinqToHqlGeneratorsRegistryFactory.CreateGeneratorsRegistry(properties);
      ISQLExceptionConverter exceptionConverter;
      try
      {
        exceptionConverter = SQLExceptionConverterFactory.BuildSQLExceptionConverter(dialect, properties);
      }
      catch (HibernateException ex)
      {
        SettingsFactory.log.Warn((object) "Error building SQLExceptionConverter; using minimal converter");
        exceptionConverter = SQLExceptionConverterFactory.BuildMinimalSQLExceptionConverter();
      }
      settings.SqlExceptionConverter = exceptionConverter;
      bool boolean1 = PropertiesHelper.GetBoolean("use_sql_comments", properties);
      SettingsFactory.log.Info((object) ("Generate SQL with comments: " + SettingsFactory.EnabledDisabled(boolean1)));
      settings.IsCommentsEnabled = boolean1;
      int int32_1 = PropertiesHelper.GetInt32("max_fetch_depth", properties, -1);
      if (int32_1 != -1)
        SettingsFactory.log.Info((object) ("Maximum outer join fetch depth: " + (object) int32_1));
      IConnectionProvider connectionProvider = ConnectionProviderFactory.NewConnectionProvider(properties);
      ITransactionFactory transactionFactory = SettingsFactory.CreateTransactionFactory(properties);
      bool boolean2 = PropertiesHelper.GetBoolean("cache.use_minimal_puts", properties, false);
      SettingsFactory.log.Info((object) ("Optimize cache for minimal puts: " + (object) boolean2));
      string str1 = PropertiesHelper.GetString("connection.release_mode", properties, "auto");
      SettingsFactory.log.Info((object) ("Connection release mode: " + str1));
      ConnectionReleaseMode connectionReleaseMode = !"auto".Equals(str1) ? ConnectionReleaseModeParser.Convert(str1) : ConnectionReleaseMode.AfterTransaction;
      settings.ConnectionReleaseMode = connectionReleaseMode;
      string str2 = PropertiesHelper.GetString("default_schema", properties, (string) null);
      string str3 = PropertiesHelper.GetString("default_catalog", properties, (string) null);
      if (str2 != null)
        SettingsFactory.log.Info((object) ("Default schema: " + str2));
      if (str3 != null)
        SettingsFactory.log.Info((object) ("Default catalog: " + str3));
      settings.DefaultSchemaName = str2;
      settings.DefaultCatalogName = str3;
      int int32_2 = PropertiesHelper.GetInt32("default_batch_fetch_size", properties, 1);
      SettingsFactory.log.Info((object) ("Default batch fetch size: " + (object) int32_2));
      settings.DefaultBatchFetchSize = int32_2;
      bool boolean3 = PropertiesHelper.GetBoolean("show_sql", properties, false);
      if (boolean3)
        SettingsFactory.log.Info((object) "echoing all SQL to stdout");
      bool boolean4 = PropertiesHelper.GetBoolean("format_sql", properties);
      bool boolean5 = PropertiesHelper.GetBoolean("generate_statistics", properties);
      SettingsFactory.log.Info((object) ("Statistics: " + SettingsFactory.EnabledDisabled(boolean5)));
      settings.IsStatisticsEnabled = boolean5;
      bool boolean6 = PropertiesHelper.GetBoolean("use_identifier_rollback", properties);
      SettingsFactory.log.Info((object) ("Deleted entity synthetic identifier rollback: " + SettingsFactory.EnabledDisabled(boolean6)));
      settings.IsIdentifierRollbackEnabled = boolean6;
      settings.QueryTranslatorFactory = SettingsFactory.CreateQueryTranslatorFactory(properties);
      IDictionary<string, string> dictionary1 = PropertiesHelper.ToDictionary("query.substitutions", " ,=;:\n\t\r\f", properties);
      if (SettingsFactory.log.IsInfoEnabled)
        SettingsFactory.log.Info((object) ("Query language substitutions: " + CollectionPrinter.ToString((IDictionary) dictionary1)));
      string str4 = PropertiesHelper.GetString("hbm2ddl.auto", properties, (string) null);
      if (SchemaAutoAction.Update == str4)
        settings.IsAutoUpdateSchema = true;
      else if (SchemaAutoAction.Create == str4)
        settings.IsAutoCreateSchema = true;
      else if (SchemaAutoAction.Recreate == str4)
      {
        settings.IsAutoCreateSchema = true;
        settings.IsAutoDropSchema = true;
      }
      else if (SchemaAutoAction.Validate == str4)
        settings.IsAutoValidateSchema = true;
      string lowerInvariant = PropertiesHelper.GetString("hbm2ddl.keywords", properties, "not-defined").ToLowerInvariant();
      if (lowerInvariant == Hbm2DDLKeyWords.None)
      {
        settings.IsKeywordsImportEnabled = false;
        settings.IsAutoQuoteEnabled = false;
      }
      else if (lowerInvariant == Hbm2DDLKeyWords.Keywords)
        settings.IsKeywordsImportEnabled = true;
      else if (lowerInvariant == Hbm2DDLKeyWords.AutoQuote)
      {
        settings.IsKeywordsImportEnabled = true;
        settings.IsAutoQuoteEnabled = true;
      }
      else if (lowerInvariant == "not-defined")
      {
        settings.IsKeywordsImportEnabled = true;
        settings.IsAutoQuoteEnabled = false;
      }
      bool boolean7 = PropertiesHelper.GetBoolean("cache.use_second_level_cache", properties, true);
      bool boolean8 = PropertiesHelper.GetBoolean("cache.use_query_cache", properties);
      settings.CacheProvider = boolean7 || boolean8 ? SettingsFactory.CreateCacheProvider(properties) : (ICacheProvider) new NoCacheProvider();
      string str5 = PropertiesHelper.GetString("cache.region_prefix", properties, (string) null);
      if (string.IsNullOrEmpty(str5))
        str5 = (string) null;
      if (str5 != null)
        SettingsFactory.log.Info((object) ("Cache region prefix: " + str5));
      if (boolean8)
      {
        string name = PropertiesHelper.GetString("cache.query_cache_factory", properties, typeof (StandardQueryCacheFactory).FullName);
        SettingsFactory.log.Info((object) ("query cache factory: " + name));
        try
        {
          settings.QueryCacheFactory = (IQueryCacheFactory) Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(name));
        }
        catch (Exception ex)
        {
          throw new HibernateException("could not instantiate IQueryCacheFactory: " + name, ex);
        }
      }
      string str6 = PropertiesHelper.GetString("session_factory_name", properties, (string) null);
      settings.AdoBatchSize = PropertiesHelper.GetInt32("adonet.batch_size", properties, 0);
      bool boolean9 = PropertiesHelper.GetBoolean("order_inserts", properties, settings.AdoBatchSize > 0);
      SettingsFactory.log.Info((object) ("Order SQL inserts for batching: " + SettingsFactory.EnabledDisabled(boolean9)));
      settings.IsOrderInsertsEnabled = boolean9;
      bool boolean10 = PropertiesHelper.GetBoolean("adonet.wrap_result_sets", properties, false);
      SettingsFactory.log.Debug((object) ("Wrap result sets: " + SettingsFactory.EnabledDisabled(boolean10)));
      settings.IsWrapResultSetsEnabled = boolean10;
      settings.BatcherFactory = SettingsFactory.CreateBatcherFactory(properties, settings.AdoBatchSize, connectionProvider);
      string str7 = PropertiesHelper.GetString("connection.isolation", properties, string.Empty);
      IsolationLevel isolationLevel = IsolationLevel.Unspecified;
      if (str7.Length > 0)
      {
        try
        {
          isolationLevel = (IsolationLevel) Enum.Parse(typeof (IsolationLevel), str7);
          SettingsFactory.log.Info((object) ("Using Isolation Level: " + (object) isolationLevel));
        }
        catch (ArgumentException ex)
        {
          SettingsFactory.log.Error((object) ("error configuring IsolationLevel " + str7), (Exception) ex);
          throw new HibernateException("The isolation level of " + str7 + " is not a valid IsolationLevel.  Please use one of the Member Names from the IsolationLevel.", (Exception) ex);
        }
      }
      EntityMode entityMode = EntityModeHelper.Parse(PropertiesHelper.GetString("default_entity_mode", properties, "poco"));
      SettingsFactory.log.Info((object) ("Default entity-mode: " + (object) entityMode));
      settings.DefaultEntityMode = entityMode;
      bool boolean11 = PropertiesHelper.GetBoolean("query.startup_check", properties, true);
      SettingsFactory.log.Info((object) ("Named query checking : " + SettingsFactory.EnabledDisabled(boolean11)));
      settings.IsNamedQueryStartupCheckingEnabled = boolean11;
      settings.SqlStatementLogger = new SqlStatementLogger(boolean3, boolean4);
      settings.ConnectionProvider = connectionProvider;
      settings.QuerySubstitutions = dictionary1;
      settings.TransactionFactory = transactionFactory;
      settings.SessionFactoryName = str6;
      settings.MaximumFetchDepth = int32_1;
      settings.IsQueryCacheEnabled = boolean8;
      settings.IsSecondLevelCacheEnabled = boolean7;
      settings.CacheRegionPrefix = str5;
      settings.IsMinimalPutsEnabled = boolean2;
      settings.IsolationLevel = isolationLevel;
      return settings;
    }

    private static IBatcherFactory CreateBatcherFactory(
      IDictionary<string, string> properties,
      int batchSize,
      IConnectionProvider connectionProvider)
    {
      Type type = typeof (NonBatchingBatcherFactory);
      string name = PropertiesHelper.GetString("adonet.factory_class", properties, (string) null);
      if (string.IsNullOrEmpty(name))
      {
        if (batchSize > 0 && connectionProvider.Driver is IEmbeddedBatcherFactoryProvider driver && driver.BatcherFactoryClass != null)
          type = driver.BatcherFactoryClass;
      }
      else
        type = ReflectHelper.ClassForName(name);
      SettingsFactory.log.Info((object) ("Batcher factory: " + type.AssemblyQualifiedName));
      try
      {
        return (IBatcherFactory) Environment.BytecodeProvider.ObjectsFactory.CreateInstance(type);
      }
      catch (Exception ex)
      {
        throw new HibernateException("Could not instantiate BatcherFactory: " + name, ex);
      }
    }

    private static string EnabledDisabled(bool value) => !value ? "disabled" : "enabled";

    private static ICacheProvider CreateCacheProvider(IDictionary<string, string> properties)
    {
      string name = PropertiesHelper.GetString("cache.provider_class", properties, SettingsFactory.DefaultCacheProvider);
      SettingsFactory.log.Info((object) ("cache provider: " + name));
      try
      {
        return (ICacheProvider) Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(name));
      }
      catch (Exception ex)
      {
        throw new HibernateException("could not instantiate CacheProvider: " + name, ex);
      }
    }

    private static IQueryTranslatorFactory CreateQueryTranslatorFactory(
      IDictionary<string, string> properties)
    {
      string name = PropertiesHelper.GetString("query.factory_class", properties, typeof (ASTQueryTranslatorFactory).FullName);
      SettingsFactory.log.Info((object) ("Query translator: " + name));
      try
      {
        return (IQueryTranslatorFactory) Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(name));
      }
      catch (Exception ex)
      {
        throw new HibernateException("could not instantiate QueryTranslatorFactory: " + name, ex);
      }
    }

    private static ITransactionFactory CreateTransactionFactory(
      IDictionary<string, string> properties)
    {
      string name = PropertiesHelper.GetString("transaction.factory_class", properties, typeof (AdoNetWithDistributedTransactionFactory).FullName);
      SettingsFactory.log.Info((object) ("Transaction factory: " + name));
      try
      {
        return (ITransactionFactory) Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(name));
      }
      catch (Exception ex)
      {
        throw new HibernateException("could not instantiate TransactionFactory: " + name, ex);
      }
    }
  }
}
