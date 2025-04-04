// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.DebugTreeAdaptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class DebugTreeAdaptor : ITreeAdaptor
  {
    protected IDebugEventListener dbg;
    protected ITreeAdaptor adaptor;

    public DebugTreeAdaptor(IDebugEventListener dbg, ITreeAdaptor adaptor)
    {
      this.dbg = dbg;
      this.adaptor = adaptor;
    }

    public object Create(IToken payload)
    {
      if (payload.TokenIndex < 0)
        return this.Create(payload.Type, payload.Text);
      object node = this.adaptor.Create(payload);
      this.dbg.CreateNode(node, payload);
      return node;
    }

    public object ErrorNode(ITokenStream input, IToken start, IToken stop, RecognitionException e)
    {
      object t = this.adaptor.ErrorNode(input, start, stop, e);
      if (t != null)
        this.dbg.ErrorNode(t);
      return t;
    }

    public object DupTree(object tree)
    {
      object t = this.adaptor.DupTree(tree);
      this.SimulateTreeConstruction(t);
      return t;
    }

    protected void SimulateTreeConstruction(object t)
    {
      this.dbg.CreateNode(t);
      int childCount = this.adaptor.GetChildCount(t);
      for (int i = 0; i < childCount; ++i)
      {
        object child = this.adaptor.GetChild(t, i);
        this.SimulateTreeConstruction(child);
        this.dbg.AddChild(t, child);
      }
    }

    public object DupNode(object treeNode)
    {
      object t = this.adaptor.DupNode(treeNode);
      this.dbg.CreateNode(t);
      return t;
    }

    public object GetNilNode()
    {
      object nilNode = this.adaptor.GetNilNode();
      this.dbg.GetNilNode(nilNode);
      return nilNode;
    }

    public bool IsNil(object tree) => this.adaptor.IsNil(tree);

    public void AddChild(object t, object child)
    {
      if (t == null || child == null)
        return;
      this.adaptor.AddChild(t, child);
      this.dbg.AddChild(t, child);
    }

    public object BecomeRoot(object newRoot, object oldRoot)
    {
      object obj = this.adaptor.BecomeRoot(newRoot, oldRoot);
      this.dbg.BecomeRoot(newRoot, oldRoot);
      return obj;
    }

    public object RulePostProcessing(object root) => this.adaptor.RulePostProcessing(root);

    public void AddChild(object t, IToken child)
    {
      object child1 = this.Create(child);
      this.AddChild(t, child1);
    }

    public object BecomeRoot(IToken newRoot, object oldRoot)
    {
      object newRoot1 = this.Create(newRoot);
      this.adaptor.BecomeRoot(newRoot1, oldRoot);
      this.dbg.BecomeRoot((object) newRoot, oldRoot);
      return newRoot1;
    }

    public object Create(int tokenType, IToken fromToken)
    {
      object t = this.adaptor.Create(tokenType, fromToken);
      this.dbg.CreateNode(t);
      return t;
    }

    public object Create(int tokenType, IToken fromToken, string text)
    {
      object t = this.adaptor.Create(tokenType, fromToken, text);
      this.dbg.CreateNode(t);
      return t;
    }

    public object Create(int tokenType, string text)
    {
      object t = this.adaptor.Create(tokenType, text);
      this.dbg.CreateNode(t);
      return t;
    }

    public int GetNodeType(object t) => this.adaptor.GetNodeType(t);

    public void SetNodeType(object t, int type) => this.adaptor.SetNodeType(t, type);

    public string GetNodeText(object t) => this.adaptor.GetNodeText(t);

    public void SetNodeText(object t, string text) => this.adaptor.SetNodeText(t, text);

    public IToken GetToken(object treeNode) => this.adaptor.GetToken(treeNode);

    public void SetTokenBoundaries(object t, IToken startToken, IToken stopToken)
    {
      this.adaptor.SetTokenBoundaries(t, startToken, stopToken);
      if (t == null || startToken == null || stopToken == null)
        return;
      this.dbg.SetTokenBoundaries(t, startToken.TokenIndex, stopToken.TokenIndex);
    }

    public int GetTokenStartIndex(object t) => this.adaptor.GetTokenStartIndex(t);

    public int GetTokenStopIndex(object t) => this.adaptor.GetTokenStopIndex(t);

    public object GetChild(object t, int i) => this.adaptor.GetChild(t, i);

    public void SetChild(object t, int i, object child) => this.adaptor.SetChild(t, i, child);

    public object DeleteChild(object t, int i) => this.adaptor.DeleteChild(t, i);

    public int GetChildCount(object t) => this.adaptor.GetChildCount(t);

    public int GetUniqueID(object node) => this.adaptor.GetUniqueID(node);

    public object GetParent(object t) => this.adaptor.GetParent(t);

    public int GetChildIndex(object t) => this.adaptor.GetChildIndex(t);

    public void SetParent(object t, object parent) => this.adaptor.SetParent(t, parent);

    public void SetChildIndex(object t, int index) => this.adaptor.SetChildIndex(t, index);

    public void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t)
    {
      this.adaptor.ReplaceChildren(parent, startChildIndex, stopChildIndex, t);
    }

    public IDebugEventListener DebugListener
    {
      get => this.dbg;
      set => this.dbg = value;
    }

    public ITreeAdaptor TreeAdaptor => this.adaptor;
  }
}
