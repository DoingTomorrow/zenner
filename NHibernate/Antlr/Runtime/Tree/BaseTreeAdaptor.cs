// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.BaseTreeAdaptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal abstract class BaseTreeAdaptor : ITreeAdaptor
  {
    protected IDictionary treeToUniqueIDMap;
    protected int uniqueNodeID = 1;

    public virtual object GetNilNode() => this.Create((IToken) null);

    public virtual object ErrorNode(
      ITokenStream input,
      IToken start,
      IToken stop,
      RecognitionException e)
    {
      return (object) new CommonErrorNode(input, start, stop, e);
    }

    public virtual bool IsNil(object tree) => ((ITree) tree).IsNil;

    public virtual object DupTree(object tree) => this.DupTree(tree, (object) null);

    public virtual object DupTree(object t, object parent)
    {
      if (t == null)
        return (object) null;
      object t1 = this.DupNode(t);
      this.SetChildIndex(t1, this.GetChildIndex(t));
      this.SetParent(t1, parent);
      int childCount = this.GetChildCount(t);
      for (int i = 0; i < childCount; ++i)
      {
        object child = this.DupTree(this.GetChild(t, i), t);
        this.AddChild(t1, child);
      }
      return t1;
    }

    public virtual void AddChild(object t, object child)
    {
      if (t == null || child == null)
        return;
      ((ITree) t).AddChild((ITree) child);
    }

    public virtual object BecomeRoot(object newRoot, object oldRoot)
    {
      ITree tree = (ITree) newRoot;
      ITree t = (ITree) oldRoot;
      if (oldRoot == null)
        return newRoot;
      if (tree.IsNil)
      {
        int childCount = tree.ChildCount;
        if (childCount == 1)
          tree = tree.GetChild(0);
        else if (childCount > 1)
          throw new SystemException("more than one node as root (TODO: make exception hierarchy)");
      }
      tree.AddChild(t);
      return (object) tree;
    }

    public virtual object RulePostProcessing(object root)
    {
      ITree tree = (ITree) root;
      if (tree != null && tree.IsNil)
      {
        if (tree.ChildCount == 0)
          tree = (ITree) null;
        else if (tree.ChildCount == 1)
        {
          tree = tree.GetChild(0);
          tree.Parent = (ITree) null;
          tree.ChildIndex = -1;
        }
      }
      return (object) tree;
    }

    public virtual object BecomeRoot(IToken newRoot, object oldRoot)
    {
      return this.BecomeRoot(this.Create(newRoot), oldRoot);
    }

    public virtual object Create(int tokenType, IToken fromToken)
    {
      fromToken = this.CreateToken(fromToken);
      fromToken.Type = tokenType;
      return (object) (ITree) this.Create(fromToken);
    }

    public virtual object Create(int tokenType, IToken fromToken, string text)
    {
      fromToken = this.CreateToken(fromToken);
      fromToken.Type = tokenType;
      fromToken.Text = text;
      return (object) (ITree) this.Create(fromToken);
    }

    public virtual object Create(int tokenType, string text)
    {
      return (object) (ITree) this.Create(this.CreateToken(tokenType, text));
    }

    public virtual int GetNodeType(object t) => ((ITree) t).Type;

    public virtual void SetNodeType(object t, int type)
    {
      throw new NotImplementedException("don't know enough about Tree node");
    }

    public virtual string GetNodeText(object t) => ((ITree) t).Text;

    public virtual void SetNodeText(object t, string text)
    {
      throw new NotImplementedException("don't know enough about Tree node");
    }

    public virtual object GetChild(object t, int i) => (object) ((ITree) t).GetChild(i);

    public virtual void SetChild(object t, int i, object child)
    {
      ((ITree) t).SetChild(i, (ITree) child);
    }

    public virtual object DeleteChild(object t, int i) => ((ITree) t).DeleteChild(i);

    public virtual int GetChildCount(object t) => ((ITree) t).ChildCount;

    public abstract object DupNode(object param1);

    public abstract object Create(IToken param1);

    public abstract void SetTokenBoundaries(object param1, IToken param2, IToken param3);

    public abstract int GetTokenStartIndex(object t);

    public abstract int GetTokenStopIndex(object t);

    public abstract IToken GetToken(object treeNode);

    public int GetUniqueID(object node)
    {
      if (this.treeToUniqueIDMap == null)
        this.treeToUniqueIDMap = (IDictionary) new Hashtable();
      object treeToUniqueId = this.treeToUniqueIDMap[node];
      if (treeToUniqueId != null)
        return (int) treeToUniqueId;
      int uniqueNodeId = this.uniqueNodeID;
      this.treeToUniqueIDMap[node] = (object) uniqueNodeId;
      ++this.uniqueNodeID;
      return uniqueNodeId;
    }

    public abstract IToken CreateToken(int tokenType, string text);

    public abstract IToken CreateToken(IToken fromToken);

    public abstract object GetParent(object t);

    public abstract void SetParent(object t, object parent);

    public abstract int GetChildIndex(object t);

    public abstract void SetChildIndex(object t, int index);

    public abstract void ReplaceChildren(
      object parent,
      int startChildIndex,
      int stopChildIndex,
      object t);
  }
}
