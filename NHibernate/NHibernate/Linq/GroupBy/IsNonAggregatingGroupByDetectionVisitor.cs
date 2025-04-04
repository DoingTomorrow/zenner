// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupBy.IsNonAggregatingGroupByDetectionVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Expressions;
using NHibernate.Linq.Visitors;
using Remotion.Linq.Clauses.Expressions;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupBy
{
  internal class IsNonAggregatingGroupByDetectionVisitor : NhExpressionTreeVisitor
  {
    private bool _containsNakedQuerySourceReferenceExpression;

    public bool IsNonAggregatingGroupBy(Expression expression)
    {
      this._containsNakedQuerySourceReferenceExpression = false;
      this.VisitExpression(expression);
      return this._containsNakedQuerySourceReferenceExpression;
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      return !(expression.Member.Name == "Key") ? base.VisitMemberExpression(expression) : (Expression) expression;
    }

    protected override Expression VisitNhAggregate(NhAggregatedExpression expression)
    {
      return (Expression) expression;
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      this._containsNakedQuerySourceReferenceExpression = true;
      return (Expression) expression;
    }
  }
}
