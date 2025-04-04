// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations.InvocationOfLambdaExpressionTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations
{
  public class InvocationOfLambdaExpressionTransformer : IExpressionTransformer<InvocationExpression>
  {
    public ExpressionType[] SupportedExpressionTypes
    {
      get => new ExpressionType[1]{ ExpressionType.Invoke };
    }

    public Expression Transform(InvocationExpression expression)
    {
      ArgumentUtility.CheckNotNull<InvocationExpression>(nameof (expression), expression);
      return this.StripTrivialConversions(expression.Expression) is LambdaExpression lambdaExpression ? this.InlineLambdaExpression(lambdaExpression, expression.Arguments) : (Expression) expression;
    }

    private Expression StripTrivialConversions(Expression invokedExpression)
    {
      while (invokedExpression.NodeType == ExpressionType.Convert && invokedExpression.Type == ((UnaryExpression) invokedExpression).Operand.Type && ((UnaryExpression) invokedExpression).Method == null)
        invokedExpression = ((UnaryExpression) invokedExpression).Operand;
      return invokedExpression;
    }

    private Expression InlineLambdaExpression(
      LambdaExpression lambdaExpression,
      ReadOnlyCollection<Expression> arguments)
    {
      Dictionary<Expression, Expression> expressionMapping = new Dictionary<Expression, Expression>(arguments.Count);
      Expression body = lambdaExpression.Body;
      for (int index = 0; index < lambdaExpression.Parameters.Count; ++index)
        expressionMapping.Add((Expression) lambdaExpression.Parameters[index], arguments[index]);
      return MultiReplacingExpressionTreeVisitor.Replace((IDictionary<Expression, Expression>) expressionMapping, body);
    }
  }
}
