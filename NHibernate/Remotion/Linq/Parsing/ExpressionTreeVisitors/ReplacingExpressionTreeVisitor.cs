// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.ReplacingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors
{
  public class ReplacingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    private readonly Expression _replacedExpression;
    private readonly Expression _replacementExpression;

    public static Expression Replace(
      Expression replacedExpression,
      Expression replacementExpression,
      Expression sourceTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (replacedExpression), replacedExpression);
      ArgumentUtility.CheckNotNull<Expression>(nameof (replacementExpression), replacementExpression);
      ArgumentUtility.CheckNotNull<Expression>(nameof (sourceTree), sourceTree);
      return new ReplacingExpressionTreeVisitor(replacedExpression, replacementExpression).VisitExpression(sourceTree);
    }

    private ReplacingExpressionTreeVisitor(
      Expression replacedExpression,
      Expression replacementExpression)
    {
      this._replacedExpression = replacedExpression;
      this._replacementExpression = replacementExpression;
    }

    public override Expression VisitExpression(Expression expression)
    {
      return object.Equals((object) expression, (object) this._replacedExpression) ? this._replacementExpression : base.VisitExpression(expression);
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
      return (Expression) expression;
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      return expression;
    }
  }
}
