// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.UnbufferedTokenStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using Antlr.Runtime.Misc;
using System;

#nullable disable
namespace Antlr.Runtime
{
  public class UnbufferedTokenStream : 
    LookaheadStream<IToken>,
    ITokenStream,
    IIntStream,
    ITokenStreamInformation
  {
    [CLSCompliant(false)]
    protected ITokenSource tokenSource;
    protected int tokenIndex;
    protected int channel;
    private readonly ListStack<IToken> _realTokens;

    public UnbufferedTokenStream(ITokenSource tokenSource)
    {
      ListStack<IToken> listStack = new ListStack<IToken>();
      listStack.Add((IToken) null);
      this._realTokens = listStack;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.tokenSource = tokenSource;
    }

    public ITokenSource TokenSource => this.tokenSource;

    public string SourceName => this.TokenSource.SourceName;

    public IToken LastToken => this.LB(1);

    public IToken LastRealToken => this._realTokens.Peek();

    public int MaxLookBehind => 1;

    public override int Mark()
    {
      this._realTokens.Push(this._realTokens.Peek());
      return base.Mark();
    }

    public override void Release(int marker)
    {
      base.Release(marker);
      this._realTokens.Pop();
    }

    public override void Clear()
    {
      this._realTokens.Clear();
      this._realTokens.Push((IToken) null);
    }

    public override void Consume()
    {
      base.Consume();
      if (this.PreviousElement == null || this.PreviousElement.Line <= 0)
        return;
      this._realTokens[this._realTokens.Count - 1] = this.PreviousElement;
    }

    public override IToken NextElement()
    {
      IToken token = this.tokenSource.NextToken();
      token.TokenIndex = this.tokenIndex++;
      return token;
    }

    public override bool IsEndOfFile(IToken o) => o.Type == -1;

    public IToken Get(int i)
    {
      throw new NotSupportedException("Absolute token indexes are meaningless in an unbuffered stream");
    }

    public int LA(int i) => this.LT(i).Type;

    public string ToString(int start, int stop) => "n/a";

    public string ToString(IToken start, IToken stop) => "n/a";
  }
}
