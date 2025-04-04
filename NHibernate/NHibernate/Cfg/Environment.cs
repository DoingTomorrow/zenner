// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Environment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Bytecode;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

#nullable disable
namespace NHibernate.Cfg
{
  public static class Environment
  {
    public const string ConnectionProvider = "connection.provider";
    public const string ConnectionDriver = "connection.driver_class";
    public const string ConnectionString = "connection.connection_string";
    public const string Isolation = "connection.isolation";
    public const string ReleaseConnections = "connection.release_mode";
    public const string ConnectionStringName = "connection.connection_string_name";
    public const string SessionFactoryName = "session_factory_name";
    public const string Dialect = "dialect";
    public const string DefaultSchema = "default_schema";
    public const string DefaultCatalog = "default_catalog";
    public const string DefaultEntityMode = "default_entity_mode";
    public const string PreferPooledValuesLo = "id.optimizer.pooled.prefer_lo";
    public const string ShowSql = "show_sql";
    public const string MaxFetchDepth = "max_fetch_depth";
    public const string CurrentSessionContextClass = "current_session_context_class";
    public const string UseSqlComments = "use_sql_comments";
    public const string FormatSql = "format_sql";
    public const string UseGetGeneratedKeys = "jdbc.use_get_generated_keys";
    public const string StatementFetchSize = "jdbc.fetch_size";
    public const string BatchVersionedData = "jdbc.batch_versioned_data";
    public const string OutputStylesheet = "xml.output_stylesheet";
    public const string TransactionStrategy = "transaction.factory_class";
    public const string TransactionManagerStrategy = "transaction.manager_lookup_class";
    public const string CacheProvider = "cache.provider_class";
    public const string UseQueryCache = "cache.use_query_cache";
    public const string QueryCacheFactory = "cache.query_cache_factory";
    public const string UseSecondLevelCache = "cache.use_second_level_cache";
    public const string CacheRegionPrefix = "cache.region_prefix";
    public const string UseMinimalPuts = "cache.use_minimal_puts";
    public const string CacheDefaultExpiration = "cache.default_expiration";
    public const string QuerySubstitutions = "query.substitutions";
    public const string QueryStartupChecking = "query.startup_check";
    public const string GenerateStatistics = "generate_statistics";
    public const string UseIdentifierRollBack = "use_identifier_rollback";
    public const string QueryTranslator = "query.factory_class";
    public const string QueryImports = "query.imports";
    public const string Hbm2ddlAuto = "hbm2ddl.auto";
    public const string Hbm2ddlKeyWords = "hbm2ddl.keywords";
    public const string SqlExceptionConverter = "sql_exception_converter";
    public const string WrapResultSets = "adonet.wrap_result_sets";
    public const string BatchSize = "adonet.batch_size";
    public const string BatchStrategy = "adonet.factory_class";
    public const string PrepareSql = "prepare_sql";
    public const string CommandTimeout = "command_timeout";
    public const string PropertyBytecodeProvider = "bytecode.provider";
    public const string PropertyUseReflectionOptimizer = "use_reflection_optimizer";
    public const string UseProxyValidator = "use_proxy_validator";
    public const string ProxyFactoryFactoryClass = "proxyfactory.factory_class";
    public const string DefaultBatchFetchSize = "default_batch_fetch_size";
    public const string CollectionTypeFactoryClass = "collectiontype.factory_class";
    public const string LinqToHqlGeneratorsRegistry = "linqtohql.generatorsregistry";
    public const string OrderInserts = "order_inserts";
    private static string cachedVersion;
    private static readonly Dictionary<string, string> GlobalProperties;
    private static IBytecodeProvider BytecodeProviderInstance;
    private static bool EnableReflectionOptimizer;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (Environment));

    public static string Version
    {
      get
      {
        if (Environment.cachedVersion == null)
        {
          Assembly executingAssembly = Assembly.GetExecutingAssembly();
          AssemblyInformationalVersionAttribute[] customAttributes = (AssemblyInformationalVersionAttribute[]) executingAssembly.GetCustomAttributes(typeof (AssemblyInformationalVersionAttribute), false);
          Environment.cachedVersion = customAttributes == null || customAttributes.Length <= 0 ? executingAssembly.GetName().Version.ToString() : string.Format("{0} ({1})", (object) executingAssembly.GetName().Version, (object) customAttributes[0].InformationalVersion);
        }
        return Environment.cachedVersion;
      }
    }

    public static void VerifyProperties(IDictionary<string, string> props)
    {
    }

