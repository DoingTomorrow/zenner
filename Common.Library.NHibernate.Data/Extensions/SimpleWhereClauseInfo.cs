// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.SimpleWhereClauseInfo
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public class SimpleWhereClauseInfo : IWhereClauseInfo
  {
    public string PropertyName { get; set; }

    public HQLOperator Operator { get; set; }

    public object Value { get; set; }

    public override string ToString()
    {
      return string.Format("{0} {1} {2}", (object) this.PropertyName, (object) this.Operator, this.Value);
    }
  }
}
