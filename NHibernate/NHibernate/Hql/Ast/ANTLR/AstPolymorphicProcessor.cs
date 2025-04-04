// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.AstPolymorphicProcessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  public class AstPolymorphicProcessor
  {
    private readonly IASTNode _ast;
    private readonly ISessionFactoryImplementor _factory;
    private IEnumerable<KeyValuePair<IASTNode, IASTNode[]>> _nodeMapping;

    private AstPolymorphicProcessor(IASTNode ast, ISessionFactoryImplementor factory)
    {
      this._ast = ast;
      this._factory = factory;
    }

    public static IASTNode[] Process(IASTNode ast, ISessionFactoryImplementor factory)
    {
      return new AstPolymorphicProcessor(ast, factory).Process();
    }

    private IASTNode[] Process()
    {
      this._nodeMapping = (IEnumerable<KeyValuePair<IASTNode, IASTNode[]>>) new PolymorphicQuerySourceDetector(this._factory).Process(this._ast);
      if (this._nodeMapping.Count<KeyValuePair<IASTNode, IASTNode[]>>() > 0)
        return this.DuplicateTree().ToArray<IASTNode>();
      return new IASTNode[1]{ this._ast };
    }

    private IEnumerable<IASTNode> DuplicateTree()
    {
      IList<Dictionary<IASTNode, IASTNode>> source = CrossJoinDictionaryArrays.PerformCrossJoin(this._nodeMapping);
      IASTNode[] astNodeArray = new IASTNode[source.Count<Dictionary<IASTNode, IASTNode>>()];
      for (int index = 0; index < source.Count<Dictionary<IASTNode, IASTNode>>(); ++index)
        astNodeArray[index] = AstPolymorphicProcessor.DuplicateTree(this._ast, (IDictionary<IASTNode, IASTNode>) source[index]);
      return (IEnumerable<IASTNode>) astNodeArray;
    }

    private static IASTNode DuplicateTree(IASTNode ast, IDictionary<IASTNode, IASTNode> nodeMapping)
    {
      IASTNode astNode1;
      if (nodeMapping.TryGetValue(ast, out astNode1))
        return astNode1;
      IASTNode astNode2 = ast.DupNode();
      foreach (IASTNode ast1 in (IEnumerable<IASTNode>) ast)
        astNode2.AddChild(AstPolymorphicProcessor.DuplicateTree(ast1, nodeMapping));
      return astNode2;
    }
  }
}
