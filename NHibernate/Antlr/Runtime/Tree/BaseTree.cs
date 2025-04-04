// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.BaseTree
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  internal abstract class BaseTree : ITree
  {
    protected IList children;

    public BaseTree()
    {
    }

    public BaseTree(ITree node)
    {
    }

    public virtual int ChildCount => this.children == null ? 0 : this.children.Count;

    public virtual bool IsNil => false;

    public virtual int Line => 0;

    public virtual int CharPositionInLine => 0;

    public virtual ITree GetChild(int i)
    {
      return this.children == null || i >= this.children.Count ? (ITree) null : (ITree) this.children[i];
    }

    public IList Children => this.children;

    public virtual void AddChild(ITree t)
    {
      if (t == null)
        return;
      BaseTree baseTree = (BaseTree) t;
      if (baseTree.IsNil)
      {
        if (this.children != null && this.children == baseTree.children)
          throw new InvalidOperationException("attempt to add child list to itself");
        if (baseTree.children == null)
          return;
        if (this.children != null)
        {
          int count = baseTree.children.Count;
          for (int index = 0; index < count; ++index)
          {
            ITree child = (ITree) baseTree.Children[index];
            this.children.Add((object) child);
            child.Parent = (ITree) this;
            child.ChildIndex = this.children.Count - 1;
          }
        }
        else
        {
          this.children = baseTree.children;
          this.FreshenParentAndChildIndexes();
        }
      }
      else
      {
        if (this.children == null)
          this.children = this.CreateChildrenList();
        this.children.Add((object) t);
        baseTree.Parent = (ITree) this;
        baseTree.ChildIndex = this.children.Count - 1;
      }
    }

    public void AddChildren(IList kids)
    {
      for (int index = 0; index < kids.Count; ++index)
        this.AddChild((ITree) kids[index]);
    }

    public virtual void SetChild(int i, ITree t)
    {
      if (t == null)
        return;
      if (t.IsNil)
        throw new ArgumentException("Can't set single child to a list");
      if (this.children == null)
        this.children = this.CreateChildrenList();
      this.children[i] = (object) t;
      t.Parent = (ITree) this;
      t.ChildIndex = i;
    }

    public virtual object DeleteChild(int i)
    {
      if (this.children == null)
        return (object) null;
      ITree child = (ITree) this.children[i];
      this.children.RemoveAt(i);
      this.FreshenParentAndChildIndexes(i);
      return (object) child;
    }

    public virtual void ReplaceChildren(int startChildIndex, int stopChildIndex, object t)
    {
      if (this.children == null)
        throw new ArgumentException("indexes invalid; no children in list");
      int num1 = stopChildIndex - startChildIndex + 1;
      BaseTree baseTree1 = (BaseTree) t;
      IList list;
      if (baseTree1.IsNil)
      {
        list = baseTree1.Children;
      }
      else
      {
        list = (IList) new ArrayList(1);
        list.Add((object) baseTree1);
      }
      int count1 = list.Count;
      int count2 = list.Count;
      int num2 = num1 - count1;
      if (num2 == 0)
      {
        int index1 = 0;
        for (int index2 = startChildIndex; index2 <= stopChildIndex; ++index2)
        {
          BaseTree baseTree2 = (BaseTree) list[index1];
          this.children[index2] = (object) baseTree2;
          baseTree2.Parent = (ITree) this;
          baseTree2.ChildIndex = index2;
          ++index1;
        }
      }
      else if (num2 > 0)
      {
        for (int index = 0; index < count2; ++index)
          this.children[startChildIndex + index] = list[index];
        int index3 = startChildIndex + count2;
        for (int index4 = index3; index4 <= stopChildIndex; ++index4)
          this.children.RemoveAt(index3);
        this.FreshenParentAndChildIndexes(startChildIndex);
      }
      else
      {
        int index;
        for (index = 0; index < num1; ++index)
          this.children[startChildIndex + index] = list[index];
        for (; index < count1; ++index)
          this.children.Insert(startChildIndex + index, list[index]);
        this.FreshenParentAndChildIndexes(startChildIndex);
      }
    }

    protected internal virtual IList CreateChildrenList() => (IList) new ArrayList();

    public virtual void FreshenParentAndChildIndexes() => this.FreshenParentAndChildIndexes(0);

    public virtual void FreshenParentAndChildIndexes(int offset)
    {
      int childCount = this.ChildCount;
      for (int i = offset; i < childCount; ++i)
      {
        ITree child = this.GetChild(i);
        child.ChildIndex = i;
        child.Parent = (ITree) this;
      }
    }

    public virtual void SanityCheckParentAndChildIndexes()
    {
      this.SanityCheckParentAndChildIndexes((ITree) null, -1);
    }

    public virtual void SanityCheckParentAndChildIndexes(ITree parent, int i)
    {
      if (parent != this.Parent)
        throw new ArgumentException("parents don't match; expected " + (object) parent + " found " + (object) this.Parent);
      if (i != this.ChildIndex)
        throw new NotSupportedException("child indexes don't match; expected " + (object) i + " found " + (object) this.ChildIndex);
      int childCount = this.ChildCount;
      for (int i1 = 0; i1 < childCount; ++i1)
        ((BaseTree) this.GetChild(i1)).SanityCheckParentAndChildIndexes((ITree) this, i1);
    }

    public virtual int ChildIndex
    {
      get => 0;
      set
      {
      }
    }

    public virtual ITree Parent
    {
      get => (ITree) null;
      set
      {
      }
    }

    public bool HasAncestor(int ttype) => this.GetAncestor(ttype) != null;

    public ITree GetAncestor(int ttype)
    {
      for (ITree parent = this.Parent; parent != null; parent = parent.Parent)
      {
        if (parent.Type == ttype)
          return parent;
      }
      return (ITree) null;
    }

    public IList GetAncestors()
    {
      if (this.Parent == null)
        return (IList) null;
      IList ancestors = (IList) new ArrayList();
      for (ITree parent = this.Parent; parent != null; parent = parent.Parent)
        ancestors.Insert(0, (object) parent);
      return ancestors;
    }

    public virtual string ToStringTree()
    {
      if (this.children == null || this.children.Count == 0)
        return this.ToString();
      StringBuilder stringBuilder = new StringBuilder();
      if (!this.IsNil)
      {
        stringBuilder.Append("(");
        stringBuilder.Append(this.ToString());
        stringBuilder.Append(' ');
      }
      for (int index = 0; this.children != null && index < this.children.Count; ++index)
      {
        ITree child = (ITree) this.children[index];
        if (index > 0)
          stringBuilder.Append(' ');
        stringBuilder.Append(child.ToStringTree());
      }
      if (!this.IsNil)
        stringBuilder.Append(")");
      return stringBuilder.ToString();
    }

    public abstract override string ToString();

    public abstract ITree DupNode();

    public abstract int Type { get; }

    public abstract int TokenStartIndex { get; set; }

    public abstract int TokenStopIndex { get; set; }

    public abstract string Text { get; }
  }
}
