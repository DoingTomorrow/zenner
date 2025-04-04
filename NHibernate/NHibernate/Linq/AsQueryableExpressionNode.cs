// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.AsQueryableExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public class AsQueryableExpressionNode(MethodCallExpressionParseInfo parseInfo) : 
    MethodCallExpressionNodeBase(parseInfo)
  {
    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      return this.Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      return queryModel;
    }
  }
}
