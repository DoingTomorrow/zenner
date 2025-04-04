// Decompiled with JetBrains decompiler
// Type: NHibernate.Stat.IStatistics
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Stat
{
  public interface IStatistics
  {
    long EntityDeleteCount { get; }

    long EntityInsertCount { get; }

    long EntityLoadCount { get; }

    long EntityFetchCount { get; }

    long EntityUpdateCount { get; }

    long QueryExecutionCount { get; }

    TimeSpan QueryExecutionMaxTime { get; }

    string QueryExecutionMaxTimeQueryString { get; }

    long QueryCacheHitCount { get; }

    long QueryCacheMissCount { get; }

    long QueryCachePutCount { get; }

    long FlushCount { get; }

    long ConnectCount { get; }

    long SecondLevelCacheHitCount { get; }

    long SecondLevelCacheMissCount { get; }

    long SecondLevelCachePutCount { get; }

    long SessionCloseCount { get; }

    long SessionOpenCount { get; }

    long CollectionLoadCount { get; }

    long CollectionFetchCount { get; }

    long CollectionUpdateCount { get; }

    long CollectionRemoveCount { get; }

    long CollectionRecreateCount { get; }

    DateTime StartTime { get; }

    bool IsStatisticsEnabled { get; set; }

    string[] Queries { get; }

    string[] EntityNames { get; }

    string[] CollectionRoleNames { get; }

    string[] SecondLevelCacheRegionNames { get; }

    long SuccessfulTransactionCount { get; }

    long TransactionCount { get; }

    long PrepareStatementCount { get; }

    long CloseStatementCount { get; }

    long OptimisticFailureCount { get; }

    void Clear();

    EntityStatistics GetEntityStatistics(string entityName);

    CollectionStatistics GetCollectionStatistics(string role);

    SecondLevelCacheStatistics GetSecondLevelCacheStatistics(string regionName);

    QueryStatistics GetQueryStatistics(string queryString);

    void LogSummary();

    TimeSpan OperationThreshold { get; set; }
  }
}
