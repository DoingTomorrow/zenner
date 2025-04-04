// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverSubqueryPropertyBuilderBase`3
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverSubqueryPropertyBuilderBase<TReturn, TRoot, TSubType> : 
    QueryOverSubqueryPropertyBuilderBase
    where TReturn : IQueryOver<TRoot, TSubType>
  {
    protected TReturn root;
    protected string path;
    protected object value;

    protected QueryOverSubqueryPropertyBuilderBase()
    {
    }

    internal override QueryOverSubqueryPropertyBuilderBase Set(
      object root,
      string path,
      object value)
    {
      this.root = (TReturn) root;
      this.path = path;
      this.value = value;
      return (QueryOverSubqueryPropertyBuilderBase) this;
    }

    private void AddSubquery<U>(
      Func<string, DetachedCriteria, AbstractCriterion> propertyMethod,
      Func<object, DetachedCriteria, AbstractCriterion> valueMethod,
      QueryOver<U> detachedCriteria)
    {
      if (this.path != null)
        this.root.And((ICriterion) propertyMethod(this.path, detachedCriteria.DetachedCriteria));
      else
        this.root.And((ICriterion) valueMethod(this.value, detachedCriteria.DetachedCriteria));
    }

    public TReturn Eq<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyEq), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Eq), detachedCriteria);
      return this.root;
    }

    public TReturn EqAll<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyEqAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.EqAll), detachedCriteria);
      return this.root;
    }

    public TReturn Ge<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGe), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Ge), detachedCriteria);
      return this.root;
    }

    public TReturn GeAll<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGeAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.GeAll), detachedCriteria);
      return this.root;
    }

    public TReturn GeSome<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGeSome), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.GeSome), detachedCriteria);
      return this.root;
    }

    public TReturn Gt<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGt), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Gt), detachedCriteria);
      return this.root;
    }

    public TReturn GtAll<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGtAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.GtAll), detachedCriteria);
      return this.root;
    }

    public TReturn GtSome<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGtSome), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.GtSome), detachedCriteria);
      return this.root;
    }

    public TReturn In<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyIn), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.In), detachedCriteria);
      return this.root;
    }

    public TReturn Le<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLe), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Le), detachedCriteria);
      return this.root;
    }

    public TReturn LeAll<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLeAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.LeAll), detachedCriteria);
      return this.root;
    }

    public TReturn LeSome<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLeSome), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.LeSome), detachedCriteria);
      return this.root;
    }

    public TReturn Lt<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLt), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Lt), detachedCriteria);
      return this.root;
    }

    public TReturn LtAll<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLtAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.LtAll), detachedCriteria);
      return this.root;
    }

    public TReturn LtSome<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLtSome), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.LtSome), detachedCriteria);
      return this.root;
    }

    public TReturn Ne<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyNe), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Ne), detachedCriteria);
      return this.root;
    }

    public TReturn NotIn<U>(QueryOver<U> detachedCriteria)
    {
      this.AddSubquery<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyNotIn), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.NotIn), detachedCriteria);
      return this.root;
    }
  }
}
