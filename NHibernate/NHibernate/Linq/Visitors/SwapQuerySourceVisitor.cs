// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.SwapQuerySourceVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class SwapQuerySourceVisitor : NhExpressionTreeVisitor
  {
    private readonly IQuerySource _oldClause;
    private readonly IQuerySource _newClause;

    public SwapQuerySourceVisitor(IQuerySource oldClause, IQuerySource newClause)
    {
      this._oldClause = oldClause;
      this._newClause = newClause;
    }

    public Expression Swap(Expression expression) => this.VisitExpression(expression);

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      if (expression.ReferencedQuerySource == this._oldClause)
        return (Expression) new QuerySourceReferenceExpression(this._newClause);
      if (expression.ReferencedQuerySource is MainFromClause referencedQuerySource)
        referencedQuerySource.FromExpression = this.VisitExpression(referencedQuerySource.FromExpression);
      return (Expression) expression;
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
      return base.VisitSubQueryExpression(expression);
    }
  }
}
