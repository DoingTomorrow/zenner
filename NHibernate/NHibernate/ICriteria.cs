// Decompiled with JetBrains decompiler
// Type: NHibernate.ICriteria
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate
{
  public interface ICriteria : ICloneable
  {
    string Alias { get; }

    bool IsReadOnlyInitialized { get; }

    bool IsReadOnly { get; }

    ICriteria SetProjection(params IProjection[] projection);

    ICriteria Add(ICriterion expression);

    ICriteria AddOrder(Order order);

    ICriteria SetFetchMode(string associationPath, FetchMode mode);

    ICriteria SetLockMode(LockMode lockMode);

    ICriteria SetLockMode(string alias, LockMode lockMode);

    ICriteria CreateAlias(string associationPath, string alias);

    ICriteria CreateAlias(string associationPath, string alias, JoinType joinType);

    ICriteria CreateAlias(
      string associationPath,
      string alias,
      JoinType joinType,
      ICriterion withClause);

    ICriteria CreateCriteria(string associationPath);

    ICriteria CreateCriteria(string associationPath, JoinType joinType);

    ICriteria CreateCriteria(string associationPath, string alias);

    ICriteria CreateCriteria(string associationPath, string alias, JoinType joinType);

    ICriteria CreateCriteria(
      string associationPath,
      string alias,
      JoinType joinType,
      ICriterion withClause);

    ICriteria SetResultTransformer(IResultTransformer resultTransformer);

    ICriteria SetMaxResults(int maxResults);

    ICriteria SetFirstResult(int firstResult);

    ICriteria SetFetchSize(int fetchSize);

    ICriteria SetTimeout(int timeout);

    ICriteria SetCacheable(bool cacheable);

    ICriteria SetCacheRegion(string cacheRegion);

    ICriteria SetComment(string comment);

    ICriteria SetFlushMode(FlushMode flushMode);

    ICriteria SetCacheMode(CacheMode cacheMode);

    IList List();

    object UniqueResult();

    IEnumerable<T> Future<T>();

    IFutureValue<T> FutureValue<T>();

    ICriteria SetReadOnly(bool readOnly);

    void List(IList results);

    IList<T> List<T>();

    T UniqueResult<T>();

    void ClearOrders();

    ICriteria GetCriteriaByPath(string path);

    ICriteria GetCriteriaByAlias(string alias);

    Type GetRootEntityTypeIfAvailable();
  }
}
