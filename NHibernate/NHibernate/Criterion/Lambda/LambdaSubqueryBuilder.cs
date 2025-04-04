// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.LambdaSubqueryBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class LambdaSubqueryBuilder
  {
    private string propertyName;
    private object value;

    public LambdaSubqueryBuilder(string propertyName, object value)
    {
      this.propertyName = propertyName;
      this.value = value;
    }

    private AbstractCriterion CreatePropertyCriterion<U>(
      Func<string, DetachedCriteria, AbstractCriterion> propertyFactoryMethod,
      Func<object, DetachedCriteria, AbstractCriterion> valueFactoryMethod,
      QueryOver<U> detachedCriteria)
    {
      return this.propertyName != null ? propertyFactoryMethod(this.propertyName, detachedCriteria.DetachedCriteria) : valueFactoryMethod(this.value, detachedCriteria.DetachedCriteria);
    }

    public AbstractCriterion Eq<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyEq), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Eq), detachedCriteria);
    }

    public AbstractCriterion EqAll<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyEqAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.EqAll), detachedCriteria);
    }

    public AbstractCriterion Ge<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGe), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Ge), detachedCriteria);
    }

    public AbstractCriterion GeAll<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGeAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.GeAll), detachedCriteria);
    }

    public AbstractCriterion GeSome<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGeSome), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.GeSome), detachedCriteria);
    }

    public AbstractCriterion Gt<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGt), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Gt), detachedCriteria);
    }

    public AbstractCriterion GtAll<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGtAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.GtAll), detachedCriteria);
    }

    public AbstractCriterion GtSome<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGtSome), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.GtSome), detachedCriteria);
    }

    public AbstractCriterion In<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyIn), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.In), detachedCriteria);
    }

    public AbstractCriterion Le<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLe), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Le), detachedCriteria);
    }

    public AbstractCriterion LeAll<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLeAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.LeAll), detachedCriteria);
    }

    public AbstractCriterion LeSome<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLeSome), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.LeSome), detachedCriteria);
    }

    public AbstractCriterion Lt<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLt), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Lt), detachedCriteria);
    }

    public AbstractCriterion LtAll<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLtAll), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.LtAll), detachedCriteria);
    }

    public AbstractCriterion LtSome<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLtSome), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.LtSome), detachedCriteria);
    }

    public AbstractCriterion Ne<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyNe), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.Ne), detachedCriteria);
    }

    public AbstractCriterion NotIn<U>(QueryOver<U> detachedCriteria)
    {
      return this.CreatePropertyCriterion<U>(new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyNotIn), new Func<object, DetachedCriteria, AbstractCriterion>(Subqueries.NotIn), detachedCriteria);
    }
  }
}
