// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.ParameterDetector
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Ast.ANTLR.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  internal class ParameterDetector : IVisitationStrategy
  {
    private readonly List<IASTNode> _nodes;
    private readonly HashSet<string> _parameterNames;
    private readonly IASTNode _tree;

    private ParameterDetector(IASTNode tree, HashSet<string> parameterNames)
    {
      this._tree = tree;
      this._parameterNames = parameterNames;
      this._nodes = new List<IASTNode>();
    }

    public void Visit(IASTNode node)
    {
      if (node.Type != 106 && node.Type != 105 || !this._parameterNames.Contains(node.GetChild(0).Text))
        return;
      this._nodes.Add(node);
    }

    public static IList<IASTNode> LocateParameters(IASTNode tree, HashSet<string> parameterNames)
    {
      return new ParameterDetector(tree, parameterNames).LocateParameters();
    }

    private IList<IASTNode> LocateParameters()
    {
      new NodeTraverser((IVisitationStrategy) this).TraverseDepthFirst(this._tree);
      return (IList<IASTNode>) this._nodes;
    }
  }
}
