// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverJoinBuilderBase`3
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
  public class QueryOverJoinBuilderBase<TReturn, TRoot, TSubType> where TReturn : IQueryOver<TRoot, TSubType>
  {
    protected TReturn root;
    protected JoinType joinType;

    public QueryOverJoinBuilderBase(TReturn root, JoinType joinType)
    {
      this.root = root;
      this.joinType = joinType;
    }

    public TReturn JoinAlias(
      Expression<Func<TSubType, object>> path,
      Expression<Func<object>> alias)
    {
      return (TReturn) this.root.JoinAlias(path, alias, this.joinType);
    }

    public TReturn JoinAlias<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      Expression<Func<U, bool>> withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression<U>(withClause));
    }

    public TReturn JoinAlias<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      Expression<Func<bool>> withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression(withClause));
    }

    public TReturn JoinAlias<U>(
      Expression<Func<TSubType, U>> path,
      Expression<Func<U>> alias,
      ICriterion withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, withClause);
    }

    public TReturn JoinAlias<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      Expression<Func<U, bool>> withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression<U>(withClause));
    }

    public TReturn JoinAlias<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      Expression<Func<bool>> withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression(withClause));
    }

    public TReturn JoinAlias<U>(
      Expression<Func<TSubType, IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      ICriterion withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, withClause);
    }

    public TReturn JoinAlias(Expression<Func<object>> path, Expression<Func<object>> alias)
    {
      return (TReturn) this.root.JoinAlias(path, alias, this.joinType);
    }

    public TReturn JoinAlias<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      Expression<Func<U, bool>> withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression<U>(withClause));
    }

    public TReturn JoinAlias<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      Expression<Func<bool>> withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression(withClause));
    }

    public TReturn JoinAlias<U>(
      Expression<Func<U>> path,
      Expression<Func<U>> alias,
      ICriterion withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, withClause);
    }

    public TReturn JoinAlias<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      Expression<Func<U, bool>> withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression<U>(withClause));
    }

    public TReturn JoinAlias<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      Expression<Func<bool>> withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, ExpressionProcessor.ProcessExpression(withClause));
    }

    public TReturn JoinAlias<U>(
      Expression<Func<IEnumerable<U>>> path,
      Expression<Func<U>> alias,
      ICriterion withClause)
    {
      return (TReturn) this.root.JoinAlias<U>(path, alias, this.joinType, withClause);
    }
  }
}
