// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ASTTreeAdaptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class ASTTreeAdaptor : BaseTreeAdaptor
  {
    public override object DupNode(object t)
    {
      return t == null ? (object) null : (object) ((IASTNode) t).DupNode();
    }

    public override object Create(IToken payload) => (object) new ASTNode(payload);

    public override IToken GetToken(object treeNode) => ((ASTNode) treeNode).Token;

    public override IToken CreateToken(int tokenType, string text)
    {
      return (IToken) new CommonToken(tokenType, text);
    }

    public override IToken CreateToken(IToken fromToken) => (IToken) new CommonToken(fromToken);

    public override object GetParent(object t) => throw new NotImplementedException();

    public override void SetParent(object t, object parent) => ((ITree) t).Parent = (ITree) parent;

    public override int GetChildIndex(object t) => ((ITree) t).ChildIndex;

    public override void SetChildIndex(object t, int index) => ((ITree) t).ChildIndex = index;

    public override int GetNodeType(object t) => ((ITree) t).Type;

    public override void ReplaceChildren(
      object parent,
      int startChildIndex,
      int stopChildIndex,
      object t)
    {
      throw new NotImplementedException();
    }

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

    public override object ErrorNode(
      ITokenStream input,
      IToken start,
      IToken stop,
      RecognitionException e)
    {
      return (object) new ASTErrorNode(input, start, stop, e);
    }

    public override int GetTokenStartIndex(object t) => throw new NotImplementedException();

    public override int GetTokenStopIndex(object t) => throw new NotImplementedException();
  }
}
