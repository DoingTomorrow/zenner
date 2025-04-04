// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.NotExpression
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
  public class NotExpression : AbstractCriterion
  {
    private readonly ICriterion _criterion;

    public NotExpression(ICriterion criterion) => this._criterion = criterion;

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add("not (");
      sqlStringBuilder.Add(this._criterion.ToSqlString(criteria, criteriaQuery, enabledFilters));
      sqlStringBuilder.Add(")");
      return sqlStringBuilder.ToSqlString();
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this._criterion.GetTypedValues(criteria, criteriaQuery);
    }

    public override string ToString()
    {
      return string.Format("not ({0})", (object) this._criterion.ToString());
    }

    public override IProjection[] GetProjections() => this._criterion.GetProjections();
  }
}
