// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverProjectionBuilder`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverProjectionBuilder<T>
  {
    private ProjectionList projectionList;
    private IProjection lastProjection;

    public QueryOverProjectionBuilder() => this.projectionList = Projections.ProjectionList();

    private void AddLastProjection()
    {
      if (this.lastProjection == null)
        return;
      this.projectionList.Add(this.lastProjection);
    }

    private void PushProjection(IProjection projection)
    {
      this.AddLastProjection();
      this.lastProjection = projection;
    }

    internal ProjectionList ProjectionList
    {
      get
      {
        this.AddLastProjection();
        return this.projectionList;
      }
    }

    public QueryOverProjectionBuilder<T> WithAlias(Expression<Func<object>> alias)
    {
      this.lastProjection = Projections.Alias(this.lastProjection, ExpressionProcessor.FindPropertyExpression(alias.Body));
      return this;
    }

    public QueryOverProjectionBuilder<T> Select(IProjection projection)
    {
      this.PushProjection(projection);
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectAvg(Expression<Func<T, object>> expression)
    {
      this.PushProjection((IProjection) Projections.Avg<T>(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectAvg(Expression<Func<object>> expression)
    {
      this.PushProjection((IProjection) Projections.Avg(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectCount(Expression<Func<T, object>> expression)
    {
      this.PushProjection((IProjection) Projections.Count<T>(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectCount(Expression<Func<object>> expression)
    {
      this.PushProjection((IProjection) Projections.Count(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectCountDistinct(Expression<Func<T, object>> expression)
    {
      this.PushProjection((IProjection) Projections.CountDistinct<T>(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectCountDistinct(Expression<Func<object>> expression)
    {
      this.PushProjection((IProjection) Projections.CountDistinct(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectGroup(Expression<Func<T, object>> expression)
    {
      this.PushProjection((IProjection) Projections.Group<T>(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectGroup(Expression<Func<object>> expression)
    {
      this.PushProjection((IProjection) Projections.Group(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectMax(Expression<Func<T, object>> expression)
    {
      this.PushProjection((IProjection) Projections.Max<T>(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectMax(Expression<Func<object>> expression)
    {
      this.PushProjection((IProjection) Projections.Max(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectMin(Expression<Func<T, object>> expression)
    {
      this.PushProjection((IProjection) Projections.Min<T>(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectMin(Expression<Func<object>> expression)
    {
      this.PushProjection((IProjection) Projections.Min(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> Select(Expression<Func<T, object>> expression)
    {
      this.PushProjection(ExpressionProcessor.FindMemberProjection(expression.Body).AsProjection());
      return this;
    }

    public QueryOverProjectionBuilder<T> Select(Expression<Func<object>> expression)
    {
      this.PushProjection(ExpressionProcessor.FindMemberProjection(expression.Body).AsProjection());
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectSubQuery<U>(QueryOver<U> detachedQueryOver)
    {
      this.PushProjection(Projections.SubQuery<U>(detachedQueryOver));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectSum(Expression<Func<T, object>> expression)
    {
      this.PushProjection((IProjection) Projections.Sum<T>(expression));
      return this;
    }

    public QueryOverProjectionBuilder<T> SelectSum(Expression<Func<object>> expression)
    {
      this.PushProjection((IProjection) Projections.Sum(expression));
      return this;
    }
  }
}
