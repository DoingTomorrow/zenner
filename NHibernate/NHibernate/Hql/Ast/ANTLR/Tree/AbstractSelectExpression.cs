// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.AbstractSelectExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public abstract class AbstractSelectExpression(IToken token) : HqlSqlWalkerNode(token), ISelectExpression
  {
    private string _alias;

    public string Alias
    {
      get => this._alias;
      set => this._alias = value;
    }

    public bool IsConstructor => false;

    public virtual bool IsReturnableEntity => false;

    public virtual FromElement FromElement
    {
      get => (FromElement) null;
      set
      {
      }
    }

    public virtual bool IsScalar => this.DataType != null && !this.DataType.IsAssociationType;

    public abstract void SetScalarColumnText(int i);
  }
}
