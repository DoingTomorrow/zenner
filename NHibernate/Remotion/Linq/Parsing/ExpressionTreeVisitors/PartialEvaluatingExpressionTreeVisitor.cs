// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.PartialEvaluatingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors.TreeEvaluation;
using Remotion.Linq.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors
{
  public class PartialEvaluatingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    private readonly PartialEvaluationInfo _partialEvaluationInfo;

    public static Expression EvaluateIndependentSubtrees(Expression expressionTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      PartialEvaluationInfo partialEvaluationInfo = EvaluatableTreeFindingExpressionTreeVisitor.Analyze(expressionTree);
      return new PartialEvaluatingExpressionTreeVisitor(expressionTree, partialEvaluationInfo).VisitExpression(expressionTree);
    }

    private PartialEvaluatingExpressionTreeVisitor(
      Expression treeRoot,
      PartialEvaluationInfo partialEvaluationInfo)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (treeRoot), treeRoot);
      ArgumentUtility.CheckNotNull<PartialEvaluationInfo>(nameof (partialEvaluationInfo), partialEvaluationInfo);
      this._partialEvaluationInfo = partialEvaluationInfo;
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      return expression;
    }

    public override Expression VisitExpression(Expression expression)
    {
      if (expression == null)
        return (Expression) null;
      if (expression.NodeType != ExpressionType.Lambda)
      {
        if (this._partialEvaluationInfo.IsEvaluatableExpression(expression))
        {
          Expression subtree;
          try
          {
            subtree = this.EvaluateSubtree(expression);
          }
          catch (Exception ex)
          {
            return (Expression) new PartialEvaluationExceptionExpression(ex, base.VisitExpression(expression));
          }
          return subtree != expression ? PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees(subtree) : subtree;
        }
      }
      return base.VisitExpression(expression);
    }

    protected Expression EvaluateSubtree(Expression subtree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (subtree), subtree);
      if (subtree.NodeType != ExpressionType.Constant)
        return (Expression) Expression.Constant(Expression.Lambda<Func<object>>((Expression) Expression.Convert(subtree, typeof (object))).Compile()(), subtree.Type);
      ConstantExpression constantExpression = (ConstantExpression) subtree;
      return constantExpression.Value is IQueryable queryable && queryable.Expression != constantExpression ? queryable.Expression : (Expression) constantExpression;
    }
  }
}
