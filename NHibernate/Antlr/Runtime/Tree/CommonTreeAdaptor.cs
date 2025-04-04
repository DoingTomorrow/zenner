// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.CommonTreeAdaptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal class CommonTreeAdaptor : BaseTreeAdaptor
  {
    public override object DupNode(object t)
    {
      return t == null ? (object) null : (object) ((ITree) t).DupNode();
    }

    public override object Create(IToken payload) => (object) new CommonTree(payload);

    public override IToken CreateToken(int tokenType, string text)
    {
      return (IToken) new CommonToken(tokenType, text);
    }

    public override IToken CreateToken(IToken fromToken) => (IToken) new CommonToken(fromToken);

    public override void SetTokenBoundaries(object t, IToken startToken, IToken stopToken)
    {
      if (t == null)
        return;
      int num1 = 0;
      int num2 = 0;
      if (startToken != null)
        num1 = startToken.TokenIndex;
      if (stopToken != null)
        num2 = stopToken.TokenIndex;
      ((ITree) t).TokenStartIndex = num1;
      ((ITree) t).TokenStopIndex = num2;
    }

    public override int GetTokenStartIndex(object t)
    {
      return t == null ? -1 : ((ITree) t).TokenStartIndex;
    }

    public override int GetTokenStopIndex(object t) => t == null ? -1 : ((ITree) t).TokenStopIndex;

    public override string GetNodeText(object t) => ((ITree) t)?.Text;

    public override int GetNodeType(object t) => t == null ? 0 : ((ITree) t).Type;

    public override IToken GetToken(object treeNode)
    {
      return treeNode is CommonTree ? ((CommonTree) treeNode).Token : (IToken) null;
    }

    public override object GetChild(object t, int i)
    {
      return t == null ? (object) null : (object) ((ITree) t).GetChild(i);
    }

    public override int GetChildCount(object t) => t == null ? 0 : ((ITree) t).ChildCount;

    public override object GetParent(object t)
    {
      return t == null ? (object) null : (object) ((ITree) t).Parent;
    }

    public override void SetParent(object t, object parent)
    {
      if (t != null)
        return;
      ((ITree) t).Parent = (ITree) parent;
    }

    public override int GetChildIndex(object t) => t == null ? 0 : ((ITree) t).ChildIndex;

    public override void SetChildIndex(object t, int index)
    {
      if (t != null)
        return;
      ((ITree) t).ChildIndex = index;
    }

    public override void ReplaceChildren(
      object parent,
      int startChildIndex,
      int stopChildIndex,
      object t)
    {
      ((ITree) parent)?.ReplaceChildren(startChildIndex, stopChildIndex, t);
    }
  }
}
