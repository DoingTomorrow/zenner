// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.DetachedCriteria
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class DetachedCriteria
  {
    private readonly CriteriaImpl impl;
    private readonly ICriteria criteria;

    protected DetachedCriteria(Type entityType)
    {
      this.impl = new CriteriaImpl(entityType, (ISessionImplementor) null);
      this.criteria = (ICriteria) this.impl;
    }

    protected DetachedCriteria(Type entityType, string alias)
    {
      this.impl = new CriteriaImpl(entityType, alias, (ISessionImplementor) null);
      this.criteria = (ICriteria) this.impl;
    }

    protected DetachedCriteria(string entityName)
    {
      this.impl = new CriteriaImpl(entityName, (ISessionImplementor) null);
      this.criteria = (ICriteria) this.impl;
    }

    protected DetachedCriteria(string entityName, string alias)
    {
      this.impl = new CriteriaImpl(entityName, alias, (ISessionImplementor) null);
      this.criteria = (ICriteria) this.impl;
    }

    protected internal DetachedCriteria(CriteriaImpl impl, ICriteria criteria)
    {
      this.impl = impl;
      this.criteria = criteria;
    }

    internal DetachedCriteria(CriteriaImpl impl)
    {
      this.impl = impl;
      this.criteria = (ICriteria) impl;
    }

    public ICriteria GetExecutableCriteria(ISession session)
    {
      this.impl.Session = session.GetSessionImplementation();
      return (ICriteria) this.impl;
    }

    public ICriteria GetExecutableCriteria(IStatelessSession session)
    {
      this.impl.Session = session.GetSessionImplementation();
      return (ICriteria) this.impl;
    }

    public static DetachedCriteria For(Type entityType) => new DetachedCriteria(entityType);

    public static DetachedCriteria For<T>() => new DetachedCriteria(typeof (T));

    public static DetachedCriteria For<T>(string alias) => new DetachedCriteria(typeof (T), alias);

    public static DetachedCriteria For(Type entityType, string alias)
    {
      return new DetachedCriteria(entityType, alias);
    }

    public static DetachedCriteria ForEntityName(string entityName)
    {
      return new DetachedCriteria(entityName);
    }

    public static DetachedCriteria ForEntityName(string entityName, string alias)
    {
      return new DetachedCriteria(entityName, alias);
    }

    public DetachedCriteria Add(ICriterion criterion)
    {
      this.criteria.Add(criterion);
      return this;
    }

    public DetachedCriteria AddOrder(Order order)
    {
      this.criteria.AddOrder(order);
      return this;
    }

    public DetachedCriteria CreateAlias(string associationPath, string alias)
    {
      this.criteria.CreateAlias(associationPath, alias);
      return this;
    }

    public DetachedCriteria CreateAlias(string associationPath, string alias, JoinType joinType)
    {
      this.criteria.CreateAlias(associationPath, alias, joinType);
      return this;
    }

    public DetachedCriteria CreateAlias(
      string associationPath,
      string alias,
      JoinType joinType,
      ICriterion withClause)
    {
      this.criteria.CreateAlias(associationPath, alias, joinType, withClause);
      return this;
    }

    public DetachedCriteria CreateCriteria(string associationPath, string alias)
    {
      return new DetachedCriteria(this.impl, this.criteria.CreateCriteria(associationPath, alias));
    }

    public DetachedCriteria CreateCriteria(string associationPath)
    {
      return new DetachedCriteria(this.impl, this.criteria.CreateCriteria(associationPath));
    }

    public DetachedCriteria CreateCriteria(string associationPath, JoinType joinType)
    {
      return new DetachedCriteria(this.impl, this.criteria.CreateCriteria(associationPath, joinType));
    }

    public DetachedCriteria CreateCriteria(string associationPath, string alias, JoinType joinType)
    {
      return new DetachedCriteria(this.impl, this.criteria.CreateCriteria(associationPath, alias, joinType));
    }

    public DetachedCriteria CreateCriteria(
      string associationPath,
      string alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return new DetachedCriteria(this.impl, this.criteria.CreateCriteria(associationPath, alias, joinType, withClause));
    }

    public string Alias => this.criteria.Alias;

    public string EntityOrClassName => this.impl.EntityOrClassName;

    protected internal CriteriaImpl GetCriteriaImpl() => this.impl;

    public DetachedCriteria SetFetchMode(string associationPath, FetchMode mode)
    {
      this.criteria.SetFetchMode(associationPath, mode);
      return this;
    }

    public DetachedCriteria SetLockMode(LockMode lockMode)
    {
      this.criteria.SetLockMode(lockMode);
      return this;
    }

    public DetachedCriteria SetLockMode(string alias, LockMode lockMode)
    {
      this.criteria.SetLockMode(alias, lockMode);
      return this;
    }

    public DetachedCriteria SetCacheMode(CacheMode cacheMode)
    {
      this.criteria.SetCacheMode(cacheMode);
      return this;
    }

    public DetachedCriteria SetCacheRegion(string region)
    {
      this.criteria.SetCacheRegion(region);
      return this;
    }

    public DetachedCriteria SetCacheable(bool cacheable)
    {
      this.criteria.SetCacheable(cacheable);
      return this;
    }

    public DetachedCriteria SetProjection(IProjection projection)
    {
      this.criteria.SetProjection(projection);
      return this;
    }

    public DetachedCriteria SetResultTransformer(IResultTransformer resultTransformer)
    {
      this.criteria.SetResultTransformer(resultTransformer);
      return this;
    }

    public DetachedCriteria SetFirstResult(int firstResult)
    {
      this.criteria.SetFirstResult(firstResult);
      return this;
    }

    public DetachedCriteria SetMaxResults(int maxResults)
    {
      this.criteria.SetMaxResults(maxResults);
      return this;
    }

    public override string ToString()
    {
      return string.Format("DetachableCriteria({0})", (object) this.criteria);
    }

    public DetachedCriteria GetCriteriaByPath(string path)
    {
      ICriteria criteriaByPath = this.criteria.GetCriteriaByPath(path);
      return criteriaByPath == null ? (DetachedCriteria) null : new DetachedCriteria(this.impl, criteriaByPath);
    }

    public DetachedCriteria GetCriteriaByAlias(string alias)
    {
      ICriteria criteriaByAlias = this.criteria.GetCriteriaByAlias(alias);
      return criteriaByAlias == null ? (DetachedCriteria) null : new DetachedCriteria(this.impl, criteriaByAlias);
    }

    public Type GetRootEntityTypeIfAvailable() => this.criteria.GetRootEntityTypeIfAvailable();

    public void ClearOrders() => this.criteria.ClearOrders();
  }
}
