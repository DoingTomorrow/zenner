// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionTreeVisitors.ReverseResolvingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ExpressionTreeVisitors
{
  public class ReverseResolvingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    private readonly Expression _itemExpression;
    private readonly ParameterExpression _lambdaParameter;

    public static LambdaExpression ReverseResolve(
      Expression itemExpression,
      Expression resolvedExpression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (itemExpression), itemExpression);
      ArgumentUtility.CheckNotNull<Expression>(nameof (resolvedExpression), resolvedExpression);
      ParameterExpression lambdaParameter = Expression.Parameter(itemExpression.Type, "input");
      return Expression.Lambda(new ReverseResolvingExpressionTreeVisitor(itemExpression, lambdaParameter).VisitExpression(resolvedExpression), lambdaParameter);
    }

    public static LambdaExpression ReverseResolveLambda(
      Expression itemExpression,
      LambdaExpression resolvedExpression,
      int parameterInsertionPosition)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (itemExpression), itemExpression);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (resolvedExpression), resolvedExpression);
      if (parameterInsertionPosition < 0 || parameterInsertionPosition > resolvedExpression.Parameters.Count)
        throw new ArgumentOutOfRangeException(nameof (parameterInsertionPosition));
      ParameterExpression lambdaParameter = Expression.Parameter(itemExpression.Type, "input");
      Expression body = new ReverseResolvingExpressionTreeVisitor(itemExpression, lambdaParameter).VisitExpression(resolvedExpression.Body);
      List<ParameterExpression> parameterExpressionList = new List<ParameterExpression>((IEnumerable<ParameterExpression>) resolvedExpression.Parameters);
      parameterExpressionList.Insert(parameterInsertionPosition, lambdaParameter);
      return Expression.Lambda(body, parameterExpressionList.ToArray());
    }

    private ReverseResolvingExpressionTreeVisitor(
      Expression itemExpression,
      ParameterExpression lambdaParameter)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (itemExpression), itemExpression);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (lambdaParameter), lambdaParameter);
      this._itemExpression = itemExpression;
      this._lambdaParameter = lambdaParameter;
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      ArgumentUtility.CheckNotNull<QuerySourceReferenceExpression>(nameof (expression), expression);
      try
      {
        return AccessorFindingExpressionTreeVisitor.FindAccessorLambda((Expression) expression, this._itemExpression, this._lambdaParameter).Body;
      }
      catch (ArgumentException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot create a LambdaExpression that retrieves the value of '{0}' from items with a structure of '{1}'. The item expression does not contain the value or it is too complex.", (object) FormattingExpressionTreeVisitor.Format((Expression) expression), (object) FormattingExpressionTreeVisitor.Format(this._itemExpression)), (Exception) ex);
      }
    }
  }
}
