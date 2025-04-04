// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverRestrictionBuilderBase`3
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverRestrictionBuilderBase<TReturn, TRoot, TSubType> where TReturn : IQueryOver<TRoot, TSubType>
  {
    private TReturn root;
    private ExpressionProcessor.ProjectionInfo projection;
    protected bool isNot;

    public QueryOverRestrictionBuilderBase(
      TReturn root,
      ExpressionProcessor.ProjectionInfo projection)
    {
      this.root = root;
      this.projection = projection;
    }

    private TReturn Add(ICriterion criterion)
    {
      if (this.isNot)
        criterion = (ICriterion) Restrictions.Not(criterion);
      return (TReturn) this.root.And(criterion);
    }

    public QueryOverRestrictionBuilderBase<TReturn, TRoot, TSubType>.LambdaBetweenBuilder IsBetween(
      object lo)
    {
      return new QueryOverRestrictionBuilderBase<TReturn, TRoot, TSubType>.LambdaBetweenBuilder(this.root, this.projection, this.isNot, lo);
    }

    public TReturn IsIn(ICollection values)
    {
      return this.Add(this.projection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.In(s, values)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.In(p, values))));
    }

    public TReturn IsIn(object[] values)
    {
      return this.Add(this.projection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.In(s, values)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.In(p, values))));
    }

    public TReturn IsInG<T>(IEnumerable<T> values)
    {
      return this.Add(this.projection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.InG<T>(s, values)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.InG<T>(p, values))));
    }

    public TReturn IsInsensitiveLike(object value)
    {
      return this.Add(this.projection.CreateCriterion(new Func<string, object, ICriterion>(Restrictions.InsensitiveLike), new Func<IProjection, object, ICriterion>(Restrictions.InsensitiveLike), value));
    }

    public TReturn IsInsensitiveLike(string value, MatchMode matchMode)
    {
      return this.Add(this.projection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.InsensitiveLike(s, value, matchMode)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.InsensitiveLike(p, value, matchMode))));
    }

    public TReturn IsEmpty
    {
      get => this.Add((ICriterion) Restrictions.IsEmpty(this.projection.AsProperty()));
    }

    public TReturn IsNotEmpty
    {
      get => this.Add((ICriterion) Restrictions.IsNotEmpty(this.projection.AsProperty()));
    }

    public TReturn IsNull
    {
      get
      {
        return this.Add(this.projection.CreateCriterion(new Func<string, ICriterion>(Restrictions.IsNull), new Func<IProjection, ICriterion>(Restrictions.IsNull)));
      }
    }

    public TReturn IsNotNull
    {
      get
      {
        return this.Add(this.projection.CreateCriterion(new Func<string, ICriterion>(Restrictions.IsNotNull), new Func<IProjection, ICriterion>(Restrictions.IsNotNull)));
      }
    }

    public TReturn IsLike(object value)
    {
      return this.Add(this.projection.CreateCriterion(new Func<string, object, ICriterion>(Restrictions.Like), new Func<IProjection, object, ICriterion>(Restrictions.Like), value));
    }

    public TReturn IsLike(string value, MatchMode matchMode)
    {
      return this.Add(this.projection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.Like(s, value, matchMode)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.Like(p, value, matchMode))));
    }

    public TReturn IsLike(string value, MatchMode matchMode, char? escapeChar)
    {
      return this.Add((ICriterion) Restrictions.Like(this.projection.AsProperty(), value, matchMode, escapeChar));
    }

    public class LambdaBetweenBuilder
    {
      private TReturn root;
      private ExpressionProcessor.ProjectionInfo projection;
      private bool isNot;
      private object lo;

      public LambdaBetweenBuilder(
        TReturn root,
        ExpressionProcessor.ProjectionInfo projection,
        bool isNot,
        object lo)
      {
        this.root = root;
        this.projection = projection;
        this.isNot = isNot;
        this.lo = lo;
      }

      private TReturn Add(ICriterion criterion)
      {
        if (this.isNot)
          criterion = (ICriterion) Restrictions.Not(criterion);
        return (TReturn) this.root.And(criterion);
      }

      public TReturn And(object hi)
      {
        return this.Add(this.projection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.Between(s, this.lo, hi)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.Between(p, this.lo, hi))));
      }
    }
  }
}
