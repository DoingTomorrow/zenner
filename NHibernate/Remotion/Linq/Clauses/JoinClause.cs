// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.JoinClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Utilities;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses
{
  public class JoinClause : IBodyClause, IClause, IQuerySource
  {
    private Type _itemType;
    private string _itemName;
    private Expression _innerSequence;
    private Expression _outerKeySelector;
    private Expression _innerKeySelector;

    public JoinClause(
      string itemName,
      Type itemType,
      Expression innerSequence,
      Expression outerKeySelector,
      Expression innerKeySelector)
    {
      ArgumentUtility.CheckNotNull<string>(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType);
      ArgumentUtility.CheckNotNull<Expression>(nameof (innerSequence), innerSequence);
      ArgumentUtility.CheckNotNull<Expression>(nameof (outerKeySelector), outerKeySelector);
      ArgumentUtility.CheckNotNull<Expression>(nameof (innerKeySelector), innerKeySelector);
      this._itemName = itemName;
      this._itemType = itemType;
      this._innerSequence = innerSequence;
      this._outerKeySelector = outerKeySelector;
      this._innerKeySelector = innerKeySelector;
    }

    public Type ItemType
    {
      get => this._itemType;
      set => this._itemType = ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNullOrEmpty(nameof (value), value);
    }

    [DebuggerDisplay("{Remotion.Linq.Clauses.ExpressionTreeVisitors.FormattingExpressionTreeVisitor.Format (InnerSequence),nq}")]
    public Expression InnerSequence
    {
      get => this._innerSequence;
      set => this._innerSequence = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    [DebuggerDisplay("{Remotion.Linq.Clauses.ExpressionTreeVisitors.FormattingExpressionTreeVisitor.Format (OuterKeySelector),nq}")]
    public Expression OuterKeySelector
    {
      get => this._outerKeySelector;
      set
      {
        this._outerKeySelector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
      }
    }

    [DebuggerDisplay("{Remotion.Linq.Clauses.ExpressionTreeVisitors.FormattingExpressionTreeVisitor.Format (InnerKeySelector),nq}")]
    public Expression InnerKeySelector
    {
      get => this._innerKeySelector;
      set
      {
        this._innerKeySelector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
      }
    }

    public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitJoinClause(this, queryModel, index);
    }

    public virtual void Accept(
      IQueryModelVisitor visitor,
      QueryModel queryModel,
      GroupJoinClause groupJoinClause)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<GroupJoinClause>(nameof (groupJoinClause), groupJoinClause);
      visitor.VisitJoinClause(this, queryModel, groupJoinClause);
    }

    public JoinClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      JoinClause joinClause = new JoinClause(this.ItemName, this.ItemType, this.InnerSequence, this.OuterKeySelector, this.InnerKeySelector);
      cloneContext.QuerySourceMapping.AddMapping((IQuerySource) this, (Expression) new QuerySourceReferenceExpression((IQuerySource) joinClause));
      return joinClause;
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext)
    {
      return (IBodyClause) this.Clone(cloneContext);
    }

    public virtual void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.InnerSequence = transformation(this.InnerSequence);
      this.OuterKeySelector = transformation(this.OuterKeySelector);
      this.InnerKeySelector = transformation(this.InnerKeySelector);
    }

    public override string ToString()
    {
      return string.Format("join {0} {1} in {2} on {3} equals {4}", (object) this.ItemType.Name, (object) this.ItemName, (object) FormattingExpressionTreeVisitor.Format(this.InnerSequence), (object) FormattingExpressionTreeVisitor.Format(this.OuterKeySelector), (object) FormattingExpressionTreeVisitor.Format(this.InnerKeySelector));
    }
  }
}
