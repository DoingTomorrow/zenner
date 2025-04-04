// Decompiled with JetBrains decompiler
// Type: NHibernate.Stat.StatisticsImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

#nullable disable
namespace NHibernate.Stat
{
  public class StatisticsImpl : IStatistics, IStatisticsImplementor
  {
    internal const string OperationLoad = "load ";
    internal const string OperationFetch = "fetch ";
    internal const string OperationUpdate = "update ";
    internal const string OperationInsert = "insert ";
    internal const string OperationDelete = "delete ";
    internal const string OperationLoadCollection = "loadCollection ";
    internal const string OperationFetchCollection = "fetchCollection ";
    internal const string OperationUpdateCollection = "updateCollection ";
    internal const string OperationRecreateCollection = "recreateCollection ";
    internal const string OperationRemoveCollection = "removeCollection ";
    internal const string OperationExecuteQuery = "executeQuery ";
    internal const string OperationEndTransaction = "endTransaction ";
    private object _syncRoot;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (StatisticsImpl));
    private readonly ISessionFactoryImplementor sessionFactory;
    private long entityDeleteCount;
    private long entityInsertCount;
    private long entityLoadCount;
    private long entityFetchCount;
    private long entityUpdateCount;
    private TimeSpan operationThreshold = TimeSpan.MaxValue;
    private long queryExecutionCount;
    private TimeSpan queryExecutionMaxTime;
    private string queryExecutionMaxTimeQueryString;
    private long queryCacheHitCount;
    private long queryCacheMissCount;
    private long queryCachePutCount;
    private long flushCount;
    private long connectCount;
    private long secondLevelCacheHitCount;
    private long secondLevelCacheMissCount;
    private long secondLevelCachePutCount;
    private long sessionCloseCount;
    private long sessionOpenCount;
    private long collectionLoadCount;
    private long collectionFetchCount;
    private long collectionUpdateCount;
    private long collectionRemoveCount;
    private long collectionRecreateCount;
    private DateTime startTime;
    private long commitedTransactionCount;
    private long transactionCount;
    private long prepareStatementCount;
    private long closeStatementCount;
    private long optimisticFailureCount;
    private readonly Dictionary<string, SecondLevelCacheStatistics> secondLevelCacheStatistics = new Dictionary<string, SecondLevelCacheStatistics>();
    private readonly Dictionary<string, EntityStatistics> entityStatistics = new Dictionary<string, EntityStatistics>();
    private readonly Dictionary<string, CollectionStatistics> collectionStatistics = new Dictionary<string, CollectionStatistics>();
    private readonly Dictionary<string, QueryStatistics> queryStatistics = new Dictionary<string, QueryStatistics>();

    public StatisticsImpl() => this.Clear();

    public StatisticsImpl(ISessionFactoryImplementor sessionFactory)
      : this()
    {
      this.sessionFactory = sessionFactory;
    }

    private object SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    public long EntityDeleteCount => this.entityDeleteCount;

    public long EntityInsertCount => this.entityInsertCount;

    public long EntityLoadCount => this.entityLoadCount;

    public long EntityFetchCount => this.entityFetchCount;

    public long EntityUpdateCount => this.entityUpdateCount;

    public long QueryExecutionCount => this.queryExecutionCount;

    public TimeSpan QueryExecutionMaxTime => this.queryExecutionMaxTime;

    public string QueryExecutionMaxTimeQueryString => this.queryExecutionMaxTimeQueryString;

    public long QueryCacheHitCount => this.queryCacheHitCount;

    public long QueryCacheMissCount => this.queryCacheMissCount;

    public long QueryCachePutCount => this.queryCachePutCount;

    public long FlushCount => this.flushCount;

    public long ConnectCount => this.connectCount;

    public long SecondLevelCacheHitCount => this.secondLevelCacheHitCount;

    public long SecondLevelCacheMissCount => this.secondLevelCacheMissCount;

    public long SecondLevelCachePutCount => this.secondLevelCachePutCount;

    public long SessionCloseCount => this.sessionCloseCount;

    public long SessionOpenCount => this.sessionOpenCount;