    static Environment()
    {
      if (Environment.log.IsInfoEnabled)
        Environment.log.Info((object) ("NHibernate " + Environment.Version));
      Environment.GlobalProperties = new Dictionary<string, string>();
      Environment.GlobalProperties["use_reflection_optimizer"] = bool.TrueString;
      Environment.LoadGlobalPropertiesFromAppConfig();
      Environment.VerifyProperties((IDictionary<string, string>) Environment.GlobalProperties);
      Environment.BytecodeProviderInstance = Environment.BuildBytecodeProvider((IDictionary<string, string>) Environment.GlobalProperties);
      Environment.EnableReflectionOptimizer = PropertiesHelper.GetBoolean("use_reflection_optimizer", (IDictionary<string, string>) Environment.GlobalProperties);
      if (!Environment.EnableReflectionOptimizer)
        return;
      Environment.log.Info((object) "Using reflection optimizer");
    }

    private static void LoadGlobalPropertiesFromAppConfig()
    {
      object section = ConfigurationManager.GetSection("hibernate-configuration");
      if (section == null)
        Environment.log.Info((object) string.Format("{0} section not found in application configuration file", (object) "hibernate-configuration"));
      else if (!(section is IHibernateConfiguration hibernateConfiguration))
      {
        Environment.log.Info((object) string.Format("{0} section handler, in application configuration file, is not IHibernateConfiguration, section ignored", (object) "hibernate-configuration"));
      }
      else
      {
        Environment.GlobalProperties["bytecode.provider"] = hibernateConfiguration.ByteCodeProviderType;
        Environment.GlobalProperties["use_reflection_optimizer"] = hibernateConfiguration.UseReflectionOptimizer.ToString();
        if (hibernateConfiguration.SessionFactory == null)
          return;
        foreach (KeyValuePair<string, string> property in (IEnumerable<KeyValuePair<string, string>>) hibernateConfiguration.SessionFactory.Properties)
          Environment.GlobalProperties[property.Key] = property.Value;
      }
    }

    internal static void ResetSessionFactoryProperties()
    {
      string str1 = (string) null;
      string str2 = (string) null;
      if (Environment.GlobalProperties.ContainsKey("bytecode.provider"))
        str1 = Environment.GlobalProperties["bytecode.provider"];
      if (Environment.GlobalProperties.ContainsKey("use_reflection_optimizer"))
        str2 = Environment.GlobalProperties["use_reflection_optimizer"];
      Environment.GlobalProperties.Clear();
      if (str1 != null)
        Environment.GlobalProperties["bytecode.provider"] = str1;
      if (str2 == null)
        return;
      Environment.GlobalProperties["use_reflection_optimizer"] = str2;
    }

    public static IDictionary<string, string> Properties
    {
      get
      {
        return (IDictionary<string, string>) new Dictionary<string, string>((IDictionary<string, string>) Environment.GlobalProperties);
      }
    }

    public static IBytecodeProvider BytecodeProvider
    {
      get => Environment.BytecodeProviderInstance;
      set => Environment.BytecodeProviderInstance = value;
    }

    public static bool UseReflectionOptimizer
    {
      get => Environment.EnableReflectionOptimizer;
      set => Environment.EnableReflectionOptimizer = value;
    }

    public static IBytecodeProvider BuildBytecodeProvider(IDictionary<string, string> properties)
    {
      string providerName = PropertiesHelper.GetString("bytecode.provider", properties, "lcg");
      Environment.log.Info((object) ("Bytecode provider name : " + providerName));
      return Environment.BuildBytecodeProvider(providerName);
    }

    private static IBytecodeProvider BuildBytecodeProvider(string providerName)
    {
      switch (providerName)
      {
        case "codedom":
          return (IBytecodeProvider) new NHibernate.Bytecode.CodeDom.BytecodeProviderImpl();
        case "lcg":
          return (IBytecodeProvider) new NHibernate.Bytecode.Lightweight.BytecodeProviderImpl();
        case "null":
          return (IBytecodeProvider) new NullBytecodeProvider();
        default:
          Environment.log.Info((object) ("custom bytecode provider [" + providerName + "]"));
          return Environment.CreateCustomBytecodeProvider(providerName);
      }
    }

    private static IBytecodeProvider CreateCustomBytecodeProvider(string assemblyQualifiedName)
    {
      try
      {
        Type type = ReflectHelper.ClassForName(assemblyQualifiedName);
        try
        {
          return (IBytecodeProvider) Activator.CreateInstance(type);
        }
        catch (MissingMethodException ex)
        {
          throw new HibernateByteCodeException("Public constructor was not found for " + (object) type, (Exception) ex);
        }
        catch (InvalidCastException ex)
        {
          throw new HibernateByteCodeException(type.ToString() + "Type does not implement " + (object) typeof (IBytecodeProvider), (Exception) ex);
        }
        catch (Exception ex)
        {
          throw new HibernateByteCodeException("Unable to instantiate: " + (object) type, ex);
        }
      }
      catch (Exception ex)
      {
        throw new HibernateByteCodeException("Unable to create the instance of Bytecode provider; check inner exception for detail", ex);
      }
    }
  }
}
