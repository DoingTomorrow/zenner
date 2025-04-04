// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ResultOperators.NonAggregatingGroupBy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ResultOperators;

#nullable disable
namespace NHibernate.Linq.ResultOperators
{
  public class NonAggregatingGroupBy : ClientSideTransformOperator
  {
    public NonAggregatingGroupBy(GroupResultOperator groupBy) => this.GroupBy = groupBy;

    public GroupResultOperator GroupBy { get; private set; }
  }
}
