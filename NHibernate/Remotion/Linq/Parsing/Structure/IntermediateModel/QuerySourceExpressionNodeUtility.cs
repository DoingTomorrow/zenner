// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.QuerySourceExpressionNodeUtility
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class QuerySourceExpressionNodeUtility
  {
    public static Expression ReplaceParameterWithReference(
      IQuerySourceExpressionNode referencedNode,
      ParameterExpression parameterToReplace,
      Expression expression,
      ClauseGenerationContext context)
    {
      ArgumentUtility.CheckNotNull<IQuerySourceExpressionNode>(nameof (referencedNode), referencedNode);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (parameterToReplace), parameterToReplace);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ArgumentUtility.CheckNotNull<ClauseGenerationContext>(nameof (context), context);
      QuerySourceReferenceExpression replacementExpression = new QuerySourceReferenceExpression(QuerySourceExpressionNodeUtility.GetQuerySourceForNode(referencedNode, context));
      return ReplacingExpressionTreeVisitor.Replace((Expression) parameterToReplace, (Expression) replacementExpression, expression);
    }

    public static IQuerySource GetQuerySourceForNode(
      IQuerySourceExpressionNode node,
      ClauseGenerationContext context)
    {
      try
      {
        return (IQuerySource) context.GetContextInfo((IExpressionNode) node);
      }
      catch (KeyNotFoundException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot retrieve an IQuerySource for the given {0}. Be sure to call Apply before calling methods that require IQuerySources, and pass in the same QuerySourceClauseMapping to both.", (object) node.GetType().Name), (Exception) ex);
      }
    }
  }
}
