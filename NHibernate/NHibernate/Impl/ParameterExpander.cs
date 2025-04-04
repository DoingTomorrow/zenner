// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.ParameterExpander
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  internal class ParameterExpander
  {
    private readonly Dictionary<string, List<string>> _map;
    private readonly IASTNode _tree;

    private ParameterExpander(IASTNode tree, Dictionary<string, List<string>> map)
    {
      this._tree = tree;
      this._map = map;
    }

    public static IASTNode Expand(IASTNode tree, Dictionary<string, List<string>> map)
    {
      return new ParameterExpander(tree, map).Expand();
    }

    private IASTNode Expand()
    {
      IList<IASTNode> astNodeList1 = ParameterDetector.LocateParameters(this._tree, new HashSet<string>((IEnumerable<string>) this._map.Keys));
      Dictionary<IASTNode, IEnumerable<IASTNode>> nodeMapping = new Dictionary<IASTNode, IEnumerable<IASTNode>>();
      foreach (IASTNode key in (IEnumerable<IASTNode>) astNodeList1)
      {
        IASTNode child = key.GetChild(0);
        List<string> stringList = this._map[child.Text];
        List<IASTNode> astNodeList2 = new List<IASTNode>();
        foreach (string str in stringList)
        {
          IASTNode astNode = key.DupNode();
          IASTNode childNode = child.DupNode();
          childNode.Text = str;
          astNode.AddChild(childNode);
          astNodeList2.Add(astNode);
        }
        nodeMapping.Add(key, (IEnumerable<IASTNode>) astNodeList2);
      }
      return ParameterExpander.DuplicateTree(this._tree, (IDictionary<IASTNode, IEnumerable<IASTNode>>) nodeMapping);
    }

    private static IASTNode DuplicateTree(
      IASTNode ast,
      IDictionary<IASTNode, IEnumerable<IASTNode>> nodeMapping)
    {
      IASTNode astNode1 = ast.DupNode();
      foreach (IASTNode astNode2 in (IEnumerable<IASTNode>) ast)
      {
        IEnumerable<IASTNode> astNodes;
        if (nodeMapping.TryGetValue(astNode2, out astNodes))
        {
          foreach (IASTNode childNode in astNodes)
            astNode1.AddChild(childNode);
        }
        else
          astNode1.AddChild(ParameterExpander.DuplicateTree(astNode2, nodeMapping));
      }
      return astNode1;
    }
  }
}
