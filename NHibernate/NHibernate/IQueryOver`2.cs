// Decompiled with JetBrains decompiler
// Type: NHibernate.IQueryOver`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate
{
  public interface IQueryOver<TRoot, TSubType> : IQueryOver<TRoot>, IQueryOver
  {
    IQueryOver<TRoot, TSubType> And(Expression<Func<TSubType, bool>> expression);

    IQueryOver<TRoot, TSubType> And(Expression<Func<bool>> expression);

    IQueryOver<TRoot, TSubType> And(ICriterion expression);

    IQueryOver<TRoot, TSubType> AndNot(Expression<Func<TSubType, bool>> expression);

    IQueryOver<TRoot, TSubType> AndNot(Expression<Func<bool>> expression);

    IQueryOverRestrictionBuilder<TRoot, TSubType> AndRestrictionOn(
      Expression<Func<TSubType, object>> expression);

    IQueryOverRestrictionBuilder<TRoot, TSubType> AndRestrictionOn(
      Expression<Func<object>> expression);

    IQueryOver<TRoot, TSubType> Where(Expression<Func<TSubType, bool>> expression);

    IQueryOver<TRoot, TSubType> Where(Expression<Func<bool>> expression);

    IQueryOver<TRoot, TSubType> Where(ICriterion expression);

    IQueryOver<TRoot, TSubType> WhereNot(Expression<Func<TSubType, bool>> expression);

    IQueryOver<TRoot, TSubType> WhereNot(Expression<Func<bool>> expression);

    IQueryOverRestrictionBuilder<TRoot, TSubType> WhereRestrictionOn(
      Expression<Func<TSubType, object>> expression);

    IQueryOverRestrictionBuilder<TRoot, TSubType> WhereRestrictionOn(
      Expression<Func<object>> expression);

    IQueryOver<TRoot, TSubType> Select(
      params Expression<Func<TRoot, object>>[] projections);

    IQueryOver<TRoot, TSubType> Select(params IProjection[] projections);

    IQueryOver<TRoot, TSubType> SelectList(
      Func<QueryOverProjectionBuilder<TRoot>, QueryOverProjectionBuilder<TRoot>> list);

    IQueryOverOrderBuilder<TRoot, TSubType> OrderBy(Expression<Func<TSubType, object>> path);

    IQueryOverOrderBuilder<TRoot, TSubType> OrderBy(Expression<Func<object>> path);

    IQueryOverOrderBuilder<TRoot, TSubType> OrderBy(IProjection projection);

    IQueryOverOrderBuilder<TRoot, TSubType> OrderByAlias(Expression<Func<object>> path);

    IQueryOverOrderBuilder<TRoot, TSubType> ThenBy(Expression<Func<TSubType, object>> path);

    IQueryOverOrderBuilder<TRoot, TSubType> ThenBy(Expression<Func<object>> path);

    IQueryOverOrderBuilder<TRoot, TSubType> ThenBy(IProjection projection);

    IQueryOverOrderBuilder<TRoot, TSubType> ThenByAlias(Expression<Func<object>> path);

    IQueryOver<TRoot, TSubType> TransformUsing(IResultTransformer resultTransformer);

    IQueryOverSubqueryBuilder<TRoot, TSubType> WithSubquery { get; }

    IQueryOverFetchBuilder<TRoot, TSubType> Fetch(Expression<Func<TRoot, object>> path);

    IQueryOverLockBuilder<TRoot, TSubType> Lock();

    IQueryOverLockBuilder<TRoot, TSubType> Lock(Expression<Func<object>> alias);

    IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path);

    IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias);

    IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, Expression<Func<U>> alias);

    IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path, JoinType joinType);

    IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, JoinType joinType);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause);

    IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, IEnumerable<U>>> path);

    IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      JoinType joinType);

    IQueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path, JoinType joinType);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType);

    IQueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause);

    IQueryOver<TRoot, TSubType> JoinAlias(
      Expression<Func<TSubType, object>> path,
      Expression<Func<object>> alias);

    IQueryOver<TRoot, TSubType> JoinAlias(
      Expression<Func<object>> path,
      Expression<Func<object>> alias);

    IQueryOver<TRoot, TSubType> JoinAlias(
      Expression<Func<TSubType, object>> path,
      Expression<Func<object>> alias,
      JoinType joinType);

    IQueryOver<TRoot, TSubType> JoinAlias<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause);

    IQueryOver<TRoot, TSubType> JoinAlias<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause);

    IQueryOver<TRoot, TSubType> JoinAlias(
      Expression<Func<object>> path,
      Expression<Func<object>> alias,
      JoinType joinType);

    IQueryOver<TRoot, TSubType> JoinAlias<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause);

    IQueryOver<TRoot, TSubType> JoinAlias<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      JoinType joinType,
      ICriterion withClause);

    IQueryOverJoinBuilder<TRoot, TSubType> Inner { get; }

    IQueryOverJoinBuilder<TRoot, TSubType> Left { get; }

    IQueryOverJoinBuilder<TRoot, TSubType> Right { get; }

    IQueryOverJoinBuilder<TRoot, TSubType> Full { get; }
  }
}
