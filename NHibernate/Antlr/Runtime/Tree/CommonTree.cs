// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.CommonTree
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  internal class CommonTree : BaseTree
  {
    public int startIndex = -1;
    public int stopIndex = -1;
    protected IToken token;
    public CommonTree parent;
    public int childIndex = -1;

    public CommonTree()
    {
    }

    public CommonTree(CommonTree node)
      : base((ITree) node)
    {
      this.token = node.token;
      this.startIndex = node.startIndex;
      this.stopIndex = node.stopIndex;
    }

    public CommonTree(IToken t) => this.token = t;

    public virtual IToken Token => this.token;

    public override bool IsNil => this.token == null;

    public override int Type => this.token == null ? 0 : this.token.Type;

    public override string Text => this.token == null ? (string) null : this.token.Text;

    public override int Line
    {
      get
      {
        if (this.token != null && this.token.Line != 0)
          return this.token.Line;
        return this.ChildCount > 0 ? this.GetChild(0).Line : 0;
      }
    }

    public override int CharPositionInLine
    {
      get
      {
        if (this.token != null && this.token.CharPositionInLine != -1)
          return this.token.CharPositionInLine;
        return this.ChildCount > 0 ? this.GetChild(0).CharPositionInLine : 0;
      }
    }

    public override int TokenStartIndex
    {
      get => this.startIndex == -1 && this.token != null ? this.token.TokenIndex : this.startIndex;
      set => this.startIndex = value;
    }

    public override int TokenStopIndex
    {
      get => this.stopIndex == -1 && this.token != null ? this.token.TokenIndex : this.stopIndex;
      set => this.stopIndex = value;
    }

    public void SetUnknownTokenBoundaries()
    {
      if (this.children == null)
      {
        if (this.startIndex >= 0 && this.stopIndex >= 0)
          return;
        this.startIndex = this.stopIndex = this.token.TokenIndex;
      }
      else
      {
        for (int index = 0; index < this.children.Count; ++index)
          ((CommonTree) this.children[index]).SetUnknownTokenBoundaries();
        if (this.startIndex >= 0 && this.stopIndex >= 0 || this.children.Count <= 0)
          return;
        CommonTree child1 = (CommonTree) this.children[0];
        CommonTree child2 = (CommonTree) this.children[this.children.Count - 1];
        this.startIndex = child1.TokenStartIndex;
        this.stopIndex = child2.TokenStopIndex;
      }
    }

    public override int ChildIndex
    {
      get => this.childIndex;
      set => this.childIndex = value;
    }

    public override ITree Parent
    {
      get => (ITree) this.parent;
      set => this.parent = (CommonTree) value;
    }

    public override ITree DupNode() => (ITree) new CommonTree(this);

    public override string ToString()
    {
      if (this.IsNil)
        return "nil";
      if (this.Type == 0)
        return "<errornode>";
      return this.token == null ? (string) null : this.token.Text;
    }
  }
}
