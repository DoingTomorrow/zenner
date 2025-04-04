// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.QueryOver`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion.Lambda;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class QueryOver<TRoot, TSubType> : 
    QueryOver<TRoot>,
    IQueryOver<TRoot, TSubType>,
    IQueryOver<TRoot>,
    IQueryOver
  {
    protected internal QueryOver()
    {
      this.impl = new CriteriaImpl(typeof (TRoot), (ISessionImplementor) null);
      this.criteria = (ICriteria) this.impl;
    }

    protected internal QueryOver(string entityName)
    {
      this.impl = new CriteriaImpl(entityName, (ISessionImplementor) null);
      this.criteria = (ICriteria) this.impl;
    }

    protected internal QueryOver(Expression<Func<TSubType>> alias)
    {
      this.impl = new CriteriaImpl(typeof (TRoot), ExpressionProcessor.FindMemberExpression(alias.Body), (ISessionImplementor) null);
      this.criteria = (ICriteria) this.impl;
    }

    protected internal QueryOver(string entityName, Expression<Func<TSubType>> alias)
    {
      string memberExpression = ExpressionProcessor.FindMemberExpression(alias.Body);
      this.impl = new CriteriaImpl(entityName, memberExpression, (ISessionImplementor) null);
      this.criteria = (ICriteria) this.impl;
    }

    protected internal QueryOver(CriteriaImpl impl)
    {
      this.impl = impl;
      this.criteria = (ICriteria) impl;
    }

    protected internal QueryOver(CriteriaImpl rootImpl, ICriteria criteria)
    {
      this.impl = rootImpl;
      this.criteria = criteria;
    }

    public QueryOver<TRoot, TSubType> And(Expression<Func<TSubType, bool>> expression)
    {
      return this.Add(expression);
    }

    public QueryOver<TRoot, TSubType> And(Expression<Func<bool>> expression)
    {
      return this.Add(expression);
    }

    public QueryOver<TRoot, TSubType> And(ICriterion expression) => this.Add(expression);

    public QueryOver<TRoot, TSubType> AndNot(Expression<Func<TSubType, bool>> expression)
    {
      return this.AddNot(expression);
    }

    public QueryOver<TRoot, TSubType> AndNot(Expression<Func<bool>> expression)
    {
      return this.AddNot(expression);
    }

    public QueryOverRestrictionBuilder<TRoot, TSubType> AndRestrictionOn(
      Expression<Func<TSubType, object>> expression)
    {
      return new QueryOverRestrictionBuilder<TRoot, TSubType>(this, ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    public QueryOverRestrictionBuilder<TRoot, TSubType> AndRestrictionOn(
      Expression<Func<object>> expression)
    {
      return new QueryOverRestrictionBuilder<TRoot, TSubType>(this, ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    public QueryOver<TRoot, TSubType> Where(Expression<Func<TSubType, bool>> expression)
    {
      return this.Add(expression);
    }

    public QueryOver<TRoot, TSubType> Where(Expression<Func<bool>> expression)
    {
      return this.Add(expression);
    }

    public QueryOver<TRoot, TSubType> Where(ICriterion expression) => this.Add(expression);

    public QueryOver<TRoot, TSubType> WhereNot(Expression<Func<TSubType, bool>> expression)
    {
      return this.AddNot(expression);
    }

    public QueryOver<TRoot, TSubType> WhereNot(Expression<Func<bool>> expression)
    {
      return this.AddNot(expression);
    }

    public QueryOverRestrictionBuilder<TRoot, TSubType> WhereRestrictionOn(
      Expression<Func<TSubType, object>> expression)
    {
      return new QueryOverRestrictionBuilder<TRoot, TSubType>(this, ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    public QueryOverRestrictionBuilder<TRoot, TSubType> WhereRestrictionOn(
      Expression<Func<object>> expression)
    {
      return new QueryOverRestrictionBuilder<TRoot, TSubType>(this, ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    public QueryOver<TRoot, TSubType> Select(
      params Expression<Func<TRoot, object>>[] projections)
    {
      List<IProjection> projectionList = new List<IProjection>();
      foreach (Expression<Func<TRoot, object>> projection in projections)
        projectionList.Add(ExpressionProcessor.FindMemberProjection(projection.Body).AsProjection());
      this.criteria.SetProjection(projectionList.ToArray());
      return this;
    }

    public QueryOver<TRoot, TSubType> Select(params IProjection[] projections)
    {
      this.criteria.SetProjection(projections);
      return this;
    }

    public QueryOver<TRoot, TSubType> SelectList(
      Func<QueryOverProjectionBuilder<TRoot>, QueryOverProjectionBuilder<TRoot>> list)
    {
      this.criteria.SetProjection((IProjection) list(new QueryOverProjectionBuilder<TRoot>()).ProjectionList);
      return this;
    }

    public QueryOverOrderBuilder<TRoot, TSubType> OrderBy(Expression<Func<TSubType, object>> path)
    {
      return new QueryOverOrderBuilder<TRoot, TSubType>(this, path);
    }

    public QueryOverOrderBuilder<TRoot, TSubType> OrderBy(Expression<Func<object>> path)
    {
      return new QueryOverOrderBuilder<TRoot, TSubType>(this, path, false);
    }

    public QueryOverOrderBuilder<TRoot, TSubType> OrderBy(IProjection projection)
    {
      return new QueryOverOrderBuilder<TRoot, TSubType>(this, ExpressionProcessor.ProjectionInfo.ForProjection(projection));
    }

    public QueryOverOrderBuilder<TRoot, TSubType> OrderByAlias(Expression<Func<object>> path)
    {
      return new QueryOverOrderBuilder<TRoot, TSubType>(this, path, true);
    }

    public QueryOverOrderBuilder<TRoot, TSubType> ThenBy(Expression<Func<TSubType, object>> path)
    {
      return new QueryOverOrderBuilder<TRoot, TSubType>(this, path);
    }

    public QueryOverOrderBuilder<TRoot, TSubType> ThenBy(Expression<Func<object>> path)
    {
      return new QueryOverOrderBuilder<TRoot, TSubType>(this, path, false);
    }

    public QueryOverOrderBuilder<TRoot, TSubType> ThenBy(IProjection projection)
    {
      return new QueryOverOrderBuilder<TRoot, TSubType>(this, ExpressionProcessor.ProjectionInfo.ForProjection(projection));
    }

    public QueryOverOrderBuilder<TRoot, TSubType> ThenByAlias(Expression<Func<object>> path)
    {
      return new QueryOverOrderBuilder<TRoot, TSubType>(this, path, true);
    }

    public QueryOver<TRoot, TSubType> TransformUsing(IResultTransformer resultTransformer)
    {
      this.criteria.SetResultTransformer(resultTransformer);
      return this;
    }

    public QueryOverSubqueryBuilder<TRoot, TSubType> WithSubquery
    {
      get => new QueryOverSubqueryBuilder<TRoot, TSubType>(this);
    }

    public QueryOverFetchBuilder<TRoot, TSubType> Fetch(Expression<Func<TRoot, object>> path)
    {
      return new QueryOverFetchBuilder<TRoot, TSubType>(this, path);
    }

    public QueryOverLockBuilder<TRoot, TSubType> Lock()
    {
      return new QueryOverLockBuilder<TRoot, TSubType>(this, (Expression<Func<object>>) null);
    }

    public QueryOverLockBuilder<TRoot, TSubType> Lock(Expression<Func<object>> alias)
    {
      return new QueryOverLockBuilder<TRoot, TSubType>(this, alias);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body)));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body)));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body)));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, Expression<Func<U>> alias)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body)));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      JoinType joinType)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), joinType));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, JoinType joinType)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), joinType));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType, withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType, withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body)));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body)));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, IEnumerable<U>>> path)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body)));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body)));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType, withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType, withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      JoinType joinType)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), joinType));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      JoinType joinType)
    {
      return new QueryOver<TRoot, U>(this.impl, this.criteria.CreateCriteria(ExpressionProcessor.FindMemberExpression(path.Body), joinType));
    }

    public QueryOver<TRoot, TSubType> JoinAlias(
      Expression<Func<TSubType, object>> path,
      Expression<Func<object>> alias)
    {
      return this.AddAlias(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), JoinType.InnerJoin);
    }

    public QueryOver<TRoot, TSubType> JoinAlias(
      Expression<Func<object>> path,
      Expression<Func<object>> alias)
    {
      return this.AddAlias(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), JoinType.InnerJoin);
    }

    public QueryOver<TRoot, TSubType> JoinAlias(
      Expression<Func<TSubType, object>> path,
      Expression<Func<object>> alias,
      JoinType joinType)
    {
      return this.AddAlias(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType);
    }

    public QueryOver<TRoot, TSubType> JoinAlias<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return this.AddAlias(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType, withClause);
    }

    public QueryOver<TRoot, TSubType> JoinAlias<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return this.AddAlias(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType, withClause);
    }

    public QueryOver<TRoot, TSubType> JoinAlias(
      Expression<Func<object>> path,
      Expression<Func<object>> alias,
      JoinType joinType)
    {
      return this.AddAlias(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType);
    }

    public QueryOver<TRoot, TSubType> JoinAlias<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return this.AddAlias(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType, withClause);
    }

    public QueryOver<TRoot, TSubType> JoinAlias<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return this.AddAlias(ExpressionProcessor.FindMemberExpression(path.Body), ExpressionProcessor.FindMemberExpression(alias.Body), joinType, withClause);
    }

    public QueryOverJoinBuilder<TRoot, TSubType> Inner
    {
      get => new QueryOverJoinBuilder<TRoot, TSubType>(this, JoinType.InnerJoin);
    }

    public QueryOverJoinBuilder<TRoot, TSubType> Left
    {
      get => new QueryOverJoinBuilder<TRoot, TSubType>(this, JoinType.LeftOuterJoin);
    }

    public QueryOverJoinBuilder<TRoot, TSubType> Right
    {
      get => new QueryOverJoinBuilder<TRoot, TSubType>(this, JoinType.RightOuterJoin);
    }

    public QueryOverJoinBuilder<TRoot, TSubType> Full
    {
      get => new QueryOverJoinBuilder<TRoot, TSubType>(this, JoinType.FullJoin);
    }

    private QueryOver<TRoot, TSubType> AddAlias(string path, string alias, JoinType joinType)
    {
      this.criteria.CreateAlias(path, alias, joinType);
      return this;
    }

    private QueryOver<TRoot, TSubType> AddAlias(
      string path,
      string alias,
      JoinType joinType,
      ICriterion withClause)
    {
      this.criteria.CreateAlias(path, alias, joinType, withClause);
      return this;
    }

    private QueryOver<TRoot, TSubType> Add(Expression<Func<TSubType, bool>> expression)
    {
      this.criteria.Add(ExpressionProcessor.ProcessExpression<TSubType>(expression));
      return this;
    }

    private QueryOver<TRoot, TSubType> Add(Expression<Func<bool>> expression)
    {
      this.criteria.Add(ExpressionProcessor.ProcessExpression(expression));
      return this;
    }

    private QueryOver<TRoot, TSubType> Add(ICriterion expression)
    {
      this.criteria.Add(expression);
      return this;
    }

    private QueryOver<TRoot, TSubType> AddNot(Expression<Func<TSubType, bool>> expression)
    {
      this.criteria.Add((ICriterion) Restrictions.Not(ExpressionProcessor.ProcessExpression<TSubType>(expression)));
      return this;
    }

    private QueryOver<TRoot, TSubType> AddNot(Expression<Func<bool>> expression)
    {
      this.criteria.Add((ICriterion) Restrictions.Not(ExpressionProcessor.ProcessExpression(expression)));
      return this;
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.And(
      Expression<Func<TSubType, bool>> expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.And(expression);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.And(Expression<Func<bool>> expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.And(expression);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.And(ICriterion expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.And(expression);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.AndNot(
      Expression<Func<TSubType, bool>> expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.AndNot(expression);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.AndNot(Expression<Func<bool>> expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.AndNot(expression);
    }

    IQueryOverRestrictionBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.AndRestrictionOn(
      Expression<Func<TSubType, object>> expression)
    {
      return new IQueryOverRestrictionBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    IQueryOverRestrictionBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.AndRestrictionOn(
      Expression<Func<object>> expression)
    {
      return new IQueryOverRestrictionBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Where(
      Expression<Func<TSubType, bool>> expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.Where(expression);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Where(Expression<Func<bool>> expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.Where(expression);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Where(ICriterion expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.Where(expression);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.WhereNot(
      Expression<Func<TSubType, bool>> expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.WhereNot(expression);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.WhereNot(
      Expression<Func<bool>> expression)
    {
      return (IQueryOver<TRoot, TSubType>) this.WhereNot(expression);
    }

    IQueryOverRestrictionBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.WhereRestrictionOn(
      Expression<Func<TSubType, object>> expression)
    {
      return new IQueryOverRestrictionBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    IQueryOverRestrictionBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.WhereRestrictionOn(
      Expression<Func<object>> expression)
    {
      return new IQueryOverRestrictionBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Select(
      params Expression<Func<TRoot, object>>[] projections)
    {
      return (IQueryOver<TRoot, TSubType>) this.Select(projections);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Select(params IProjection[] projections)
    {
      return (IQueryOver<TRoot, TSubType>) this.Select(projections);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.SelectList(
      Func<QueryOverProjectionBuilder<TRoot>, QueryOverProjectionBuilder<TRoot>> list)
    {
      return (IQueryOver<TRoot, TSubType>) this.SelectList(list);
    }

    IQueryOverOrderBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.OrderBy(
      Expression<Func<TSubType, object>> path)
    {
      return new IQueryOverOrderBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, path);
    }

    IQueryOverOrderBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.OrderBy(
      Expression<Func<object>> path)
    {
      return new IQueryOverOrderBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, path, false);
    }

    IQueryOverOrderBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.OrderBy(
      IProjection projection)
    {
      return new IQueryOverOrderBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, ExpressionProcessor.ProjectionInfo.ForProjection(projection));
    }

    IQueryOverOrderBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.OrderByAlias(
      Expression<Func<object>> path)
    {
      return new IQueryOverOrderBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, path, true);
    }

    IQueryOverOrderBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.ThenBy(
      Expression<Func<TSubType, object>> path)
    {
      return new IQueryOverOrderBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, path);
    }

    IQueryOverOrderBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.ThenBy(
      Expression<Func<object>> path)
    {
      return new IQueryOverOrderBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, path, false);
    }

    IQueryOverOrderBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.ThenBy(
      IProjection projection)
    {
      return new IQueryOverOrderBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, ExpressionProcessor.ProjectionInfo.ForProjection(projection));
    }

    IQueryOverOrderBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.ThenByAlias(
      Expression<Func<object>> path)
    {
      return new IQueryOverOrderBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, path, true);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.TransformUsing(
      IResultTransformer resultTransformer)
    {
      return (IQueryOver<TRoot, TSubType>) this.TransformUsing(resultTransformer);
    }

    IQueryOverSubqueryBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.WithSubquery
    {
      get => new IQueryOverSubqueryBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this);
    }

    IQueryOverFetchBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Fetch(
      Expression<Func<TRoot, object>> path)
    {
      return new IQueryOverFetchBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, path);
    }

    IQueryOverLockBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Lock()
    {
      return new IQueryOverLockBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, (Expression<Func<object>>) null);
    }

    IQueryOverLockBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Lock(
      Expression<Func<object>> alias)
    {
      return new IQueryOverLockBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, alias);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(Expression<Func<U>> path)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, joinType);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<U>> path,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, joinType);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias, joinType);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias, joinType, withClause);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias, joinType);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias, joinType, withClause);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, joinType);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, joinType);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias, joinType);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias, joinType, withClause);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias, joinType);
    }

    IQueryOver<TRoot, U> IQueryOver<TRoot, TSubType>.JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (IQueryOver<TRoot, U>) this.JoinQueryOver<U>(path, alias, joinType, withClause);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.JoinAlias(
      Expression<Func<TSubType, object>> path,
      Expression<Func<object>> alias)
    {
      return (IQueryOver<TRoot, TSubType>) this.JoinAlias(path, alias);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.JoinAlias(
      Expression<Func<object>> path,
      Expression<Func<object>> alias)
    {
      return (IQueryOver<TRoot, TSubType>) this.JoinAlias(path, alias);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.JoinAlias(
      Expression<Func<TSubType, object>> path,
      Expression<Func<object>> alias,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, TSubType>) this.JoinAlias(path, alias, joinType);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.JoinAlias<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (IQueryOver<TRoot, TSubType>) this.JoinAlias<U>(path, alias, joinType, withClause);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.JoinAlias<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (IQueryOver<TRoot, TSubType>) this.JoinAlias<U>(path, alias, joinType, withClause);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.JoinAlias(
      Expression<Func<object>> path,
      Expression<Func<object>> alias,
      JoinType joinType)
    {
      return (IQueryOver<TRoot, TSubType>) this.JoinAlias(path, alias, joinType);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.JoinAlias<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (IQueryOver<TRoot, TSubType>) this.JoinAlias<U>(path, alias, joinType, withClause);
    }

    IQueryOver<TRoot, TSubType> IQueryOver<TRoot, TSubType>.JoinAlias<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (IQueryOver<TRoot, TSubType>) this.JoinAlias<U>(path, alias, joinType, withClause);
    }

    IQueryOverJoinBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Inner
    {
      get
      {
        return new IQueryOverJoinBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, JoinType.InnerJoin);
      }
    }

    IQueryOverJoinBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Left
    {
      get
      {
        return new IQueryOverJoinBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, JoinType.LeftOuterJoin);
      }
    }

    IQueryOverJoinBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Right
    {
      get
      {
        return new IQueryOverJoinBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, JoinType.RightOuterJoin);
      }
    }

    IQueryOverJoinBuilder<TRoot, TSubType> IQueryOver<TRoot, TSubType>.Full
    {
      get
      {
        return new IQueryOverJoinBuilder<TRoot, TSubType>((IQueryOver<TRoot, TSubType>) this, JoinType.FullJoin);
      }
    }
  }
}
