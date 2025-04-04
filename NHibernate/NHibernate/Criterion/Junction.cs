// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Junction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public abstract class Junction : AbstractCriterion
  {
    private readonly IList<ICriterion> criteria = (IList<ICriterion>) new List<ICriterion>();

    public Junction Add(ICriterion criterion)
    {
      this.criteria.Add(criterion);
      return this;
    }

    public Junction Add<T>(Expression<Func<T, bool>> expression)
    {
      this.criteria.Add(ExpressionProcessor.ProcessExpression<T>(expression));
      return this;
    }

    public Junction Add(Expression<Func<bool>> expression)
    {
      this.criteria.Add(ExpressionProcessor.ProcessExpression(expression));
      return this;
    }

    protected abstract string Op { get; }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      ArrayList to = new ArrayList();
      foreach (ICriterion criterion in (IEnumerable<ICriterion>) this.criteria)
      {
        TypedValue[] typedValues = criterion.GetTypedValues(criteria, criteriaQuery);
        ArrayHelper.AddAll((IList) to, (IList) typedValues);
      }
      return (TypedValue[]) to.ToArray(typeof (TypedValue));
    }

    protected abstract SqlString EmptyExpression { get; }

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      if (this.criteria.Count == 0)
        return this.EmptyExpression;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add("(");
      for (int index = 0; index < this.criteria.Count - 1; ++index)
      {
        sqlStringBuilder.Add(this.criteria[index].ToSqlString(criteria, criteriaQuery, enabledFilters));
        sqlStringBuilder.Add(this.Op);
      }
      sqlStringBuilder.Add(this.criteria[this.criteria.Count - 1].ToSqlString(criteria, criteriaQuery, enabledFilters));
      sqlStringBuilder.Add(")");
      return sqlStringBuilder.ToSqlString();
    }

    public override string ToString()
    {
      return '('.ToString() + StringHelper.Join(this.Op, (IEnumerable) this.criteria) + (object) ')';
    }

    public override IProjection[] GetProjections() => (IProjection[]) null;
  }
}
