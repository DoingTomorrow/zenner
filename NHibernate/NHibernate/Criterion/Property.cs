// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Property
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class Property : PropertyProjection
  {
    internal Property(string propertyName)
      : base(propertyName)
    {
    }

    public AbstractCriterion Between(object min, object max)
    {
      return Restrictions.Between(this.PropertyName, min, max);
    }

    public AbstractCriterion In(ICollection values) => Restrictions.In(this.PropertyName, values);

    public AbstractCriterion In(object[] values) => Restrictions.In(this.PropertyName, values);

    public AbstractCriterion Like(object value)
    {
      return (AbstractCriterion) Restrictions.Like(this.PropertyName, value);
    }

    public AbstractCriterion Like(string value, MatchMode matchMode)
    {
      return (AbstractCriterion) Restrictions.Like(this.PropertyName, value, matchMode);
    }

    public AbstractCriterion Eq(object value)
    {
      return (AbstractCriterion) Restrictions.Eq(this.PropertyName, value);
    }

    public AbstractCriterion Gt(object value)
    {
      return (AbstractCriterion) Restrictions.Gt(this.PropertyName, value);
    }

    public AbstractCriterion Lt(object value)
    {
      return (AbstractCriterion) Restrictions.Lt(this.PropertyName, value);
    }

    public AbstractCriterion Le(object value)
    {
      return (AbstractCriterion) Restrictions.Le(this.PropertyName, value);
    }

    public AbstractCriterion Ge(object value)
    {
      return (AbstractCriterion) Restrictions.Ge(this.PropertyName, value);
    }

    public AbstractCriterion EqProperty(Property other)
    {
      return Restrictions.EqProperty(this.PropertyName, other.PropertyName);
    }

    public AbstractCriterion NotEqProperty(Property other)
    {
      return Restrictions.NotEqProperty(this.PropertyName, other.PropertyName);
    }

    public AbstractCriterion LeProperty(Property other)
    {
      return Restrictions.LeProperty(this.PropertyName, other.PropertyName);
    }

    public AbstractCriterion GeProperty(Property other)
    {
      return Restrictions.GeProperty(this.PropertyName, other.PropertyName);
    }

    public AbstractCriterion LtProperty(Property other)
    {
      return Restrictions.LtProperty(this.PropertyName, other.PropertyName);
    }

    public AbstractCriterion GtProperty(Property other)
    {
      return Restrictions.GtProperty(this.PropertyName, other.PropertyName);
    }

    public AbstractCriterion EqProperty(string other)
    {
      return Restrictions.EqProperty(this.PropertyName, other);
    }

    public AbstractCriterion NotEqProperty(string other)
    {
      return Restrictions.NotEqProperty(this.PropertyName, other);
    }

    public AbstractCriterion LeProperty(string other)
    {
      return Restrictions.LeProperty(this.PropertyName, other);
    }

    public AbstractCriterion GeProperty(string other)
    {
      return Restrictions.GeProperty(this.PropertyName, other);
    }

    public AbstractCriterion LtProperty(string other)
    {
      return Restrictions.LtProperty(this.PropertyName, other);
    }

    public AbstractCriterion GtProperty(string other)
    {
      return Restrictions.GtProperty(this.PropertyName, other);
    }

    public AbstractCriterion IsNull() => Restrictions.IsNull(this.PropertyName);

    public AbstractCriterion IsNotNull() => Restrictions.IsNotNull(this.PropertyName);

    public AbstractEmptinessExpression IsEmpty() => Restrictions.IsEmpty(this.PropertyName);

    public AbstractEmptinessExpression IsNotEmpty() => Restrictions.IsNotEmpty(this.PropertyName);

    public CountProjection Count() => Projections.Count(this.PropertyName);

    public AggregateProjection Max() => Projections.Max(this.PropertyName);

    public AggregateProjection Min() => Projections.Min(this.PropertyName);

    public AggregateProjection Avg() => Projections.Avg(this.PropertyName);

    public PropertyProjection Group() => Projections.GroupProperty(this.PropertyName);

    public Order Asc() => Order.Asc(this.PropertyName);

    public Order Desc() => Order.Desc(this.PropertyName);

    public static Property ForName(string propertyName) => new Property(propertyName);

    public Property GetProperty(string propertyName)
    {
      return Property.ForName(this.PropertyName + (object) '.' + propertyName);
    }

    public AbstractCriterion Eq(DetachedCriteria subselect)
    {
      return Subqueries.PropertyEq(this.PropertyName, subselect);
    }

    public AbstractCriterion Ne(DetachedCriteria subselect)
    {
      return Subqueries.PropertyNe(this.PropertyName, subselect);
    }

    public AbstractCriterion Lt(DetachedCriteria subselect)
    {
      return Subqueries.PropertyLt(this.PropertyName, subselect);
    }

    public AbstractCriterion Le(DetachedCriteria subselect)
    {
      return Subqueries.PropertyLe(this.PropertyName, subselect);
    }

    public AbstractCriterion Bt(DetachedCriteria subselect)
    {
      return Subqueries.PropertyGt(this.PropertyName, subselect);
    }

    public AbstractCriterion Ge(DetachedCriteria subselect)
    {
      return Subqueries.PropertyGe(this.PropertyName, subselect);
    }

    public AbstractCriterion NotIn(DetachedCriteria subselect)
    {
      return Subqueries.PropertyNotIn(this.PropertyName, subselect);
    }

    public AbstractCriterion In(DetachedCriteria subselect)
    {
      return Subqueries.PropertyIn(this.PropertyName, subselect);
    }

    public AbstractCriterion EqAll(DetachedCriteria subselect)
    {
      return Subqueries.PropertyEqAll(this.PropertyName, subselect);
    }

    public AbstractCriterion GtAll(DetachedCriteria subselect)
    {
      return Subqueries.PropertyGtAll(this.PropertyName, subselect);
    }

    public AbstractCriterion LtAll(DetachedCriteria subselect)
    {
      return Subqueries.PropertyLtAll(this.PropertyName, subselect);
    }

    public AbstractCriterion LeAll(DetachedCriteria subselect)
    {
      return Subqueries.PropertyLeAll(this.PropertyName, subselect);
    }

    public AbstractCriterion GeAll(DetachedCriteria subselect)
    {
      return Subqueries.PropertyGeAll(this.PropertyName, subselect);
    }

    public AbstractCriterion GtSome(DetachedCriteria subselect)
    {
      return Subqueries.PropertyGtSome(this.PropertyName, subselect);
    }

    public AbstractCriterion LtSome(DetachedCriteria subselect)
    {
      return Subqueries.PropertyLtSome(this.PropertyName, subselect);
    }

    public AbstractCriterion LeSome(DetachedCriteria subselect)
    {
      return Subqueries.PropertyLeSome(this.PropertyName, subselect);
    }

    public AbstractCriterion GeSome(DetachedCriteria subselect)
    {
      return Subqueries.PropertyGeSome(this.PropertyName, subselect);
    }
  }
}
