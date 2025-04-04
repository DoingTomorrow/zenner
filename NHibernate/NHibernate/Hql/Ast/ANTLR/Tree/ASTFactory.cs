// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ASTFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class ASTFactory : IASTFactory
  {
    private readonly ITreeAdaptor _adaptor;

    public ASTFactory(ITreeAdaptor adaptor) => this._adaptor = adaptor;

    public IASTNode CreateNode(int type, string text, params IASTNode[] children)
    {
      IASTNode node = (IASTNode) this._adaptor.Create(type, text);
      node.AddChildren(children);
      return node;
    }
  }
}
