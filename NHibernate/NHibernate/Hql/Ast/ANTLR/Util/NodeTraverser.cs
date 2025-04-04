// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.NodeTraverser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public class NodeTraverser
  {
    private readonly IVisitationStrategy _visitor;

    public NodeTraverser(IVisitationStrategy visitor) => this._visitor = visitor;

    public void TraverseDepthFirst(IASTNode ast)
    {
      if (ast == null)
        throw new ArgumentNullException(nameof (ast));
      for (int index = 0; index < ast.ChildCount; ++index)
        this.VisitDepthFirst(ast.GetChild(index));
    }

    private void VisitDepthFirst(IASTNode ast)
    {
      if (ast == null)
        return;
      this._visitor.Visit(ast);
      for (int index = 0; index < ast.ChildCount; ++index)
        this.VisitDepthFirst(ast.GetChild(index));
    }
  }
}
