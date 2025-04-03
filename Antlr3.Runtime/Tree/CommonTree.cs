// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.CommonTree
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class CommonTree : BaseTree
  {
    private IToken _token;
    protected int startIndex = -1;
    protected int stopIndex = -1;
    private CommonTree parent;
    private int childIndex = -1;

    public CommonTree()
    {
    }

    public CommonTree(CommonTree node)
      : base((ITree) node)
    {
      this.Token = node != null ? node.Token : throw new ArgumentNullException(nameof (node));
      this.startIndex = node.startIndex;
      this.stopIndex = node.stopIndex;
    }

    public CommonTree(IToken t) => this.Token = t;

    public override int CharPositionInLine
    {
      get
      {
        if (this.Token != null && this.Token.CharPositionInLine != -1)
          return this.Token.CharPositionInLine;
        return this.ChildCount > 0 ? this.Children[0].CharPositionInLine : 0;
      }
      set => base.CharPositionInLine = value;
    }

    public override int ChildIndex
    {
      get => this.childIndex;
      set => this.childIndex = value;
    }

    public override bool IsNil => this.Token == null;

    public override int Line
    {
      get
      {
        if (this.Token != null && this.Token.Line != 0)
          return this.Token.Line;
        return this.ChildCount > 0 ? this.Children[0].Line : 0;
      }
      set => base.Line = value;
    }

    public override ITree Parent
    {
      get => (ITree) this.parent;
      set => this.parent = (CommonTree) value;
    }

    public override string Text
    {
      get => this.Token == null ? (string) null : this.Token.Text;
      set
      {
      }
    }

    public IToken Token
    {
      get => this._token;
      set => this._token = value;
    }

    public override int TokenStartIndex
    {
      get => this.startIndex == -1 && this.Token != null ? this.Token.TokenIndex : this.startIndex;
      set => this.startIndex = value;
    }

    public override int TokenStopIndex
    {
      get => this.stopIndex == -1 && this.Token != null ? this.Token.TokenIndex : this.stopIndex;
      set => this.stopIndex = value;
    }

    public override int Type
    {
      get => this.Token == null ? 0 : this.Token.Type;
      set
      {
      }
    }

    public override ITree DupNode() => (ITree) new CommonTree(this);

    public virtual void SetUnknownTokenBoundaries()
    {
      if (this.Children == null)
      {
        if (this.startIndex >= 0 && this.stopIndex >= 0)
          return;
        this.startIndex = this.stopIndex = this.Token.TokenIndex;
      }
      else
      {
        foreach (ITree child in (IEnumerable<ITree>) this.Children)
        {
          if (child is CommonTree commonTree)
            commonTree.SetUnknownTokenBoundaries();
        }
        if (this.startIndex >= 0 && this.stopIndex >= 0 || this.Children.Count <= 0)
          return;
        ITree child1 = this.Children[0];
        ITree child2 = this.Children[this.Children.Count - 1];
        this.startIndex = child1.TokenStartIndex;
        this.stopIndex = child2.TokenStopIndex;
      }
    }

    public override string ToString()
    {
      if (this.IsNil)
        return "nil";
      if (this.Type == 0)
        return "<errornode>";
      return this.Token == null ? string.Empty : this.Token.Text;
    }
  }
}
