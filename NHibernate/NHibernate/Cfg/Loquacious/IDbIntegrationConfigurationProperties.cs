// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.IDbIntegrationConfigurationProperties
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
  public interface IDbIntegrationConfigurationProperties
  {
    void Dialect<TDialect>() where TDialect : NHibernate.Dialect.Dialect;

    Hbm2DDLKeyWords KeywordsAutoImport { set; }

    bool LogSqlInConsole { set; }

    bool LogFormattedSql { set; }

    void ConnectionProvider<TProvider>() where TProvider : IConnectionProvider;

    void Driver<TDriver>() where TDriver : IDriver;

    IsolationLevel IsolationLevel { set; }

    ConnectionReleaseMode ConnectionReleaseMode { set; }

    string ConnectionString { set; }

    string ConnectionStringName { set; }

    void Batcher<TBatcher>() where TBatcher : IBatcherFactory;

    short BatchSize { set; }

    bool OrderInserts { set; }

    void TransactionFactory<TFactory>() where TFactory : ITransactionFactory;

    bool PrepareCommands { set; }

    byte Timeout { set; }

    void ExceptionConverter<TExceptionConverter>() where TExceptionConverter : ISQLExceptionConverter;

    bool AutoCommentSql { set; }

    string HqlToSqlSubstitutions { set; }

    byte MaximumDepthOfOuterJoinFetching { set; }

    SchemaAutoAction SchemaAction { set; }
  }
}
