// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.IDebugEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal interface IDebugEventListener
  {
    void EnterRule(string grammarFileName, string ruleName);

    void EnterAlt(int alt);

    void ExitRule(string grammarFileName, string ruleName);

    void EnterSubRule(int decisionNumber);

    void ExitSubRule(int decisionNumber);

    void EnterDecision(int decisionNumber);

    void ExitDecision(int decisionNumber);

    void ConsumeToken(IToken t);

    void ConsumeHiddenToken(IToken t);

    void LT(int i, IToken t);

    void Mark(int marker);

    void Rewind(int marker);

    void Rewind();

    void BeginBacktrack(int level);

    void EndBacktrack(int level, bool successful);

    void Location(int line, int pos);

    void RecognitionException(Antlr.Runtime.RecognitionException e);

    void BeginResync();

    void EndResync();

    void SemanticPredicate(bool result, string predicate);

    void Commence();

    void Terminate();

    void ConsumeNode(object t);

    void LT(int i, object t);

    void GetNilNode(object t);

    void ErrorNode(object t);

    void CreateNode(object t);

    void CreateNode(object node, IToken token);

    void BecomeRoot(object newRoot, object oldRoot);

    void AddChild(object root, object child);

    void SetTokenBoundaries(object t, int tokenStartIndex, int tokenStopIndex);
  }
}
