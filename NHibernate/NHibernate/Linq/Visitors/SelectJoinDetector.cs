// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.SelectJoinDetector
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
  internal class SelectJoinDetector : NhExpressionTreeVisitor
  {
    private readonly IIsEntityDecider _isEntityDecider;
    private readonly IJoiner _joiner;
    private bool _hasIdentifier;
    private int _identifierMemberExpressionDepth;

    public SelectJoinDetector(IIsEntityDecider isEntityDecider, IJoiner joiner)
    {
      this._isEntityDecider = isEntityDecider;
      this._joiner = joiner;
    }

    public void Transform(SelectClause selectClause)
    {
      selectClause.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      if (this._isEntityDecider.IsIdentifier(expression.Expression.Type, expression.Member.Name))
        this._hasIdentifier = true;
      else if (this._hasIdentifier)
        ++this._identifierMemberExpressionDepth;
      Expression expression1 = base.VisitMemberExpression(expression);
      if (this._hasIdentifier)
        --this._identifierMemberExpressionDepth;
      if (this._isEntityDecider.IsEntity(expression.Type) && (!this._hasIdentifier || this._identifierMemberExpressionDepth > 0) && this._joiner.CanAddJoin((Expression) expression))
      {
        string key = ExpressionKeyVisitor.Visit((Expression) expression, (IDictionary<ConstantExpression, NamedParameter>) null);
        return this._joiner.AddJoin(expression1, key);
      }
      this._hasIdentifier = false;
      return expression1;
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
      return (Expression) expression;
    }
  }
}
