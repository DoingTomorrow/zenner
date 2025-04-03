// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.IDebugEventListener
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

#nullable disable
namespace Antlr.Runtime.Debug
{
  public interface IDebugEventListener
  {
    void Initialize();

    void EnterRule(string grammarFileName, string ruleName);

    void EnterAlt(int alt);

    void ExitRule(string grammarFileName, string ruleName);

    void EnterSubRule(int decisionNumber);

    void ExitSubRule(int decisionNumber);

    void EnterDecision(int decisionNumber, bool couldBacktrack);

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

    void NilNode(object t);

    void ErrorNode(object t);

    void CreateNode(object t);

    void CreateNode(object node, IToken token);

    void BecomeRoot(object newRoot, object oldRoot);

    void AddChild(object root, object child);

    void SetTokenBoundaries(object t, int tokenStartIndex, int tokenStopIndex);
  }
}
