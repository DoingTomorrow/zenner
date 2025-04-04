// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Settings
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.AdoNet.Util;
using NHibernate.Cache;
using NHibernate.Connection;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.Linq.Functions;
using NHibernate.Transaction;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Cfg
{
  public sealed class Settings
  {
    public Settings() => this.MaximumFetchDepth = -1;

    public SqlStatementLogger SqlStatementLogger { get; internal set; }

    public int MaximumFetchDepth { get; internal set; }

    public IDictionary<string, string> QuerySubstitutions { get; internal set; }

    public NHibernate.Dialect.Dialect Dialect { get; internal set; }

    public int AdoBatchSize { get; internal set; }

    public int DefaultBatchFetchSize { get; internal set; }

    public bool IsScrollableResultSetsEnabled { get; internal set; }

    public bool IsGetGeneratedKeysEnabled { get; internal set; }

    public string DefaultSchemaName { get; set; }

    public string DefaultCatalogName { get; internal set; }

    public string SessionFactoryName { get; internal set; }

    public bool IsAutoCreateSchema { get; internal set; }

    public bool IsAutoDropSchema { get; internal set; }

    public bool IsAutoUpdateSchema { get; internal set; }

    public bool IsAutoValidateSchema { get; internal set; }

    public bool IsAutoQuoteEnabled { get; internal set; }

    public bool IsKeywordsImportEnabled { get; internal set; }

    public bool IsQueryCacheEnabled { get; internal set; }

    public bool IsStructuredCacheEntriesEnabled { get; internal set; }

    public bool IsSecondLevelCacheEnabled { get; internal set; }

    public string CacheRegionPrefix { get; internal set; }

    public bool IsMinimalPutsEnabled { get; internal set; }

    public bool IsCommentsEnabled { get; internal set; }

    public bool IsStatisticsEnabled { get; internal set; }

    public bool IsIdentifierRollbackEnabled { get; internal set; }

    public bool IsFlushBeforeCompletionEnabled { get; internal set; }

    public bool IsAutoCloseSessionEnabled { get; internal set; }

    public ConnectionReleaseMode ConnectionReleaseMode { get; internal set; }

    public ICacheProvider CacheProvider { get; internal set; }

    public IQueryCacheFactory QueryCacheFactory { get; internal set; }

    public IConnectionProvider ConnectionProvider { get; internal set; }

    public ITransactionFactory TransactionFactory { get; internal set; }

    public IBatcherFactory BatcherFactory { get; internal set; }

    public IQueryTranslatorFactory QueryTranslatorFactory { get; internal set; }

    public ISQLExceptionConverter SqlExceptionConverter { get; internal set; }

    public bool IsWrapResultSetsEnabled { get; internal set; }

    public bool IsOrderUpdatesEnabled { get; internal set; }

    public bool IsOrderInsertsEnabled { get; internal set; }

    public EntityMode DefaultEntityMode { get; internal set; }

    public bool IsDataDefinitionImplicitCommit { get; internal set; }

    public bool IsDataDefinitionInTransactionSupported { get; internal set; }

    public bool IsNamedQueryStartupCheckingEnabled { get; internal set; }

    public IsolationLevel IsolationLevel { get; internal set; }

    public bool IsOuterJoinFetchEnabled { get; internal set; }

    public ILinqToHqlGeneratorsRegistry LinqToHqlGeneratorsRegistry { get; internal set; }
  }
}
