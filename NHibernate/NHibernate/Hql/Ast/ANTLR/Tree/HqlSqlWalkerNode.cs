// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.HqlSqlWalkerNode
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
  public class HqlSqlWalkerNode(IToken token) : SqlNode(token), IInitializableNode
  {
    private HqlSqlWalker _walker;

    public virtual void Initialize(object param) => this._walker = (HqlSqlWalker) param;

    public HqlSqlWalker Walker => this._walker;

    internal SessionFactoryHelperExtensions SessionFactoryHelper
    {
      get => this._walker.SessionFactoryHelper;
    }

    public IASTFactory ASTFactory => this._walker.ASTFactory;

    public AliasGenerator AliasGenerator => this._walker.AliasGenerator;
  }
}
