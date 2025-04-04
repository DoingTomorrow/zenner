// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.HqlSqlWalkerTreeNodeStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class HqlSqlWalkerTreeNodeStream(object tree) : CommonTreeNodeStream(tree)
  {
    public void InsertChild(IASTNode parent, IASTNode child)
    {
      if (child.ChildCount > 0)
        throw new InvalidOperationException("Currently do not support adding nodes with children");
      int parentIndex = this.nodes.IndexOf((object) parent);
      int num = this.NumberOfChildNodes(parentIndex);
      int index1;
      if (num == 0)
      {
        int index2 = parentIndex + 1;
        this.nodes.Insert(index2, this.down);
        index1 = index2 + 1;
      }
      else
        index1 = parentIndex + num;
      parent.AddChild(child);
      this.nodes.Insert(index1, (object) child);
      int index3 = index1 + 1;
      if (num != 0)
        return;
      this.nodes.Insert(index3, this.up);
    }

    private int NumberOfChildNodes(int parentIndex)
    {
      if (this.nodes.Count - 1 == parentIndex || this.nodes[parentIndex + 1] != this.down)
        return 0;
      int num1 = 0;
      int num2 = 1;
      do
      {
        if (this.nodes[parentIndex + num2] == this.down)
          ++num1;
        else if (this.nodes[parentIndex + num2] == this.up)
          --num1;
        ++num2;
      }
      while (num1 > 0);
      return num2 - 1;
    }
  }
}
