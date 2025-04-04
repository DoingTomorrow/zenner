// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.GroupJoinClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses
{
  public class GroupJoinClause : IQuerySource, IBodyClause, IClause
  {
    private string _itemName;
    private Type _itemType;
    private JoinClause _joinClause;

    public GroupJoinClause(string itemName, Type itemType, JoinClause joinClause)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType);
      ArgumentUtility.CheckNotNull<JoinClause>(nameof (joinClause), joinClause);
      this.ItemName = itemName;
      this.ItemType = itemType;
      this.JoinClause = joinClause;
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNull<string>(nameof (value), value);
    }

    public Type ItemType
    {
      get => this._itemType;
      set
      {
        ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
        ReflectionUtility.GetItemTypeOfIEnumerable(value, nameof (value));
        this._itemType = value;
      }
    }

    public JoinClause JoinClause
    {
      get => this._joinClause;
      set => this._joinClause = ArgumentUtility.CheckNotNull<JoinClause>(nameof (value), value);
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.JoinClause.TransformExpressions(transformation);
    }

    public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitGroupJoinClause(this, queryModel, index);
    }

    public GroupJoinClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      GroupJoinClause groupJoinClause = new GroupJoinClause(this.ItemName, this.ItemType, this.JoinClause.Clone(cloneContext));
      cloneContext.QuerySourceMapping.AddMapping((IQuerySource) this, (Expression) new QuerySourceReferenceExpression((IQuerySource) groupJoinClause));
      return groupJoinClause;
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext)
    {
      return (IBodyClause) this.Clone(cloneContext);
    }

    public override string ToString()
    {
      return string.Format("{0} into {1} {2}", (object) this.JoinClause, (object) this.ItemType.Name, (object) this.ItemName);
    }
  }
}
