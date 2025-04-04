// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.QuerySourceUsageLocator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  internal class QuerySourceUsageLocator : NhExpressionTreeVisitor
  {
    private readonly IQuerySource _querySource;
    private bool _references;
    private readonly List<IBodyClause> _clauses = new List<IBodyClause>();

    public QuerySourceUsageLocator(IQuerySource querySource) => this._querySource = querySource;

    public IList<IBodyClause> Clauses => (IList<IBodyClause>) this._clauses.AsReadOnly();

    public void Search(IBodyClause clause)
    {
      this._references = false;
      clause.TransformExpressions(new Func<Expression, Expression>(this.ExpressionSearcher));
      if (!this._references)
        return;
      this._clauses.Add(clause);
    }

    private Expression ExpressionSearcher(Expression arg)
    {
      this.VisitExpression(arg);
      return arg;
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      if (expression.ReferencedQuerySource == this._querySource)
        this._references = true;
      return (Expression) expression;
    }
  }
}
