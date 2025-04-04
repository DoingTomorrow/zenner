// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.QuerySourceDetector
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Ast.ANTLR.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  internal class QuerySourceDetector : IVisitationStrategy
  {
    private readonly IASTNode _tree;
    private readonly List<IASTNode> _nodes;

    public QuerySourceDetector(IASTNode tree)
    {
      this._tree = tree;
      this._nodes = new List<IASTNode>();
    }

    public IList<IASTNode> LocateQuerySources()
    {
      new NodeTraverser((IVisitationStrategy) this).TraverseDepthFirst(this._tree);
      return (IList<IASTNode>) this._nodes;
    }

    public void Visit(IASTNode node)
    {
      if (node.Type != 22)
        return;
      this._nodes.AddRange(node.Where<IASTNode>((Func<IASTNode, bool>) (child => child.Type == 87)).Select<IASTNode, IASTNode>((Func<IASTNode, IASTNode>) (range => range.GetChild(0))));
    }
  }
}
