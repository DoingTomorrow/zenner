// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.AbstractCriterion
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public abstract class AbstractCriterion : ICriterion
  {
    public abstract override string ToString();

    public abstract SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters);

    public abstract TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery);

    public abstract IProjection[] GetProjections();

    public static AbstractCriterion operator &(AbstractCriterion lhs, AbstractCriterion rhs)
    {
      return (AbstractCriterion) new AndExpression((ICriterion) lhs, (ICriterion) rhs);
    }

    public static AbstractCriterion operator |(AbstractCriterion lhs, AbstractCriterion rhs)
    {
      return (AbstractCriterion) new OrExpression((ICriterion) lhs, (ICriterion) rhs);
    }

    public static AbstractCriterion operator &(
      AbstractCriterion lhs,
      AbstractEmptinessExpression rhs)
    {
      return (AbstractCriterion) new AndExpression((ICriterion) lhs, (ICriterion) rhs);
    }

    public static AbstractCriterion operator |(
      AbstractCriterion lhs,
      AbstractEmptinessExpression rhs)
    {
      return (AbstractCriterion) new OrExpression((ICriterion) lhs, (ICriterion) rhs);
    }

    public static AbstractCriterion operator !(AbstractCriterion crit)
    {
      return (AbstractCriterion) new NotExpression((ICriterion) crit);
    }

    public static bool operator false(AbstractCriterion criteria) => false;

    public static bool operator true(AbstractCriterion criteria) => false;
  }
}
