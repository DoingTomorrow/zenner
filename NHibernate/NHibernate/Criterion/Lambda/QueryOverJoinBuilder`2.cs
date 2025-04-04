// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverJoinBuilder`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverJoinBuilder<TRoot, TSubType>(
    QueryOver<TRoot, TSubType> root,
    JoinType joinType) : QueryOverJoinBuilderBase<QueryOver<TRoot, TSubType>, TRoot, TSubType>(root, joinType)
  {
    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, U>> path)
    {
      return this.root.JoinQueryOver<U>(path, this.joinType);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path)
    {
      return this.root.JoinQueryOver<U>(path, this.joinType);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      Expression<Func<U, bool>> withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression<U>(withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      Expression<Func<bool>> withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression(withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      ICriterion withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, withClause);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<U>> path, Expression<Func<U>> alias)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      Expression<Func<U, bool>> withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression<U>(withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      Expression<Func<bool>> withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression(withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      ICriterion withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, withClause);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<TSubType, IEnumerable<U>>> path)
    {
      return this.root.JoinQueryOver<U>(path, this.joinType);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(Expression<Func<IEnumerable<U>>> path)
    {
      return this.root.JoinQueryOver<U>(path, this.joinType);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      Expression<Func<U, bool>> withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression<U>(withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      Expression<Func<bool>> withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression(withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      ICriterion withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, withClause);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType);
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      Expression<Func<U, bool>> withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression<U>(withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      Expression<Func<bool>> withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression(withClause));
    }

    public QueryOver<TRoot, U> JoinQueryOver<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      ICriterion withClause)
    {
      return this.root.JoinQueryOver<U>(path, alias, this.joinType, withClause);
    }
  }
}
