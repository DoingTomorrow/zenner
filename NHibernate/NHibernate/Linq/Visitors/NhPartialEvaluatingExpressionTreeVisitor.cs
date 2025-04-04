// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.NhPartialEvaluatingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  internal class NhPartialEvaluatingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    protected override Expression VisitConstantExpression(ConstantExpression expression)
    {
      return !(expression.Value is Expression expression1) ? base.VisitConstantExpression(expression) : NhPartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees(expression1);
    }

    public static Expression EvaluateIndependentSubtrees(Expression expression)
    {
      return new NhPartialEvaluatingExpressionTreeVisitor().VisitExpression(PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees(expression));
    }
  }
}
