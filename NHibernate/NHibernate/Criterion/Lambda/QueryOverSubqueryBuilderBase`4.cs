// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverSubqueryBuilderBase`4
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverSubqueryBuilderBase<TReturn, TRoot, TSubType, TBuilderType>
    where TReturn : IQueryOver<TRoot, TSubType>
    where TBuilderType : QueryOverSubqueryPropertyBuilderBase, new()
  {
    protected TReturn root;

    protected QueryOverSubqueryBuilderBase(TReturn root) => this.root = root;

    public TReturn WhereExists<U>(QueryOver<U> detachedQuery)
    {
      this.root.And((ICriterion) Subqueries.Exists(detachedQuery.DetachedCriteria));
      return this.root;
    }

    public TReturn WhereNotExists<U>(QueryOver<U> detachedQuery)
    {
      this.root.And((ICriterion) Subqueries.NotExists(detachedQuery.DetachedCriteria));
      return this.root;
    }

    public TReturn Where(Expression<Func<TSubType, bool>> expression)
    {
      this.root.And((ICriterion) ExpressionProcessor.ProcessSubquery<TSubType>(LambdaSubqueryType.Exact, expression));
      return this.root;
    }

    public TReturn Where(Expression<Func<bool>> expression)
    {
      this.root.And((ICriterion) ExpressionProcessor.ProcessSubquery(LambdaSubqueryType.Exact, expression));
      return this.root;
    }

    public TReturn WhereAll(Expression<Func<TSubType, bool>> expression)
    {
      this.root.And((ICriterion) ExpressionProcessor.ProcessSubquery<TSubType>(LambdaSubqueryType.All, expression));
      return this.root;
    }

    public TReturn WhereAll(Expression<Func<bool>> expression)
    {
      this.root.And((ICriterion) ExpressionProcessor.ProcessSubquery(LambdaSubqueryType.All, expression));
      return this.root;
    }

    public TReturn WhereSome(Expression<Func<TSubType, bool>> expression)
    {
      this.root.And((ICriterion) ExpressionProcessor.ProcessSubquery<TSubType>(LambdaSubqueryType.Some, expression));
      return this.root;
    }

    public TReturn WhereSome(Expression<Func<bool>> expression)
    {
      this.root.And((ICriterion) ExpressionProcessor.ProcessSubquery(LambdaSubqueryType.Some, expression));
      return this.root;
    }

    public TBuilderType WhereProperty(Expression<Func<TSubType, object>> expression)
    {
      return (TBuilderType) new TBuilderType().Set((object) this.root, ExpressionProcessor.FindMemberExpression(expression.Body), (object) null);
    }

    public TBuilderType WhereProperty(Expression<Func<object>> expression)
    {
      return (TBuilderType) new TBuilderType().Set((object) this.root, ExpressionProcessor.FindMemberExpression(expression.Body), (object) null);
    }

    public TBuilderType WhereValue(object value)
    {
      return (TBuilderType) new TBuilderType().Set((object) this.root, (string) null, value);
    }
  }
}