    public long CollectionLoadCount => this.collectionLoadCount;

    public long CollectionFetchCount => this.collectionFetchCount;

    public long CollectionUpdateCount => this.collectionUpdateCount;

    public long CollectionRemoveCount => this.collectionRemoveCount;

    public long CollectionRecreateCount => this.collectionRecreateCount;

    public DateTime StartTime => this.startTime;

    public bool IsStatisticsEnabled { get; set; }

    public string[] Queries
    {
      get
      {
        string[] array = new string[this.queryStatistics.Keys.Count];
        this.queryStatistics.Keys.CopyTo(array, 0);
        return array;
      }
    }

    public string[] EntityNames
    {
      get
      {
        if (this.sessionFactory != null)
          return ArrayHelper.ToStringArray(this.sessionFactory.GetAllClassMetadata().Keys);
        string[] array = new string[this.entityStatistics.Keys.Count];
        this.entityStatistics.Keys.CopyTo(array, 0);
        return array;
      }
    }

    public string[] CollectionRoleNames
    {
      get
      {
        if (this.sessionFactory == null)
        {
          string[] array = new string[this.collectionStatistics.Keys.Count];
          this.collectionStatistics.Keys.CopyTo(array, 0);
          return array;
        }
        ICollection<string> keys = this.sessionFactory.GetAllCollectionMetadata().Keys;
        string[] array1 = new string[keys.Count];
        keys.CopyTo(array1, 0);
        return array1;
      }
    }

    public string[] SecondLevelCacheRegionNames
    {
      get
      {
        if (this.sessionFactory != null)
          return ArrayHelper.ToStringArray(this.sessionFactory.GetAllSecondLevelCacheRegions().Keys);
        string[] array = new string[this.secondLevelCacheStatistics.Keys.Count];
        this.secondLevelCacheStatistics.Keys.CopyTo(array, 0);
        return array;
      }
    }

    public long SuccessfulTransactionCount => this.commitedTransactionCount;

    public long TransactionCount => this.transactionCount;

    public long PrepareStatementCount => this.prepareStatementCount;

    public long CloseStatementCount => this.closeStatementCount;

