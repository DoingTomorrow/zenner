// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.NaturalIdentifier
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion.Lambda;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class NaturalIdentifier : ICriterion
  {
    private readonly Junction conjunction = (Junction) new Conjunction();

    public SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      return this.conjunction.ToSqlString(criteria, criteriaQuery, enabledFilters);
    }

    public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.conjunction.GetTypedValues(criteria, criteriaQuery);
    }

    public IProjection[] GetProjections() => this.conjunction.GetProjections();

    public NaturalIdentifier Set(string property, object value)
    {
      this.conjunction.Add((ICriterion) Restrictions.Eq(property, value));
      return this;
    }

    public LambdaNaturalIdentifierBuilder Set<T>(Expression<Func<T, object>> expression)
    {
      return new LambdaNaturalIdentifierBuilder(this, ExpressionProcessor.FindMemberExpression(expression.Body));
    }

    public LambdaNaturalIdentifierBuilder Set(Expression<Func<object>> expression)
    {
      return new LambdaNaturalIdentifierBuilder(this, ExpressionProcessor.FindMemberExpression(expression.Body));
    }
  }
}
