// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.DebugEventRepeater
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class DebugEventRepeater : IDebugEventListener
  {
    protected IDebugEventListener listener;

    public DebugEventRepeater(IDebugEventListener listener) => this.listener = listener;

    public void EnterRule(string grammarFileName, string ruleName)
    {
      this.listener.EnterRule(grammarFileName, ruleName);
    }

    public void ExitRule(string grammarFileName, string ruleName)
    {
      this.listener.ExitRule(grammarFileName, ruleName);
    }

    public void EnterAlt(int alt) => this.listener.EnterAlt(alt);

    public void EnterSubRule(int decisionNumber) => this.listener.EnterSubRule(decisionNumber);

    public void ExitSubRule(int decisionNumber) => this.listener.ExitSubRule(decisionNumber);

    public void EnterDecision(int decisionNumber) => this.listener.EnterDecision(decisionNumber);

    public void ExitDecision(int decisionNumber) => this.listener.ExitDecision(decisionNumber);

    public void Location(int line, int pos) => this.listener.Location(line, pos);

    public void ConsumeToken(IToken token) => this.listener.ConsumeToken(token);

    public void ConsumeHiddenToken(IToken token) => this.listener.ConsumeHiddenToken(token);

    public void LT(int i, IToken t) => this.listener.LT(i, t);

    public void Mark(int i) => this.listener.Mark(i);

    public void Rewind(int i) => this.listener.Rewind(i);

    public void Rewind() => this.listener.Rewind();

    public void BeginBacktrack(int level) => this.listener.BeginBacktrack(level);

    public void EndBacktrack(int level, bool successful)
    {
      this.listener.EndBacktrack(level, successful);
    }

    public void RecognitionException(Antlr.Runtime.RecognitionException e)
    {
      this.listener.RecognitionException(e);
    }

    public void BeginResync() => this.listener.BeginResync();

    public void EndResync() => this.listener.EndResync();

    public void SemanticPredicate(bool result, string predicate)
    {
      this.listener.SemanticPredicate(result, predicate);
    }

    public void Commence() => this.listener.Commence();

    public void Terminate() => this.listener.Terminate();

    public void ConsumeNode(object t) => this.listener.ConsumeNode(t);

    public void LT(int i, object t) => this.listener.LT(i, t);

    public void GetNilNode(object t) => this.listener.GetNilNode(t);

    public void ErrorNode(object t) => this.listener.ErrorNode(t);

    public void CreateNode(object t) => this.listener.CreateNode(t);

    public void CreateNode(object node, IToken token) => this.listener.CreateNode(node, token);

    public void BecomeRoot(object newRoot, object oldRoot)
    {
      this.listener.BecomeRoot(newRoot, oldRoot);
    }

    public void AddChild(object root, object child) => this.listener.AddChild(root, child);

    public void SetTokenBoundaries(object t, int tokenStartIndex, int tokenStopIndex)
    {
      this.listener.SetTokenBoundaries(t, tokenStartIndex, tokenStopIndex);
    }
  }
}
