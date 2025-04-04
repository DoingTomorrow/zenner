// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.NhExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Expressions;
using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class NhExpressionTreeVisitor : ExpressionTreeVisitor
  {
    public override Expression VisitExpression(Expression expression)
    {
      if (expression == null)
        return (Expression) null;
      switch (expression.NodeType)
      {
        case (ExpressionType) 10000:
        case (ExpressionType) 10001:
        case (ExpressionType) 10002:
        case (ExpressionType) 10003:
        case (ExpressionType) 10004:
        case (ExpressionType) 10005:
          return this.VisitNhAggregate((NhAggregatedExpression) expression);
        case (ExpressionType) 10006:
          return this.VisitNhNew((NhNewExpression) expression);
        case (ExpressionType) 10007:
          return this.VisitNhStar((NhStarExpression) expression);
        default:
          return base.VisitExpression(expression);
      }
    }

    protected virtual Expression VisitNhStar(NhStarExpression expression)
    {
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 == expression.Expression ? (Expression) expression : (Expression) new NhStarExpression(expression1);
    }

    protected virtual Expression VisitNhNew(NhNewExpression expression)
    {
      ReadOnlyCollection<Expression> arguments = this.VisitAndConvert<Expression>(expression.Arguments, nameof (VisitNhNew));
      return arguments == expression.Arguments ? (Expression) expression : (Expression) new NhNewExpression((IList<string>) expression.Members, (IList<Expression>) arguments);
    }

    protected virtual Expression VisitNhAggregate(NhAggregatedExpression expression)
    {
      switch (expression.NodeType)
      {
        case (ExpressionType) 10000:
          return this.VisitNhAverage((NhAverageExpression) expression);
        case (ExpressionType) 10001:
          return this.VisitNhMin((NhMinExpression) expression);
        case (ExpressionType) 10002:
          return this.VisitNhMax((NhMaxExpression) expression);
        case (ExpressionType) 10003:
          return this.VisitNhSum((NhSumExpression) expression);
        case (ExpressionType) 10004:
          return this.VisitNhCount((NhCountExpression) expression);
        case (ExpressionType) 10005:
          return this.VisitNhDistinct((NhDistinctExpression) expression);
        default:
          throw new ArgumentException();
      }
    }

    protected virtual Expression VisitNhDistinct(NhDistinctExpression expression)
    {
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 == expression.Expression ? (Expression) expression : (Expression) new NhDistinctExpression(expression1);
    }

    protected virtual Expression VisitNhCount(NhCountExpression expression)
    {
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 == expression.Expression ? (Expression) expression : (Expression) expression.CreateNew(expression1);
    }

    protected virtual Expression VisitNhSum(NhSumExpression expression)
    {
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 == expression.Expression ? (Expression) expression : (Expression) new NhSumExpression(expression1);
    }

    protected virtual Expression VisitNhMax(NhMaxExpression expression)
    {
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 == expression.Expression ? (Expression) expression : (Expression) new NhMaxExpression(expression1);
    }

    protected virtual Expression VisitNhMin(NhMinExpression expression)
    {
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 == expression.Expression ? (Expression) expression : (Expression) new NhMinExpression(expression1);
    }

    protected virtual Expression VisitNhAverage(NhAverageExpression expression)
    {
      Expression expression1 = this.VisitExpression(expression.Expression);
      return expression1 == expression.Expression ? (Expression) expression : (Expression) new NhAverageExpression(expression1);
    }
  }
}
