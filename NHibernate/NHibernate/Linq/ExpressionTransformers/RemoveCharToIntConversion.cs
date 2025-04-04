// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ExpressionTransformers.RemoveCharToIntConversion
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.ExpressionTransformers
{
  public class RemoveCharToIntConversion : IExpressionTransformer<BinaryExpression>
  {
    public Expression Transform(BinaryExpression expression)
    {
      Expression left = expression.Left;
      Expression right = expression.Right;
      bool flag1 = this.IsConvertExpression(left);
      bool flag2 = this.IsConvertExpression(right);
      if (!flag1 && !flag2)
        return (Expression) expression;
      bool flag3 = this.IsConstantExpression(left);
      bool flag4 = this.IsConstantExpression(right);
      if (!flag3 && !flag4)
        return (Expression) expression;
      UnaryExpression unaryExpression = flag1 ? (UnaryExpression) left : (UnaryExpression) right;
      ConstantExpression constantExpression1 = flag3 ? (ConstantExpression) left : (ConstantExpression) right;
      if (unaryExpression.Type != typeof (int) || unaryExpression.Operand.Type != typeof (char) || constantExpression1.Type != typeof (int))
        return (Expression) expression;
      ConstantExpression constantExpression2 = Expression.Constant((object) Convert.ToChar((int) constantExpression1.Value));
      return flag4 ? (Expression) Expression.MakeBinary(expression.NodeType, unaryExpression.Operand, (Expression) constantExpression2) : (Expression) Expression.MakeBinary(expression.NodeType, (Expression) constantExpression2, unaryExpression.Operand);
    }

    private bool IsConvertExpression(Expression expression)
    {
      return expression.NodeType == ExpressionType.Convert;
    }

    private bool IsConstantExpression(Expression expression)
    {
      return expression.NodeType == ExpressionType.Constant;
    }

    public ExpressionType[] SupportedExpressionTypes
    {
      get
      {
        return new ExpressionType[6]
        {
          ExpressionType.Equal,
          ExpressionType.NotEqual,
          ExpressionType.GreaterThan,
          ExpressionType.GreaterThanOrEqual,
          ExpressionType.LessThan,
          ExpressionType.LessThanOrEqual
        };
      }
    }
  }
}
