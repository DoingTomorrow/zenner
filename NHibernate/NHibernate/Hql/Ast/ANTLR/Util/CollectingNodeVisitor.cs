// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.CollectingNodeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public class CollectingNodeVisitor : IVisitationStrategy
  {
    private readonly List<IASTNode> collectedNodes = new List<IASTNode>();
    private readonly FilterPredicate predicate;

    public CollectingNodeVisitor(FilterPredicate predicate) => this.predicate = predicate;

    public void Visit(IASTNode node)
    {
      if (this.predicate != null && !this.predicate(node))
        return;
      this.collectedNodes.Add(node);
    }

    public IList<IASTNode> Collect(IASTNode root)
    {
      new NodeTraverser((IVisitationStrategy) this).TraverseDepthFirst(root);
      return (IList<IASTNode>) this.collectedNodes;
    }
  }
}
