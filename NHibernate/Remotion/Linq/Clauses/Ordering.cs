// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Ordering
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Utilities;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses
{
  public class Ordering
  {
    private Expression _expression;

    public Ordering(Expression expression, OrderingDirection direction)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      this._expression = expression;
      this.OrderingDirection = direction;
    }

    [DebuggerDisplay("{Remotion.Linq.Clauses.ExpressionTreeVisitors.FormattingExpressionTreeVisitor.Format (Expression),nq}")]
    public Expression Expression
    {
      get => this._expression;
      set => this._expression = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public OrderingDirection OrderingDirection { get; set; }

    public virtual void Accept(
      IQueryModelVisitor visitor,
      QueryModel queryModel,
      OrderByClause orderByClause,
      int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<OrderByClause>(nameof (orderByClause), orderByClause);
      visitor.VisitOrdering(this, queryModel, orderByClause, index);
    }

    public virtual Ordering Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      return new Ordering(this.Expression, this.OrderingDirection);
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Expression = transformation(this.Expression);
    }

    public override string ToString()
    {
      return FormattingExpressionTreeVisitor.Format(this.Expression) + (this.OrderingDirection == OrderingDirection.Asc ? " asc" : " desc");
    }
  }
}
