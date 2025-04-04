// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlTreeNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast
{
  public class HqlTreeNode
  {
    private readonly IASTNode _node;
    private readonly List<HqlTreeNode> _children;

    public IASTFactory Factory { get; private set; }

    protected HqlTreeNode(
      int type,
      string text,
      IASTFactory factory,
      IEnumerable<HqlTreeNode> children)
    {
      this.Factory = factory;
      this._node = factory.CreateNode(type, text);
      this._children = new List<HqlTreeNode>();
      this.AddChildren(children);
    }

    protected HqlTreeNode(
      int type,
      string text,
      IASTFactory factory,
      params HqlTreeNode[] children)
      : this(type, text, factory, (IEnumerable<HqlTreeNode>) children)
    {
    }

    private void AddChildren(IEnumerable<HqlTreeNode> children)
    {
      foreach (HqlTreeNode child in children)
      {
        if (child != null)
          this.AddChild(child);
      }
    }

    public IEnumerable<HqlTreeNode> NodesPreOrder
    {
      get
      {
        yield return this;
        foreach (HqlTreeNode child in this._children)
        {
          foreach (HqlTreeNode node in child.NodesPreOrder)
            yield return node;
        }
      }
    }

    public IEnumerable<HqlTreeNode> NodesPostOrder
    {
      get
      {
        foreach (HqlTreeNode child in this._children)
        {
          foreach (HqlTreeNode node in child.NodesPostOrder)
            yield return node;
        }
        yield return this;
      }
    }

    public IEnumerable<HqlTreeNode> Children => (IEnumerable<HqlTreeNode>) this._children;

    public void ClearChildren()
    {
      this._children.Clear();
      this._node.ClearChildren();
    }

    protected void SetText(string text) => this._node.Text = text;

    internal IASTNode AstNode => this._node;

    internal void AddChild(HqlTreeNode child)
    {
      switch (child)
      {
        case HqlExpressionSubTreeHolder _:
        case HqlDistinctHolder _:
          this.AddChildren(child.Children);
          break;
        default:
          this._children.Add(child);
          this._node.AddChild(child.AstNode);
          break;
      }
    }
  }
}
