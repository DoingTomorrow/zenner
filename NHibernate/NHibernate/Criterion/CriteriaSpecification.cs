// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.CriteriaSpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.Transform;

#nullable disable
namespace NHibernate.Criterion
{
  public static class CriteriaSpecification
  {
    public static readonly string RootAlias = "this";
    public static readonly IResultTransformer AliasToEntityMap = (IResultTransformer) new AliasToEntityMapResultTransformer();
    public static readonly IResultTransformer RootEntity = (IResultTransformer) new RootEntityResultTransformer();
    public static readonly IResultTransformer DistinctRootEntity = (IResultTransformer) new DistinctRootEntityResultTransformer();
    public static readonly IResultTransformer Projection = (IResultTransformer) new PassThroughResultTransformer();
    public static readonly JoinType InnerJoin = JoinType.InnerJoin;
    public static readonly JoinType FullJoin = JoinType.FullJoin;
    public static readonly JoinType LeftJoin = JoinType.LeftOuterJoin;
  }
}
