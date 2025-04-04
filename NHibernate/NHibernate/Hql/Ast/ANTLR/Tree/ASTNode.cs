// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ASTNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class ASTNode : IASTNode, IEnumerable<IASTNode>, IEnumerable, ITree
  {
    private int _startIndex;
    private int _stopIndex;
    private int _childIndex;
    private IASTNode _parent;
    private readonly IToken _token;
    private List<IASTNode> _children;

    public ASTNode()
      : this((IToken) null)
    {
    }

    public ASTNode(IToken token)
    {
      this._startIndex = -1;
      this._stopIndex = -1;
      this._childIndex = -1;
      this._token = token;
    }

    public ASTNode(ASTNode other)
    {
      this._startIndex = -1;
      this._stopIndex = -1;
      this._childIndex = -1;
      this._token = (IToken) new HqlToken(other._token);
      this._startIndex = other._startIndex;
      this._stopIndex = other._stopIndex;
    }

    public bool IsNil => this._token == null;

    public int Type
    {
      get => this._token == null ? 0 : this._token.Type;
      set
      {
        if (this._token == null)
          return;
        this._token.Type = value;
      }
    }

    public virtual string Text
    {
      get => this._token == null ? (string) null : this._token.Text;
      set
      {
        if (this._token == null)
          return;
        this._token.Text = value;
      }
    }

    public IASTNode Parent
    {
      get => this._parent;
      set => this._parent = value;
    }

    public int ChildCount => this._children == null ? 0 : this._children.Count;

    public int ChildIndex => this._childIndex;

    public int Line
    {
      get
      {
        if (this._token != null && this._token.Line != 0)
          return this._token.Line;
        return this.ChildCount > 0 ? this.GetChild(0).Line : 0;
      }
    }

    public int CharPositionInLine
    {
      get
      {
        if (this._token != null && this._token.CharPositionInLine != 0)
          return this._token.CharPositionInLine;
        return this.ChildCount > 0 ? this.GetChild(0).CharPositionInLine : 0;
      }
    }

    public IASTNode AddChild(IASTNode child)
    {
      if (child != null)
      {
        ASTNode astNode = (ASTNode) child;
        if (astNode.IsNil)
        {
          if (this._children != null && this._children == astNode._children)
            throw new InvalidOperationException("attempt to add child list to itself");
          if (astNode._children != null)
          {
            if (this._children != null)
            {
              int count = astNode._children.Count;
              for (int index = 0; index < count; ++index)
              {
                ASTNode child1 = (ASTNode) astNode._children[index];
                this._children.Add((IASTNode) child1);
                child1._parent = (IASTNode) this;
                child1._childIndex = this._children.Count - 1;
              }
            }
            else
            {
              this._children = astNode._children;
              this.FreshenParentAndChildIndexes();
            }
          }
        }
        else
        {
          if (this._children == null)
            this._children = new List<IASTNode>();
          this._children.Add((IASTNode) astNode);
          astNode._parent = (IASTNode) this;
          astNode._childIndex = this._children.Count - 1;
        }
      }
      return child;
    }

    public IASTNode InsertChild(int index, IASTNode child)
    {
      this._children.Insert(index, child);
      this.FreshenParentAndChildIndexes(index);
      return child;
    }

    public IASTNode AddSibling(IASTNode newSibling)
    {
      return this._parent.InsertChild(this.ChildIndex + 1, newSibling);
    }

    public void RemoveChild(IASTNode child) => this.RemoveChild(child.ChildIndex);

    public void RemoveChild(int index)
    {
      this._children.RemoveAt(index);
      this.FreshenParentAndChildIndexes(index);
    }

    public void ClearChildren()
    {
      if (this._children == null)
        return;
      this._children.Clear();
    }

    public void AddChildren(IEnumerable<IASTNode> children)
    {
      if (this._children == null)
        this._children = new List<IASTNode>();
      int count = this._children.Count;
      this._children.AddRange(children);
      this.FreshenParentAndChildIndexes(count);
    }

    public void AddChildren(params IASTNode[] children)
    {
      if (this._children == null)
        this._children = new List<IASTNode>();
      int count = this._children.Count;
      this._children.AddRange((IEnumerable<IASTNode>) children);
      this.FreshenParentAndChildIndexes(count);
    }

    public IASTNode DupNode() => (IASTNode) new ASTNode(this);

    public IASTNode NextSibling
    {
      get
      {
        return this._parent != null && this._parent.ChildCount > this._childIndex + 1 ? this._parent.GetChild(this._childIndex + 1) : (IASTNode) null;
      }
      set
      {
        if (this._parent == null)
          throw new InvalidOperationException("Trying set NextSibling without a parent.");
        if (this._parent.ChildCount > this.ChildIndex + 1)
          this._parent.SetChild(this.ChildIndex + 1, value);
        else
          this.AddSibling(value);
      }
    }

    public IASTNode GetChild(int index)
    {
      return this._children == null || this._children.Count - 1 < index ? (IASTNode) null : this._children[index];
    }

    public IASTNode GetFirstChild() => this.GetChild(0);

    public void SetFirstChild(IASTNode newChild)
    {
      if (this._children == null || this._children.Count == 0)
        this.AddChild(newChild);
      else
        this.SetChild(0, newChild);
    }

    public void SetChild(int index, IASTNode newChild)
    {
      if (this._children == null || this._children.Count <= index)
        throw new InvalidOperationException();
      ASTNode astNode = (ASTNode) newChild;
      astNode.Parent = (IASTNode) this;
      astNode._childIndex = index;
      this._children[index] = (IASTNode) astNode;
    }

    public IToken Token => this._token;

    public override string ToString()
    {
      if (this.IsNil)
        return "nil";
      if (this.Type == 0)
        return "<errornode>";
      return this._token == null ? (string) null : this._token.Text;
    }

    public string ToStringTree()
    {
      if (this._children == null || this._children.Count == 0)
        return this.ToString();
      StringBuilder stringBuilder = new StringBuilder();
      if (!this.IsNil)
      {
        stringBuilder.Append("( ");
        stringBuilder.Append(this.ToString());
      }
      foreach (ASTNode child in this._children)
      {
        stringBuilder.Append(' ');
        stringBuilder.Append(child.ToStringTree());
      }
      if (!this.IsNil)
        stringBuilder.Append(" )");
      return stringBuilder.ToString();
    }

    public IEnumerator<IASTNode> GetEnumerator()
    {
      if (this._children == null)
        this._children = new List<IASTNode>();
      return (IEnumerator<IASTNode>) this._children.GetEnumerator();
    }

    public bool HasAncestor(int ttype) => throw new NotImplementedException();

    public ITree GetAncestor(int ttype) => throw new NotImplementedException();

    public IList GetAncestors() => throw new NotImplementedException();

    void ITree.FreshenParentAndChildIndexes() => this.FreshenParentAndChildIndexes();

    ITree ITree.GetChild(int i) => (ITree) this.GetChild(i);

    void ITree.AddChild(ITree t) => this.AddChild((IASTNode) t);

    void ITree.SetChild(int i, ITree t)
    {
      ASTNode astNode = (ASTNode) t;
      this._children[i] = (IASTNode) astNode;
      astNode.Parent = (IASTNode) this;
      astNode._childIndex = i;
    }

    object ITree.DeleteChild(int i)
    {
      object child = (object) this._children[i];
      this.RemoveChild(i);
      return child;
    }

    void ITree.ReplaceChildren(int startChildIndex, int stopChildIndex, object t)
    {
      if (this._children != null)
        this._children.RemoveRange(startChildIndex, stopChildIndex - startChildIndex + 1);
      if (this._children == null)
        this._children = new List<IASTNode>();
      switch (t)
      {
        case IASTNode _:
          this._children.Insert(startChildIndex, (IASTNode) t);
          break;
        case IEnumerable enumerable:
          int num = 0;
          IEnumerator enumerator = enumerable.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              IASTNode current = (IASTNode) enumerator.Current;
              this._children.Insert(startChildIndex + num, current);
              ++num;
            }
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
      }
      this.FreshenParentAndChildIndexes(startChildIndex);
    }

    ITree ITree.DupNode() => (ITree) this.DupNode();

    int ITree.ChildIndex { get; set; }

    ITree ITree.Parent
    {
      get => (ITree) this.Parent;
      set => this.Parent = (IASTNode) value;
    }

    int ITree.TokenStartIndex
    {
      get
      {
        return this._startIndex == -1 && this._token != null ? this._token.TokenIndex : this._startIndex;
      }
      set => this._startIndex = value;
    }

    int ITree.TokenStopIndex
    {
      get
      {
        return this._stopIndex == -1 && this._token != null ? this._token.TokenIndex : this._stopIndex;
      }
      set => this._stopIndex = value;
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private void FreshenParentAndChildIndexes() => this.FreshenParentAndChildIndexes(0);

    private void FreshenParentAndChildIndexes(int offset)
    {
      for (int index = offset; index < this._children.Count; ++index)
      {
        ASTNode child = (ASTNode) this._children[index];
        child._childIndex = index;
        child._parent = (IASTNode) this;
      }
    }
  }
}
