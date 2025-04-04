// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.QueryOver`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public abstract class QueryOver<TRoot> : QueryOver, IQueryOver<TRoot>, IQueryOver
  {
    private IList<TRoot> List() => this.criteria.List<TRoot>();

    private IList<U> List<U>() => this.criteria.List<U>();

    private TRoot SingleOrDefault() => this.criteria.UniqueResult<TRoot>();

    private U SingleOrDefault<U>() => this.criteria.UniqueResult<U>();

    private IEnumerable<TRoot> Future() => this.criteria.Future<TRoot>();

    private IEnumerable<U> Future<U>() => this.criteria.Future<U>();

    private IFutureValue<TRoot> FutureValue() => this.criteria.FutureValue<TRoot>();

    private IFutureValue<U> FutureValue<U>() => this.criteria.FutureValue<U>();

    public IQueryOver<TRoot, TRoot> GetExecutableQueryOver(ISession session)
    {
      this.impl.Session = session.GetSessionImplementation();
      return (IQueryOver<TRoot, TRoot>) new QueryOver<TRoot, TRoot>(this.impl);
    }

    public IQueryOver<TRoot, TRoot> GetExecutableQueryOver(IStatelessSession session)
    {
      this.impl.Session = (ISessionImplementor) session;
      return (IQueryOver<TRoot, TRoot>) new QueryOver<TRoot, TRoot>(this.impl);
    }

    public QueryOver<TRoot, TRoot> ToRowCountQuery()
    {
      return (QueryOver<TRoot, TRoot>) this.Clone().Select(new IProjection[1]
      {
        Projections.RowCount()
      }).ClearOrders().Skip(0).Take(RowSelection.NoValue);
    }

    public QueryOver<TRoot, TRoot> ToRowCountInt64Query()
    {
      return (QueryOver<TRoot, TRoot>) this.Clone().Select(new IProjection[1]
      {
        Projections.RowCountInt64()
      }).ClearOrders().Skip(0).Take(RowSelection.NoValue);
    }

    public QueryOver<TRoot, TRoot> Clone()
    {
      return new QueryOver<TRoot, TRoot>((CriteriaImpl) this.criteria.Clone());
    }

    public QueryOver<TRoot> ClearOrders()
    {
      this.criteria.ClearOrders();
      return this;
    }

    public QueryOver<TRoot> Skip(int firstResult)
    {
      this.criteria.SetFirstResult(firstResult);
      return this;
    }

    public QueryOver<TRoot> Take(int maxResults)
    {
      this.criteria.SetMaxResults(maxResults);
      return this;
    }

    public QueryOver<TRoot> Cacheable()
    {
      this.criteria.SetCacheable(true);
      return this;
    }

    public QueryOver<TRoot> CacheMode(NHibernate.CacheMode cacheMode)
    {
      this.criteria.SetCacheMode(cacheMode);
      return this;
    }

    public QueryOver<TRoot> CacheRegion(string cacheRegion)
    {
      this.criteria.SetCacheRegion(cacheRegion);
      return this;
    }

    private QueryOver<TRoot> ReadOnly()
    {
      this.criteria.SetReadOnly(true);
      return this;
    }

    public S As<S>()
    {
      throw new HibernateException("Incorrect syntax;  .As<T> method is for use in Lambda expressions only.");
    }

    IList<TRoot> IQueryOver<TRoot>.List() => this.List();

    IList<U> IQueryOver<TRoot>.List<U>() => this.List<U>();

    IQueryOver<TRoot, TRoot> IQueryOver<TRoot>.ToRowCountQuery()
    {
      return (IQueryOver<TRoot, TRoot>) this.ToRowCountQuery();
    }

    IQueryOver<TRoot, TRoot> IQueryOver<TRoot>.ToRowCountInt64Query()
    {
      return (IQueryOver<TRoot, TRoot>) this.ToRowCountInt64Query();
    }

    int IQueryOver<TRoot>.RowCount() => this.ToRowCountQuery().SingleOrDefault<int>();

    long IQueryOver<TRoot>.RowCountInt64() => this.ToRowCountInt64Query().SingleOrDefault<long>();

    TRoot IQueryOver<TRoot>.SingleOrDefault() => this.SingleOrDefault();

    U IQueryOver<TRoot>.SingleOrDefault<U>() => this.SingleOrDefault<U>();

    IEnumerable<TRoot> IQueryOver<TRoot>.Future() => this.Future();

    IEnumerable<U> IQueryOver<TRoot>.Future<U>() => this.Future<U>();

    IFutureValue<TRoot> IQueryOver<TRoot>.FutureValue() => this.FutureValue();

    IFutureValue<U> IQueryOver<TRoot>.FutureValue<U>() => this.FutureValue<U>();

    IQueryOver<TRoot, TRoot> IQueryOver<TRoot>.Clone() => (IQueryOver<TRoot, TRoot>) this.Clone();

    IQueryOver<TRoot> IQueryOver<TRoot>.ClearOrders() => (IQueryOver<TRoot>) this.ClearOrders();

    IQueryOver<TRoot> IQueryOver<TRoot>.Skip(int firstResult)
    {
      return (IQueryOver<TRoot>) this.Skip(firstResult);
    }

    IQueryOver<TRoot> IQueryOver<TRoot>.Take(int maxResults)
    {
      return (IQueryOver<TRoot>) this.Take(maxResults);
    }

    IQueryOver<TRoot> IQueryOver<TRoot>.Cacheable() => (IQueryOver<TRoot>) this.Cacheable();

    IQueryOver<TRoot> IQueryOver<TRoot>.CacheMode(NHibernate.CacheMode cacheMode)
    {
      return (IQueryOver<TRoot>) this.CacheMode(cacheMode);
    }

    IQueryOver<TRoot> IQueryOver<TRoot>.CacheRegion(string cacheRegion)
    {
      return (IQueryOver<TRoot>) this.CacheRegion(cacheRegion);
    }

    IQueryOver<TRoot> IQueryOver<TRoot>.ReadOnly() => (IQueryOver<TRoot>) this.ReadOnly();
  }
}
