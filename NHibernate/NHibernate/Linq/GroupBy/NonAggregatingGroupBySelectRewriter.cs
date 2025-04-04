// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupBy.NonAggregatingGroupBySelectRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupBy
{
  internal class NonAggregatingGroupBySelectRewriter : NhExpressionTreeVisitor
  {
    private ParameterExpression _inputParameter;
    private IQuerySource _querySource;

    public LambdaExpression Visit(Expression clause, Type resultType, IQuerySource querySource)
    {
      this._inputParameter = Expression.Parameter(resultType, "inputParameter");
      this._querySource = querySource;
      return Expression.Lambda(this.VisitExpression(clause), this._inputParameter);
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      return expression.ReferencedQuerySource == this._querySource ? (Expression) this._inputParameter : (Expression) expression;
    }
  }
}
