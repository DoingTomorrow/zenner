// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.SelectClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses
{
  public class SelectClause : IClause
  {
    private Expression _selector;

    public SelectClause(Expression selector)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (selector), selector);
      this._selector = selector;
    }

    [DebuggerDisplay("{Remotion.Linq.Clauses.ExpressionTreeVisitors.FormattingExpressionTreeVisitor.Format (Selector),nq}")]
    public Expression Selector
    {
      get => this._selector;
      set => this._selector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitSelectClause(this, queryModel);
    }

    public SelectClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      return new SelectClause(this.Selector);
    }

    public virtual void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Selector = transformation(this.Selector);
    }

    public override string ToString()
    {
      return "select " + FormattingExpressionTreeVisitor.Format(this.Selector);
    }

    public StreamedSequenceInfo GetOutputDataInfo()
    {
      return new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this.Selector.Type), this.Selector);
    }
  }
}
