// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.BaseTreeAdaptor
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public abstract class BaseTreeAdaptor : ITreeAdaptor
  {
    protected IDictionary<object, int> treeToUniqueIDMap;
    protected int uniqueNodeID = 1;

    public virtual object Nil() => this.Create((IToken) null);

    public virtual object ErrorNode(
      ITokenStream input,
      IToken start,
      IToken stop,
      RecognitionException e)
    {
      return (object) new CommonErrorNode(input, start, stop, e);
    }

    public virtual bool IsNil(object tree) => ((ITree) tree).IsNil;

    public virtual object DupNode(int type, object treeNode)
    {
      object t = this.DupNode(treeNode);
      this.SetType(t, type);
      return t;
    }

    public virtual object DupNode(object treeNode, string text)
    {
      object t = this.DupNode(treeNode);
      this.SetText(t, text);
      return t;
    }

    public virtual object DupNode(int type, object treeNode, string text)
    {
      object t = this.DupNode(treeNode);
      this.SetType(t, type);
      this.SetText(t, text);
      return t;
    }

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
          throw new Exception("more than one node as root (TODO: make exception hierarchy)");
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
      return this.Create(fromToken);
    }

    public virtual object Create(int tokenType, IToken fromToken, string text)
    {
      if (fromToken == null)
        return this.Create(tokenType, text);
      fromToken = this.CreateToken(fromToken);
      fromToken.Type = tokenType;
      fromToken.Text = text;
      return this.Create(fromToken);
    }

    public virtual object Create(IToken fromToken, string text)
    {
      fromToken = fromToken != null ? this.CreateToken(fromToken) : throw new ArgumentNullException(nameof (fromToken));
      fromToken.Text = text;
      return this.Create(fromToken);
    }

    public virtual object Create(int tokenType, string text)
    {
      return this.Create(this.CreateToken(tokenType, text));
    }

    public virtual int GetType(object t)
    {
      ITree tree = this.GetTree(t);
      return tree == null ? 0 : tree.Type;
    }

    public virtual void SetType(object t, int type)
    {
      throw new NotSupportedException("don't know enough about Tree node");
    }

    public virtual string GetText(object t) => this.GetTree(t)?.Text;

    public virtual void SetText(object t, string text)
    {
      throw new NotSupportedException("don't know enough about Tree node");
    }

    public virtual object GetChild(object t, int i)
    {
      ITree tree = this.GetTree(t);
      return tree == null ? (object) null : (object) tree.GetChild(i);
    }

    public virtual void SetChild(object t, int i, object child)
    {
      ITree tree1 = this.GetTree(t);
      if (tree1 == null)
        return;
      ITree tree2 = this.GetTree(child);
      tree1.SetChild(i, tree2);
    }

    public virtual object DeleteChild(object t, int i) => ((ITree) t).DeleteChild(i);

    public virtual int GetChildCount(object t)
    {
      ITree tree = this.GetTree(t);
      return tree == null ? 0 : tree.ChildCount;
    }

    public virtual int GetUniqueID(object node)
    {
      if (this.treeToUniqueIDMap == null)
        this.treeToUniqueIDMap = (IDictionary<object, int>) new Dictionary<object, int>();
      int uniqueId;
      if (this.treeToUniqueIDMap.TryGetValue(node, out uniqueId))
        return uniqueId;
      int uniqueNodeId = this.uniqueNodeID;
      this.treeToUniqueIDMap[node] = uniqueNodeId;
      ++this.uniqueNodeID;
      return uniqueNodeId;
    }

    public abstract IToken CreateToken(int tokenType, string text);

    public abstract IToken CreateToken(IToken fromToken);

    public abstract object Create(IToken payload);

    public virtual object DupNode(object treeNode)
    {
      ITree tree = this.GetTree(treeNode);
      return tree == null ? (object) null : (object) tree.DupNode();
    }

    public abstract IToken GetToken(object t);

    public virtual void SetTokenBoundaries(object t, IToken startToken, IToken stopToken)
    {
      ITree tree = this.GetTree(t);
      if (tree == null)
        return;
      int num1 = 0;
      int num2 = 0;
      if (startToken != null)
        num1 = startToken.TokenIndex;
      if (stopToken != null)
        num2 = stopToken.TokenIndex;
      tree.TokenStartIndex = num1;
      tree.TokenStopIndex = num2;
    }

    public virtual int GetTokenStartIndex(object t)
    {
      ITree tree = this.GetTree(t);
      return tree == null ? -1 : tree.TokenStartIndex;
    }

    public virtual int GetTokenStopIndex(object t)
    {
      ITree tree = this.GetTree(t);
      return tree == null ? -1 : tree.TokenStopIndex;
    }

    public virtual object GetParent(object t)
    {
      ITree tree = this.GetTree(t);
      return tree == null ? (object) null : (object) tree.Parent;
    }

    public virtual void SetParent(object t, object parent)
    {
      ITree tree1 = this.GetTree(t);
      if (tree1 == null)
        return;
      ITree tree2 = this.GetTree(parent);
      tree1.Parent = tree2;
    }

    public virtual int GetChildIndex(object t)
    {
      ITree tree = this.GetTree(t);
      return tree == null ? 0 : tree.ChildIndex;
    }

    public virtual void SetChildIndex(object t, int index)
    {
      ITree tree = this.GetTree(t);
      if (tree == null)
        return;
      tree.ChildIndex = index;
    }

    public virtual void ReplaceChildren(
      object parent,
      int startChildIndex,
      int stopChildIndex,
      object t)
    {
      this.GetTree(parent)?.ReplaceChildren(startChildIndex, stopChildIndex, t);
    }

    protected virtual ITree GetTree(object t)
    {
      if (t == null)
        return (ITree) null;
      return t is ITree tree ? tree : throw new NotSupportedException();
    }
  }
}
