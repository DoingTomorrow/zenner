// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.ICriterion
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Criterion
{
  public interface ICriterion
  {
    SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters);

    TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery);

    IProjection[] GetProjections();
  }
}
