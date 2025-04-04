// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.BlankDebugEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class BlankDebugEventListener : IDebugEventListener
  {
    public virtual void EnterRule(string grammarFileName, string ruleName)
    {
    }

    public virtual void ExitRule(string grammarFileName, string ruleName)
    {
    }

    public virtual void EnterAlt(int alt)
    {
    }

    public virtual void EnterSubRule(int decisionNumber)
    {
    }

    public virtual void ExitSubRule(int decisionNumber)
    {
    }

    public virtual void EnterDecision(int decisionNumber)
    {
    }

    public virtual void ExitDecision(int decisionNumber)
    {
    }

    public virtual void Location(int line, int pos)
    {
    }

    public virtual void ConsumeToken(IToken token)
    {
    }

    public virtual void ConsumeHiddenToken(IToken token)
    {
    }

    public virtual void LT(int i, IToken t)
    {
    }

    public virtual void Mark(int i)
    {
    }

    public virtual void Rewind(int i)
    {
    }

    public virtual void Rewind()
    {
    }

    public virtual void BeginBacktrack(int level)
    {
    }

    public virtual void EndBacktrack(int level, bool successful)
    {
    }

    public virtual void RecognitionException(Antlr.Runtime.RecognitionException e)
    {
    }

    public virtual void BeginResync()
    {
    }

    public virtual void EndResync()
    {
    }

    public virtual void SemanticPredicate(bool result, string predicate)
    {
    }

    public virtual void Commence()
    {
    }

    public virtual void Terminate()
    {
    }

    public virtual void ConsumeNode(object t)
    {
    }

    public virtual void LT(int i, object t)
    {
    }

    public virtual void GetNilNode(object t)
    {
    }

    public virtual void ErrorNode(object t)
    {
    }

    public virtual void CreateNode(object t)
    {
    }

    public virtual void CreateNode(object node, IToken token)
    {
    }

    public virtual void BecomeRoot(object newRoot, object oldRoot)
    {
    }

    public virtual void AddChild(object root, object child)
    {
    }

    public virtual void SetTokenBoundaries(object t, int tokenStartIndex, int tokenStopIndex)
    {
    }
  }
}
