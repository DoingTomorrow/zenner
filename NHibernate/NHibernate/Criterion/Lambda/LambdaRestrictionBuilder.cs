// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.LambdaRestrictionBuilder
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
  public class LambdaRestrictionBuilder
  {
    private ExpressionProcessor.ProjectionInfo projection;
    private bool isNot;

    private AbstractCriterion Process(AbstractCriterion criterion)
    {
      return this.isNot ? Restrictions.Not((ICriterion) criterion) : criterion;
    }

    public LambdaRestrictionBuilder(ExpressionProcessor.ProjectionInfo projection)
    {
      this.projection = projection;
    }

    public LambdaRestrictionBuilder.LambdaBetweenBuilder IsBetween(object lo)
    {
      return new LambdaRestrictionBuilder.LambdaBetweenBuilder(this.projection, lo, this.isNot);
    }

    public LambdaRestrictionBuilder Not
    {
      get
      {
        this.isNot = !this.isNot;
        return this;
      }
    }

    public AbstractCriterion IsIn(ICollection values)
    {
      return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => Restrictions.In(s, values)), (Func<IProjection, AbstractCriterion>) (p => Restrictions.In(p, values))));
    }

    public AbstractCriterion IsIn(object[] values)
    {
      return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => Restrictions.In(s, values)), (Func<IProjection, AbstractCriterion>) (p => Restrictions.In(p, values))));
    }

    public AbstractCriterion IsInG<T>(IEnumerable<T> values)
    {
      return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => Restrictions.InG<T>(s, values)), (Func<IProjection, AbstractCriterion>) (p => Restrictions.InG<T>(p, values))));
    }

    public AbstractCriterion IsInsensitiveLike(object value)
    {
      return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => Restrictions.InsensitiveLike(s, value)), (Func<IProjection, AbstractCriterion>) (p => Restrictions.InsensitiveLike(p, value))));
    }

    public AbstractCriterion IsInsensitiveLike(string value, MatchMode matchMode)
    {
      return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => Restrictions.InsensitiveLike(s, value, matchMode)), (Func<IProjection, AbstractCriterion>) (p => Restrictions.InsensitiveLike(p, value, matchMode))));
    }

    public AbstractCriterion IsEmpty
    {
      get => this.Process((AbstractCriterion) Restrictions.IsEmpty(this.projection.AsProperty()));
    }

    public AbstractCriterion IsNotEmpty
    {
      get
      {
        return this.Process((AbstractCriterion) Restrictions.IsNotEmpty(this.projection.AsProperty()));
      }
    }

    public AbstractCriterion IsNull
    {
      get
      {
        return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => Restrictions.IsNull(s)), (Func<IProjection, AbstractCriterion>) (p => Restrictions.IsNull(p))));
      }
    }

    public AbstractCriterion IsNotNull
    {
      get
      {
        return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => Restrictions.IsNotNull(s)), (Func<IProjection, AbstractCriterion>) (p => Restrictions.IsNotNull(p))));
      }
    }

    public AbstractCriterion IsLike(object value)
    {
      return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => (AbstractCriterion) Restrictions.Like(s, value)), (Func<IProjection, AbstractCriterion>) (p => (AbstractCriterion) Restrictions.Like(p, value))));
    }

    public AbstractCriterion IsLike(string value, MatchMode matchMode)
    {
      return this.Process(this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => (AbstractCriterion) Restrictions.Like(s, value, matchMode)), (Func<IProjection, AbstractCriterion>) (p => (AbstractCriterion) Restrictions.Like(p, value, matchMode))));
    }

    public AbstractCriterion IsLike(string value, MatchMode matchMode, char? escapeChar)
    {
      return this.Process(Restrictions.Like(this.projection.AsProperty(), value, matchMode, escapeChar));
    }

    public class LambdaBetweenBuilder
    {
      private ExpressionProcessor.ProjectionInfo projection;
      private object lo;
      private bool isNot;

      public LambdaBetweenBuilder(
        ExpressionProcessor.ProjectionInfo projection,
        object lo,
        bool isNot)
      {
        this.projection = projection;
        this.lo = lo;
        this.isNot = isNot;
      }

      public AbstractCriterion And(object hi)
      {
        AbstractCriterion expression = this.projection.Create<AbstractCriterion>((Func<string, AbstractCriterion>) (s => Restrictions.Between(s, this.lo, hi)), (Func<IProjection, AbstractCriterion>) (p => Restrictions.Between(p, this.lo, hi)));
        return this.isNot ? Restrictions.Not((ICriterion) expression) : expression;
      }
    }
  }
}
