// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.FromClauseBase
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
  public abstract class FromClauseBase : IClause, IQuerySource
  {
    private string _itemName;
    private Type _itemType;
    private Expression _fromExpression;

    protected FromClauseBase(string itemName, Type itemType, Expression fromExpression)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType);
      ArgumentUtility.CheckNotNull<Expression>(nameof (fromExpression), fromExpression);
      this._itemName = itemName;
      this._itemType = itemType;
      this._fromExpression = fromExpression;
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNullOrEmpty(nameof (value), value);
    }

    public Type ItemType
    {
      get => this._itemType;
      set => this._itemType = ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
    }

    [DebuggerDisplay("{Remotion.Linq.Clauses.ExpressionTreeVisitors.FormattingExpressionTreeVisitor.Format (FromExpression),nq}")]
    public Expression FromExpression
    {
      get => this._fromExpression;
      set => this._fromExpression = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public virtual void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.FromExpression = transformation(this.FromExpression);
    }

    public override string ToString()
    {
      return string.Format("from {0} {1} in {2}", (object) this.ItemType.Name, (object) this.ItemName, (object) FormattingExpressionTreeVisitor.Format(this.FromExpression));
    }
  }
}
