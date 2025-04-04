// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.ParseTreeBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System.Collections;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class ParseTreeBuilder : BlankDebugEventListener
  {
    public static readonly string EPSILON_PAYLOAD = "<epsilon>";
    private Stack callStack = new Stack();
    private IList hiddenTokens = (IList) new ArrayList();
    private int backtracking;

    public ParseTreeBuilder(string grammarName)
    {
      this.callStack.Push((object) this.Create((object) ("<grammar " + grammarName + ">")));
    }

    public ParseTree Tree => (ParseTree) this.callStack.Peek();

    public ParseTree Create(object payload) => new ParseTree(payload);

    public ParseTree EpsilonNode() => this.Create((object) ParseTreeBuilder.EPSILON_PAYLOAD);

    public override void EnterDecision(int d) => ++this.backtracking;

    public override void ExitDecision(int i) => --this.backtracking;

    public override void EnterRule(string filename, string ruleName)
    {
      if (this.backtracking > 0)
        return;
      ParseTree parseTree = (ParseTree) this.callStack.Peek();
      ParseTree t = this.Create((object) ruleName);
      parseTree.AddChild((ITree) t);
      this.callStack.Push((object) t);
    }

    public override void ExitRule(string filename, string ruleName)
    {
      if (this.backtracking > 0)
        return;
      ParseTree parseTree = (ParseTree) this.callStack.Peek();
      if (parseTree.ChildCount == 0)
        parseTree.AddChild((ITree) this.EpsilonNode());
      this.callStack.Pop();
    }

    public override void ConsumeToken(IToken token)
    {
      if (this.backtracking > 0)
        return;
      ParseTree parseTree = (ParseTree) this.callStack.Peek();
      ParseTree t = this.Create((object) token);
      t.hiddenTokens = this.hiddenTokens;
      this.hiddenTokens = (IList) new ArrayList();
      parseTree.AddChild((ITree) t);
    }

    public override void ConsumeHiddenToken(IToken token)
    {
      if (this.backtracking > 0)
        return;
      this.hiddenTokens.Add((object) token);
    }

    public override void RecognitionException(Antlr.Runtime.RecognitionException e)
    {
      if (this.backtracking > 0)
        return;
      ((BaseTree) this.callStack.Peek()).AddChild((ITree) this.Create((object) e));
    }
  }
}
