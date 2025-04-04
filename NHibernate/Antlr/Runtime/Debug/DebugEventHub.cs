// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.DebugEventHub
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class DebugEventHub : IDebugEventListener
  {
    protected IList listeners = (IList) new ArrayList();

    public DebugEventHub(IDebugEventListener listener) => this.listeners.Add((object) listener);

    public DebugEventHub(params IDebugEventListener[] listeners)
    {
      foreach (object listener in listeners)
        this.listeners.Add(listener);
    }

    public void AddListener(IDebugEventListener listener) => this.listeners.Add((object) listener);

    public void EnterRule(string grammarFileName, string ruleName)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).EnterRule(grammarFileName, ruleName);
    }

    public void ExitRule(string grammarFileName, string ruleName)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).ExitRule(grammarFileName, ruleName);
    }

    public void EnterAlt(int alt)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).EnterAlt(alt);
    }

    public void EnterSubRule(int decisionNumber)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).EnterSubRule(decisionNumber);
    }

    public void ExitSubRule(int decisionNumber)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).ExitSubRule(decisionNumber);
    }

    public void EnterDecision(int decisionNumber)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).EnterDecision(decisionNumber);
    }

    public void ExitDecision(int decisionNumber)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).ExitDecision(decisionNumber);
    }

    public void Location(int line, int pos)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).Location(line, pos);
    }

    public void ConsumeToken(IToken token)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).ConsumeToken(token);
    }

    public void ConsumeHiddenToken(IToken token)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).ConsumeHiddenToken(token);
    }

    public void LT(int index, IToken t)
    {
      for (int index1 = 0; index1 < this.listeners.Count; ++index1)
        ((IDebugEventListener) this.listeners[index1]).LT(index, t);
    }

    public void Mark(int index)
    {
      for (int index1 = 0; index1 < this.listeners.Count; ++index1)
        ((IDebugEventListener) this.listeners[index1]).Mark(index);
    }

    public void Rewind(int index)
    {
      for (int index1 = 0; index1 < this.listeners.Count; ++index1)
        ((IDebugEventListener) this.listeners[index1]).Rewind(index);
    }

    public void Rewind()
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).Rewind();
    }

    public void BeginBacktrack(int level)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).BeginBacktrack(level);
    }

    public void EndBacktrack(int level, bool successful)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).EndBacktrack(level, successful);
    }

    public void RecognitionException(Antlr.Runtime.RecognitionException e)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).RecognitionException(e);
    }

    public void BeginResync()
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).BeginResync();
    }

    public void EndResync()
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).EndResync();
    }

    public void SemanticPredicate(bool result, string predicate)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).SemanticPredicate(result, predicate);
    }

    public void Commence()
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).Commence();
    }

    public void Terminate()
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).Terminate();
    }

    public void ConsumeNode(object t)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).ConsumeNode(t);
    }

    public void LT(int index, object t)
    {
      for (int index1 = 0; index1 < this.listeners.Count; ++index1)
        ((IDebugEventListener) this.listeners[index1]).LT(index, t);
    }

    public void GetNilNode(object t)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).GetNilNode(t);
    }

    public void ErrorNode(object t)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).ErrorNode(t);
    }

    public void CreateNode(object t)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).CreateNode(t);
    }

    public void CreateNode(object node, IToken token)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).CreateNode(node, token);
    }

    public void BecomeRoot(object newRoot, object oldRoot)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).BecomeRoot(newRoot, oldRoot);
    }

    public void AddChild(object root, object child)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).AddChild(root, child);
    }

    public void SetTokenBoundaries(object t, int tokenStartIndex, int tokenStopIndex)
    {
      for (int index = 0; index < this.listeners.Count; ++index)
        ((IDebugEventListener) this.listeners[index]).SetTokenBoundaries(t, tokenStartIndex, tokenStopIndex);
    }
  }
}
