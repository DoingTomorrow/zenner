// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.ISessionImplementor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.Collection;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Impl;
using NHibernate.Loader.Custom;
using NHibernate.Persister.Entity;
using NHibernate.Transaction;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Engine
{
  public interface ISessionImplementor
  {
    void Initialize();

    void InitializeCollection(IPersistentCollection collection, bool writing);

    object InternalLoad(string entityName, object id, bool eager, bool isNullable);

    object ImmediateLoad(string entityName, object id);

    long Timestamp { get; }

    ISessionFactoryImplementor Factory { get; }

    IBatcher Batcher { get; }

    IList List(string query, QueryParameters parameters);

    IList List(IQueryExpression queryExpression, QueryParameters parameters);

    IQuery CreateQuery(IQueryExpression queryExpression);

    void List(string query, QueryParameters parameters, IList results);

    IList<T> List<T>(string query, QueryParameters queryParameters);

    IList<T> List<T>(CriteriaImpl criteria);

    void List(CriteriaImpl criteria, IList results);

    IList List(CriteriaImpl criteria);

    IEnumerable Enumerable(string query, QueryParameters parameters);

    IEnumerable<T> Enumerable<T>(string query, QueryParameters queryParameters);

    IList ListFilter(object collection, string filter, QueryParameters parameters);

    IList<T> ListFilter<T>(object collection, string filter, QueryParameters parameters);

    IEnumerable EnumerableFilter(object collection, string filter, QueryParameters parameters);

    IEnumerable<T> EnumerableFilter<T>(
      object collection,
      string filter,
      QueryParameters parameters);

    IEntityPersister GetEntityPersister(string entityName, object obj);

    void AfterTransactionBegin(ITransaction tx);

    void BeforeTransactionCompletion(ITransaction tx);

    void AfterTransactionCompletion(bool successful, ITransaction tx);

    object GetContextEntityIdentifier(object obj);

    object Instantiate(string entityName, object id);

    IList List(NativeSQLQuerySpecification spec, QueryParameters queryParameters);

    void List(NativeSQLQuerySpecification spec, QueryParameters queryParameters, IList results);

    IList<T> List<T>(NativeSQLQuerySpecification spec, QueryParameters queryParameters);

    void ListCustomQuery(ICustomQuery customQuery, QueryParameters queryParameters, IList results);

    IList<T> ListCustomQuery<T>(ICustomQuery customQuery, QueryParameters queryParameters);

    object GetFilterParameterValue(string filterParameterName);

    IType GetFilterParameterType(string filterParameterName);

    IDictionary<string, NHibernate.IFilter> EnabledFilters { get; }

    IQuery GetNamedSQLQuery(string name);

    IQueryTranslator[] GetQueries(string query, bool scalar);

    IInterceptor Interceptor { get; }

    EventListeners Listeners { get; }

    int DontFlushFromFind { get; }

    ConnectionManager ConnectionManager { get; }

    bool IsEventSource { get; }

    object GetEntityUsingInterceptor(EntityKey key);

    IPersistenceContext PersistenceContext { get; }

    CacheMode CacheMode { get; set; }

    bool IsOpen { get; }

    bool IsConnected { get; }

    FlushMode FlushMode { get; set; }

    string FetchProfile { get; set; }

    string BestGuessEntityName(object entity);

    string GuessEntityName(object entity);

    IDbConnection Connection { get; }

    IQuery GetNamedQuery(string queryName);

    bool IsClosed { get; }

    void Flush();

    bool TransactionInProgress { get; }

    EntityMode EntityMode { get; }

    int ExecuteNativeUpdate(
      NativeSQLQuerySpecification specification,
      QueryParameters queryParameters);

    int ExecuteUpdate(string query, QueryParameters queryParameters);

    FutureCriteriaBatch FutureCriteriaBatch { get; }

    FutureQueryBatch FutureQueryBatch { get; }

    Guid SessionId { get; }

    ITransactionContext TransactionContext { get; set; }

    void CloseSessionFromDistributedTransaction();
  }
}
