// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.ITreeAdaptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal interface ITreeAdaptor
  {
    object Create(IToken payload);

    object DupNode(object treeNode);

    object DupTree(object tree);

    object GetNilNode();

    object ErrorNode(ITokenStream input, IToken start, IToken stop, RecognitionException e);

    bool IsNil(object tree);

    void AddChild(object t, object child);

    object BecomeRoot(object newRoot, object oldRoot);

    object RulePostProcessing(object root);

    int GetUniqueID(object node);

    object BecomeRoot(IToken newRoot, object oldRoot);

    object Create(int tokenType, IToken fromToken);

    object Create(int tokenType, IToken fromToken, string text);

    object Create(int tokenType, string text);

    int GetNodeType(object t);

    void SetNodeType(object t, int type);

    string GetNodeText(object t);

    void SetNodeText(object t, string text);

    IToken GetToken(object treeNode);

    void SetTokenBoundaries(object t, IToken startToken, IToken stopToken);

    int GetTokenStartIndex(object t);

    int GetTokenStopIndex(object t);

    object GetChild(object t, int i);

    void SetChild(object t, int i, object child);

    object DeleteChild(object t, int i);

    int GetChildCount(object t);

    object GetParent(object t);

    void SetParent(object t, object parent);

    int GetChildIndex(object t);

    void SetChildIndex(object t, int index);

    void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t);
  }
}
