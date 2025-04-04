// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SelectSubqueryExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using System;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class SelectSubqueryExpression : SubqueryExpression
  {
    internal SelectSubqueryExpression(DetachedCriteria dc)
      : base((string) null, (string) null, dc)
    {
    }

    protected override SqlString ToLeftSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return SqlString.Empty;
    }
  }
}
