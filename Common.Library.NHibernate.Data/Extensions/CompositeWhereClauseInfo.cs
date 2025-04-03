// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.CompositeWhereClauseInfo
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public class CompositeWhereClauseInfo : IWhereClauseInfo
  {
    public IWhereClauseInfo LeftHandSideClause { get; set; }

    public IWhereClauseInfo RightHandSideClause { get; set; }

    public string LogicalOperator { get; set; }

    public override string ToString()
    {
      return string.Format("({0} {1} {2})", (object) this.LeftHandSideClause.ToString(), (object) this.LogicalOperator, (object) this.RightHandSideClause.ToString());
    }
  }
}
