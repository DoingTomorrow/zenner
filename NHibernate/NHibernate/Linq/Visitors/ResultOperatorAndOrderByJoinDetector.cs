// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorAndOrderByJoinDetector
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.ReWriters;
using NHibernate.Param;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  internal class ResultOperatorAndOrderByJoinDetector : NhExpressionTreeVisitor
  {
    private readonly IIsEntityDecider _isEntityDecider;
    private readonly IJoiner _joiner;
    private int _memberExpressionDepth;

    public ResultOperatorAndOrderByJoinDetector(IIsEntityDecider isEntityDecider, IJoiner joiner)
    {
      this._isEntityDecider = isEntityDecider;
      this._joiner = joiner;
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      bool flag = this._isEntityDecider.IsIdentifier(expression.Expression.Type, expression.Member.Name);
      if (!flag)
        ++this._memberExpressionDepth;
      Expression expression1 = base.VisitMemberExpression(expression);
      if (!flag)
        --this._memberExpressionDepth;
      if (!this._isEntityDecider.IsEntity(expression.Type) || this._memberExpressionDepth <= 0 || !this._joiner.CanAddJoin((Expression) expression))
        return expression1;
      string key = ExpressionKeyVisitor.Visit((Expression) expression, (IDictionary<ConstantExpression, NamedParameter>) null);
      return this._joiner.AddJoin(expression1, key);
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
      return (Expression) expression;
    }

    public void Transform(ResultOperatorBase resultOperator)
    {
      resultOperator.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
    }

    public void Transform(Ordering ordering)
    {
      ordering.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
    }
  }
}
