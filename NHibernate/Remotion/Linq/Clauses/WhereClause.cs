// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.WhereClause
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
  public class WhereClause : IBodyClause, IClause
  {
    private Expression _predicate;

    public WhereClause(Expression predicate)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (predicate), predicate);
      this._predicate = predicate;
    }

    [DebuggerDisplay("{Remotion.Linq.Clauses.ExpressionTreeVisitors.FormattingExpressionTreeVisitor.Format (Predicate),nq}")]
    public Expression Predicate
    {
      get => this._predicate;
      set => this._predicate = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitWhereClause(this, queryModel, index);
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Predicate = transformation(this.Predicate);
    }

    public virtual WhereClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      return new WhereClause(this.Predicate);
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext)
    {
      return (IBodyClause) this.Clone(cloneContext);
    }

    public override string ToString()
    {
      return "where " + FormattingExpressionTreeVisitor.Format(this.Predicate);
    }
  }
}
