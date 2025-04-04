// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.DbIntegrationConfigurationProperties
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.Connection;
using NHibernate.Driver;
using NHibernate.Exceptions;
using NHibernate.Transaction;
using System.Data;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class DbIntegrationConfigurationProperties : IDbIntegrationConfigurationProperties
  {
    private readonly Configuration configuration;

    public DbIntegrationConfigurationProperties(Configuration configuration)
    {
      this.configuration = configuration;
    }

    public void Dialect<TDialect>() where TDialect : NHibernate.Dialect.Dialect
    {
      this.configuration.SetProperty("dialect", typeof (TDialect).AssemblyQualifiedName);
    }

    public Hbm2DDLKeyWords KeywordsAutoImport
    {
      set => this.configuration.SetProperty("hbm2ddl.keywords", value.ToString());
    }

    public bool LogSqlInConsole
    {
      set => this.configuration.SetProperty("show_sql", value.ToString().ToLowerInvariant());
    }

    public bool LogFormattedSql
    {
      set => this.configuration.SetProperty("format_sql", value.ToString().ToLowerInvariant());
    }

    public void ConnectionProvider<TProvider>() where TProvider : IConnectionProvider
    {
      this.configuration.SetProperty("connection.provider", typeof (TProvider).AssemblyQualifiedName);
    }

    public void Driver<TDriver>() where TDriver : IDriver
    {
      this.configuration.SetProperty("connection.driver_class", typeof (TDriver).AssemblyQualifiedName);
    }

    public IsolationLevel IsolationLevel
    {
      set => this.configuration.SetProperty("connection.isolation", value.ToString());
    }

    public ConnectionReleaseMode ConnectionReleaseMode
    {
      set
      {
        this.configuration.SetProperty("connection.release_mode", ConnectionReleaseModeParser.ToString(value));
      }
    }

    public string ConnectionString
    {
      set => this.configuration.SetProperty("connection.connection_string", value);
    }

    public string ConnectionStringName
    {
      set => this.configuration.SetProperty("connection.connection_string_name", value);
    }

    public void Batcher<TBatcher>() where TBatcher : IBatcherFactory
    {
      this.configuration.SetProperty("adonet.factory_class", typeof (TBatcher).AssemblyQualifiedName);
    }

    public short BatchSize
    {
      set => this.configuration.SetProperty("adonet.batch_size", value.ToString());
    }

    public bool OrderInserts
    {
      set => this.configuration.SetProperty("order_inserts", value.ToString().ToLowerInvariant());
    }

    public void TransactionFactory<TFactory>() where TFactory : ITransactionFactory
    {
      this.configuration.SetProperty("transaction.factory_class", typeof (TFactory).AssemblyQualifiedName);
    }

    public bool PrepareCommands
    {
      set => this.configuration.SetProperty("prepare_sql", value.ToString().ToLowerInvariant());
    }

    public byte Timeout
    {
      set => this.configuration.SetProperty("command_timeout", value.ToString());
    }

    public void ExceptionConverter<TExceptionConverter>() where TExceptionConverter : ISQLExceptionConverter
    {
      this.configuration.SetProperty("sql_exception_converter", typeof (TExceptionConverter).AssemblyQualifiedName);
    }

    public bool AutoCommentSql
    {
      set
      {
        this.configuration.SetProperty("use_sql_comments", value.ToString().ToLowerInvariant());
      }
    }

    public string HqlToSqlSubstitutions
    {
      set => this.configuration.SetProperty("query.substitutions", value);
    }

    public byte MaximumDepthOfOuterJoinFetching
    {
      set => this.configuration.SetProperty("max_fetch_depth", value.ToString());
    }

    public SchemaAutoAction SchemaAction
    {
      set => this.configuration.SetProperty("hbm2ddl.auto", value.ToString());
    }
  }
}
