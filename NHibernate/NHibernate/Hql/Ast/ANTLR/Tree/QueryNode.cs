// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.QueryNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class QueryNode(IToken token) : AbstractRestrictableStatement(token), ISelectExpression
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (QueryNode));
    private OrderByClause _orderByClause;

    protected override IInternalLogger GetLog() => QueryNode.Log;

    protected override int GetWhereClauseParentTokenType() => 22;

    public override bool NeedsExecutor => false;

    public override int StatementType => 86;

    public override IType DataType
    {
      get => ((ISelectExpression) this.GetSelectClause().GetFirstSelectExpression()).DataType;
      set => base.DataType = value;
    }

    public void SetScalarColumnText(int i)
    {
      ColumnHelper.GenerateSingleScalarColumn(this.ASTFactory, (IASTNode) this, i);
    }

    public FromElement FromElement => (FromElement) null;

    public bool IsConstructor => false;

    public bool IsReturnableEntity => false;

    public bool IsScalar => true;

    public string Alias { get; set; }

    public OrderByClause GetOrderByClause()
    {
      if (this._orderByClause == null)
      {
        this._orderByClause = this.LocateOrderByClause();
        if (this._orderByClause == null)
        {
          QueryNode.Log.Debug((object) "getOrderByClause() : Creating a new ORDER BY clause");
          this._orderByClause = (OrderByClause) this.Walker.ASTFactory.CreateNode(41, "ORDER");
          (ASTUtil.FindTypeInChildren((IASTNode) this, 55) ?? ASTUtil.FindTypeInChildren((IASTNode) this, 22)).AddSibling((IASTNode) this._orderByClause);
        }
      }
      return this._orderByClause;
    }

    public SelectClause GetSelectClause()
    {
      return (SelectClause) ASTUtil.FindTypeInChildren((IASTNode) this, 138);
    }

    private OrderByClause LocateOrderByClause()
    {
      return (OrderByClause) ASTUtil.FindTypeInChildren((IASTNode) this, 41);
    }
  }
}
