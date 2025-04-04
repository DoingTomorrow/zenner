// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SimpleSubqueryExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using System;
using System.Linq;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class SimpleSubqueryExpression : SubqueryExpression
  {
    private object value;

    internal SimpleSubqueryExpression(
      object value,
      string op,
      string quantifier,
      DetachedCriteria dc)
      : base(op, quantifier, dc)
    {
      this.value = value;
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      TypedValue[] typedValues1 = base.GetTypedValues(criteria, criteriaQuery);
      TypedValue[] typedValues2 = new TypedValue[typedValues1.Length + 1];
      typedValues1.CopyTo((Array) typedValues2, 1);
      typedValues2[0] = this.FirstTypedValue();
      return typedValues2;
    }

    private TypedValue FirstTypedValue()
    {
      return new TypedValue(this.GetTypes()[0], this.value, EntityMode.Poco);
    }

    protected override SqlString ToLeftSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new SqlString(criteriaQuery.NewQueryParameter(this.FirstTypedValue()).First<Parameter>());
    }
  }
}
