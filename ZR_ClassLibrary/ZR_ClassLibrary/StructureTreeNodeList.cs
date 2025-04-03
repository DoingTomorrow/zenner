// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.StructureTreeNodeList
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Collections.Generic;

#nullable disable
namespace ZR_ClassLibrary
{
  public class StructureTreeNodeList : List<StructureTreeNode>
  {
    public StructureTreeNode Parent { get; set; }

    public StructureTreeNodeList()
    {
    }

    public StructureTreeNodeList(StructureTreeNode parent) => this.Parent = parent;

    public StructureTreeNode Add(StructureTreeNode node)
    {
      if (node == null)
        return (StructureTreeNode) null;
      base.Add(node);
      return node;
    }

    public void Remove() => this.Clear();

    public void Remove(StructureTreeNode node)
    {
      if (node != null)
        node.Parent = (StructureTreeNode) null;
      base.Remove(node);
    }

    public override string ToString() => string.Format("Count={0}", (object) this.Count);
  }
}
