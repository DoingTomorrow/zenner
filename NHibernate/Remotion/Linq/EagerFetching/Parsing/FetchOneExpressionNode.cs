// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.Parsing.FetchOneExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.EagerFetching.Parsing
{
  public class FetchOneExpressionNode(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression relatedObjectSelector) : OuterFetchExpressionNodeBase(parseInfo, ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (relatedObjectSelector), relatedObjectSelector))
  {
    protected override FetchRequestBase CreateFetchRequest()
    {
      return (FetchRequestBase) new FetchOneRequest(this.RelationMember);
    }
  }
}
