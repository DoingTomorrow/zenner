// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.Tracer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class Tracer : BlankDebugEventListener
  {
    public IIntStream input;
    protected int level;

    public Tracer(IIntStream input) => this.input = input;

    public override void EnterRule(string grammarFileName, string ruleName)
    {
      for (int index = 1; index <= this.level; ++index)
        Console.Out.Write(" ");
      Console.Out.WriteLine("> " + grammarFileName + " " + ruleName + " lookahead(1)=" + this.GetInputSymbol(1));
      ++this.level;
    }

    public override void ExitRule(string grammarFileName, string ruleName)
    {
      --this.level;
      for (int index = 1; index <= this.level; ++index)
        Console.Out.Write(" ");
      Console.Out.WriteLine("< " + grammarFileName + " " + ruleName + " lookahead(1)=" + this.GetInputSymbol(1));
    }

    public virtual object GetInputSymbol(int k)
    {
      return this.input is ITokenStream ? (object) ((ITokenStream) this.input).LT(k) : (object) (char) this.input.LA(k);
    }
  }
}
