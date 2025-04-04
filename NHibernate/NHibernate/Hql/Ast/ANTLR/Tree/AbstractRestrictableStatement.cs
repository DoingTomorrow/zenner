// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.AbstractRestrictableStatement
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Hql.Ast.ANTLR.Util;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public abstract class AbstractRestrictableStatement(IToken token) : 
    AbstractStatement(token),
    IRestrictableStatement,
    IStatement
  {
    private FromClause _fromClause;
    private IASTNode _whereClause;

    protected abstract IInternalLogger GetLog();

    protected abstract int GetWhereClauseParentTokenType();

    public FromClause FromClause
    {
      get
      {
        if (this._fromClause == null)
          this._fromClause = (FromClause) ASTUtil.FindTypeInChildren((IASTNode) this, 22);
        return this._fromClause;
      }
    }

    public bool HasWhereClause
    {
      get
      {
        IASTNode typeInChildren = ASTUtil.FindTypeInChildren((IASTNode) this, 55);
        return typeInChildren != null && typeInChildren.ChildCount > 0;
      }
    }

    public IASTNode WhereClause
    {
      get
      {
        if (this._whereClause == null)
        {
          this._whereClause = ASTUtil.FindTypeInChildren((IASTNode) this, 55);
          if (this._whereClause == null)
          {
            this.GetLog().Debug((object) "getWhereClause() : Creating a new WHERE clause...");
            this._whereClause = this.Walker.ASTFactory.CreateNode(55, "WHERE");
            ASTUtil.FindTypeInChildren((IASTNode) this, this.GetWhereClauseParentTokenType()).AddSibling(this._whereClause);
          }
        }
        return this._whereClause;
      }
    }
  }
}