    public long OptimisticFailureCount => this.optimisticFailureCount;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Clear()
    {
      lock (this.SyncRoot)
      {
        this.secondLevelCacheHitCount = 0L;
        this.secondLevelCacheMissCount = 0L;
        this.secondLevelCachePutCount = 0L;
        this.sessionCloseCount = 0L;
        this.sessionOpenCount = 0L;
        this.flushCount = 0L;
        this.connectCount = 0L;
        this.prepareStatementCount = 0L;
        this.closeStatementCount = 0L;
        this.entityDeleteCount = 0L;
        this.entityInsertCount = 0L;
        this.entityUpdateCount = 0L;
        this.entityLoadCount = 0L;
        this.entityFetchCount = 0L;
        this.collectionRemoveCount = 0L;
        this.collectionUpdateCount = 0L;
        this.collectionRecreateCount = 0L;
        this.collectionLoadCount = 0L;
        this.collectionFetchCount = 0L;
        this.queryExecutionCount = 0L;
        this.queryCacheHitCount = 0L;
        this.queryExecutionMaxTime = TimeSpan.MinValue;
        this.queryExecutionMaxTimeQueryString = (string) null;
        this.queryCacheMissCount = 0L;
        this.queryCachePutCount = 0L;
        this.transactionCount = 0L;
        this.commitedTransactionCount = 0L;
        this.optimisticFailureCount = 0L;
        this.secondLevelCacheStatistics.Clear();
        this.entityStatistics.Clear();
        this.collectionStatistics.Clear();
        this.queryStatistics.Clear();
        this.startTime = DateTime.Now;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public EntityStatistics GetEntityStatistics(string entityName)
    {
      lock (this.SyncRoot)
      {
        EntityStatistics entityStatistics;
        if (!this.entityStatistics.TryGetValue(entityName, out entityStatistics))
        {
          entityStatistics = new EntityStatistics(entityName);
          this.entityStatistics[entityName] = entityStatistics;
        }
        return entityStatistics;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public CollectionStatistics GetCollectionStatistics(string role)
    {
      lock (this.SyncRoot)
      {
        CollectionStatistics collectionStatistics;
        if (!this.collectionStatistics.TryGetValue(role, out collectionStatistics))
        {
          collectionStatistics = new CollectionStatistics(role);
          this.collectionStatistics[role] = collectionStatistics;
        }
        return collectionStatistics;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public SecondLevelCacheStatistics GetSecondLevelCacheStatistics(string regionName)
    {
      lock (this.SyncRoot)
      {
        SecondLevelCacheStatistics levelCacheStatistics;
        if (!this.secondLevelCacheStatistics.TryGetValue(regionName, out levelCacheStatistics))
        {
          if (this.sessionFactory == null)
            return (SecondLevelCacheStatistics) null;
          ICache levelCacheRegion = this.sessionFactory.GetSecondLevelCacheRegion(regionName);
          if (levelCacheRegion == null)
            return (SecondLevelCacheStatistics) null;
          levelCacheStatistics = new SecondLevelCacheStatistics(levelCacheRegion);
          this.secondLevelCacheStatistics[regionName] = levelCacheStatistics;
        }
        return levelCacheStatistics;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public QueryStatistics GetQueryStatistics(string queryString)
    {
      lock (this.SyncRoot)
      {
        QueryStatistics queryStatistics;
        if (!this.queryStatistics.TryGetValue(queryString, out queryStatistics))
        {
          queryStatistics = new QueryStatistics(queryString);
          this.queryStatistics[queryString] = queryStatistics;
        }
        return queryStatistics;
      }
    }

    public void LogSummary()
    {
      StatisticsImpl.log.Info((object) "Logging statistics....");
      StatisticsImpl.log.Info((object) string.Format("start time: {0}", (object) this.startTime.ToString("o")));
      StatisticsImpl.log.Info((object) ("sessions opened: " + (object) this.sessionOpenCount));
      StatisticsImpl.log.Info((object) ("sessions closed: " + (object) this.sessionCloseCount));
      StatisticsImpl.log.Info((object) ("transactions: " + (object) this.transactionCount));
      StatisticsImpl.log.Info((object) ("successful transactions: " + (object) this.commitedTransactionCount));
      StatisticsImpl.log.Info((object) ("optimistic lock failures: " + (object) this.optimisticFailureCount));
      StatisticsImpl.log.Info((object) ("flushes: " + (object) this.flushCount));
      StatisticsImpl.log.Info((object) ("connections obtained: " + (object) this.connectCount));
      StatisticsImpl.log.Info((object) ("statements prepared: " + (object) this.prepareStatementCount));
      StatisticsImpl.log.Info((object) ("statements closed: " + (object) this.closeStatementCount));
      StatisticsImpl.log.Info((object) ("second level cache puts: " + (object) this.secondLevelCachePutCount));
      StatisticsImpl.log.Info((object) ("second level cache hits: " + (object) this.secondLevelCacheHitCount));
      StatisticsImpl.log.Info((object) ("second level cache misses: " + (object) this.secondLevelCacheMissCount));
      StatisticsImpl.log.Info((object) ("entities loaded: " + (object) this.entityLoadCount));
      StatisticsImpl.log.Info((object) ("entities updated: " + (object) this.entityUpdateCount));
      StatisticsImpl.log.Info((object) ("entities inserted: " + (object) this.entityInsertCount));
      StatisticsImpl.log.Info((object) ("entities deleted: " + (object) this.entityDeleteCount));
      StatisticsImpl.log.Info((object) ("entities fetched (minimize this): " + (object) this.entityFetchCount));
      StatisticsImpl.log.Info((object) ("collections loaded: " + (object) this.collectionLoadCount));
      StatisticsImpl.log.Info((object) ("collections updated: " + (object) this.collectionUpdateCount));
      StatisticsImpl.log.Info((object) ("collections removed: " + (object) this.collectionRemoveCount));
      StatisticsImpl.log.Info((object) ("collections recreated: " + (object) this.collectionRecreateCount));
      StatisticsImpl.log.Info((object) ("collections fetched (minimize this): " + (object) this.collectionFetchCount));
      StatisticsImpl.log.Info((object) ("queries executed to database: " + (object) this.queryExecutionCount));
      StatisticsImpl.log.Info((object) ("query cache puts: " + (object) this.queryCachePutCount));
      StatisticsImpl.log.Info((object) ("query cache hits: " + (object) this.queryCacheHitCount));
      StatisticsImpl.log.Info((object) ("query cache misses: " + (object) this.queryCacheMissCount));
      StatisticsImpl.log.Info((object) ("max query time: " + (object) this.queryExecutionMaxTime.Milliseconds + "ms"));
    }

    public TimeSpan OperationThreshold
    {
      get => this.operationThreshold;
      [MethodImpl(MethodImplOptions.Synchronized)] set
      {
        lock (this.SyncRoot)
          this.operationThreshold = value;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void OpenSession()
    {
      lock (this.SyncRoot)
        ++this.sessionOpenCount;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void CloseSession()
    {
      lock (this.SyncRoot)
        ++this.sessionCloseCount;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Flush()
    {
      lock (this.SyncRoot)
        ++this.flushCount;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Connect()
    {
      lock (this.SyncRoot)
        ++this.connectCount;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void LoadEntity(string entityName, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.entityLoadCount;
        ++this.GetEntityStatistics(entityName).loadCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("load ", entityName, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void FetchEntity(string entityName, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.entityFetchCount;
        ++this.GetEntityStatistics(entityName).fetchCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("load ", entityName, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateEntity(string entityName, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.entityUpdateCount;
        ++this.GetEntityStatistics(entityName).updateCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("update ", entityName, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void InsertEntity(string entityName, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.entityInsertCount;
        ++this.GetEntityStatistics(entityName).insertCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("insert ", entityName, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void DeleteEntity(string entityName, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.entityDeleteCount;
        ++this.GetEntityStatistics(entityName).deleteCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("delete ", entityName, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void LoadCollection(string role, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.collectionLoadCount;
        ++this.GetCollectionStatistics(role).loadCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("loadCollection ", role, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void FetchCollection(string role, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.collectionFetchCount;
        ++this.GetCollectionStatistics(role).fetchCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("fetchCollection ", role, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateCollection(string role, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.collectionUpdateCount;
        ++this.GetCollectionStatistics(role).updateCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("updateCollection ", role, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void RecreateCollection(string role, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.collectionRecreateCount;
        ++this.GetCollectionStatistics(role).recreateCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("recreateCollection ", role, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void RemoveCollection(string role, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.collectionRemoveCount;
        ++this.GetCollectionStatistics(role).removeCount;
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("recreateCollection ", role, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SecondLevelCachePut(string regionName)
    {
      lock (this.SyncRoot)
      {
        SecondLevelCacheStatistics levelCacheStatistics = this.GetSecondLevelCacheStatistics(regionName);
        if (levelCacheStatistics == null)
          return;
        ++this.secondLevelCachePutCount;
        ++levelCacheStatistics.putCount;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SecondLevelCacheHit(string regionName)
    {
      lock (this.SyncRoot)
      {
        SecondLevelCacheStatistics levelCacheStatistics = this.GetSecondLevelCacheStatistics(regionName);
        if (levelCacheStatistics == null)
          return;
        ++this.secondLevelCacheHitCount;
        ++levelCacheStatistics.hitCount;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void SecondLevelCacheMiss(string regionName)
    {
      lock (this.SyncRoot)
      {
        SecondLevelCacheStatistics levelCacheStatistics = this.GetSecondLevelCacheStatistics(regionName);
        if (levelCacheStatistics == null)
          return;
        ++this.secondLevelCacheMissCount;
        ++levelCacheStatistics.missCount;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void QueryExecuted(string hql, int rows, TimeSpan time)
    {
      lock (this.SyncRoot)
      {
        ++this.queryExecutionCount;
        if (this.queryExecutionMaxTime < time)
        {
          this.queryExecutionMaxTime = time;
          this.queryExecutionMaxTimeQueryString = hql;
        }
        if (hql != null)
          this.GetQueryStatistics(hql).Executed((long) rows, time);
      }
      if (!(this.operationThreshold < time))
        return;
      StatisticsImpl.LogOperation("executeQuery ", hql, time);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void QueryCacheHit(string hql, string regionName)
    {
      lock (this.SyncRoot)
      {
        ++this.queryCacheHitCount;
        if (hql != null)
          ++this.GetQueryStatistics(hql).cacheHitCount;
        SecondLevelCacheStatistics levelCacheStatistics = this.GetSecondLevelCacheStatistics(regionName);
        if (levelCacheStatistics == null)
          return;
        ++levelCacheStatistics.hitCount;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void QueryCacheMiss(string hql, string regionName)
    {
      lock (this.SyncRoot)
      {
        ++this.queryCacheMissCount;
        if (hql != null)
          ++this.GetQueryStatistics(hql).cacheMissCount;
        SecondLevelCacheStatistics levelCacheStatistics = this.GetSecondLevelCacheStatistics(regionName);
        if (levelCacheStatistics == null)
          return;
        ++levelCacheStatistics.missCount;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void QueryCachePut(string hql, string regionName)
    {
      lock (this.SyncRoot)
      {
        ++this.queryCachePutCount;
        if (hql != null)
          ++this.GetQueryStatistics(hql).cachePutCount;
        SecondLevelCacheStatistics levelCacheStatistics = this.GetSecondLevelCacheStatistics(regionName);
        if (levelCacheStatistics == null)
          return;
        ++levelCacheStatistics.putCount;
      }
    }

    public void EndTransaction(bool success)
    {
      ++this.transactionCount;
      if (!success)
        return;
      ++this.commitedTransactionCount;
    }

    public void CloseStatement() => ++this.closeStatementCount;

    public void PrepareStatement() => ++this.prepareStatementCount;

    public void OptimisticFailure(string entityName)
    {
      ++this.optimisticFailureCount;
      ++this.GetEntityStatistics(entityName).optimisticFailureCount;
    }

    private static void LogOperation(string operation, string entityName, TimeSpan time)
    {
      if (entityName != null)
        StatisticsImpl.log.Info((object) (operation + entityName + " " + (object) time.Milliseconds + "ms"));
      else
        StatisticsImpl.log.Info((object) operation);
    }

    public override string ToString()
    {
      return new StringBuilder().Append("Statistics[").Append("start time=").Append((object) this.startTime).Append(",sessions opened=").Append(this.sessionOpenCount).Append(",sessions closed=").Append(this.sessionCloseCount).Append(",transactions=").Append(this.transactionCount).Append(",successful transactions=").Append(this.commitedTransactionCount).Append(",optimistic lock failures=").Append(this.optimisticFailureCount).Append(",flushes=").Append(this.flushCount).Append(",connections obtained=").Append(this.connectCount).Append(",statements prepared=").Append(this.prepareStatementCount).Append(",statements closed=").Append(this.closeStatementCount).Append(",second level cache puts=").Append(this.secondLevelCachePutCount).Append(",second level cache hits=").Append(this.secondLevelCacheHitCount).Append(",second level cache misses=").Append(this.secondLevelCacheMissCount).Append(",entities loaded=").Append(this.entityLoadCount).Append(",entities updated=").Append(this.entityUpdateCount).Append(",entities inserted=").Append(this.entityInsertCount).Append(",entities deleted=").Append(this.entityDeleteCount).Append(",entities fetched=").Append(this.entityFetchCount).Append(",collections loaded=").Append(this.collectionLoadCount).Append(",collections updated=").Append(this.collectionUpdateCount).Append(",collections removed=").Append(this.collectionRemoveCount).Append(",collections recreated=").Append(this.collectionRecreateCount).Append(",collections fetched=").Append(this.collectionFetchCount).Append(",queries executed to database=").Append(this.queryExecutionCount).Append(",query cache puts=").Append(this.queryCachePutCount).Append(",query cache hits=").Append(this.queryCacheHitCount).Append(",query cache misses=").Append(this.queryCacheMissCount).Append(",max query time=").Append((object) this.queryExecutionMaxTime).Append(']').ToString();
    }
  }
}
