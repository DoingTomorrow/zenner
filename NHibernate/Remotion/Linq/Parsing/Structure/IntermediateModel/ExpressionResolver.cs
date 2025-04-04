// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ExpressionResolver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class ExpressionResolver
  {
    public ExpressionResolver(IExpressionNode currentNode)
    {
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (currentNode), currentNode);
      this.CurrentNode = currentNode;
    }

    public IExpressionNode CurrentNode { get; set; }

    public Expression GetResolvedExpression(
      Expression unresolvedExpression,
      ParameterExpression parameterToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (unresolvedExpression), unresolvedExpression);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (parameterToBeResolved), parameterToBeResolved);
      return TransparentIdentifierRemovingExpressionTreeVisitor.ReplaceTransparentIdentifiers(this.CurrentNode.Source.Resolve(parameterToBeResolved, unresolvedExpression, clauseGenerationContext));
    }
  }
}
