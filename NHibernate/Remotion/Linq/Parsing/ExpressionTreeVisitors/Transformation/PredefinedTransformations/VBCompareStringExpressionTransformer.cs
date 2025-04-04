// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations.VBCompareStringExpressionTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations
{
  public class VBCompareStringExpressionTransformer : IExpressionTransformer<BinaryExpression>
  {
    private const string c_vbOperatorsClassName = "Microsoft.VisualBasic.CompilerServices.Operators";
    private const string c_vbCompareStringOperatorMethodName = "CompareString";

    public ExpressionType[] SupportedExpressionTypes
    {
      get
      {
        return new ExpressionType[6]
        {
          ExpressionType.Equal,
          ExpressionType.NotEqual,
          ExpressionType.LessThan,
          ExpressionType.GreaterThan,
          ExpressionType.LessThanOrEqual,
          ExpressionType.GreaterThanOrEqual
        };
      }
    }

    public Expression Transform(BinaryExpression expression)
    {
      ArgumentUtility.CheckNotNull<BinaryExpression>(nameof (expression), expression);
      if (!(expression.Left is MethodCallExpression left) || !this.IsVBOperator(left.Method, "CompareString"))
        return (Expression) expression;
      Expression right = expression.Right;
      ConstantExpression leftSideArgument2AsConstantExpression = left.Arguments[2] as ConstantExpression;
      return this.GetExpressionForNodeType(expression, left, leftSideArgument2AsConstantExpression);
    }

    private Expression GetExpressionForNodeType(
      BinaryExpression expression,
      MethodCallExpression leftSideAsMethodCallExpression,
      ConstantExpression leftSideArgument2AsConstantExpression)
    {
      switch (expression.NodeType)
      {
        case ExpressionType.Equal:
          return (Expression) new VBStringComparisonExpression((Expression) Expression.Equal(leftSideAsMethodCallExpression.Arguments[0], leftSideAsMethodCallExpression.Arguments[1]), (bool) leftSideArgument2AsConstantExpression.Value);
        case ExpressionType.NotEqual:
          return (Expression) new VBStringComparisonExpression((Expression) Expression.NotEqual(leftSideAsMethodCallExpression.Arguments[0], leftSideAsMethodCallExpression.Arguments[1]), (bool) leftSideArgument2AsConstantExpression.Value);
        default:
          VBStringComparisonExpression left = new VBStringComparisonExpression((Expression) Expression.Call(leftSideAsMethodCallExpression.Arguments[0], typeof (string).GetMethod("CompareTo", new Type[1]
          {
            typeof (string)
          }), leftSideAsMethodCallExpression.Arguments[1]), (bool) leftSideArgument2AsConstantExpression.Value);
          if (expression.NodeType == ExpressionType.GreaterThan)
            return (Expression) Expression.GreaterThan((Expression) left, (Expression) Expression.Constant((object) 0));
          if (expression.NodeType == ExpressionType.GreaterThanOrEqual)
            return (Expression) Expression.GreaterThanOrEqual((Expression) left, (Expression) Expression.Constant((object) 0));
          if (expression.NodeType == ExpressionType.LessThan)
            return (Expression) Expression.LessThan((Expression) left, (Expression) Expression.Constant((object) 0));
          if (expression.NodeType == ExpressionType.LessThanOrEqual)
            return (Expression) Expression.LessThanOrEqual((Expression) left, (Expression) Expression.Constant((object) 0));
          throw new NotSupportedException(string.Format("Binary expression with node type '{0}' is not supported in a VB string comparison.", (object) expression.NodeType));
      }
    }

    private bool IsVBOperator(MethodInfo operatorMethod, string operatorName)
    {
      return operatorMethod.DeclaringType.FullName == "Microsoft.VisualBasic.CompilerServices.Operators" && operatorMethod.Name == operatorName;
    }
  }
}
