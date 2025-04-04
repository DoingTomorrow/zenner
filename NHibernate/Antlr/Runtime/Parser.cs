// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Parser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime
{
  internal class Parser : BaseRecognizer
  {
    protected internal ITokenStream input;

    public Parser(ITokenStream input) => this.TokenStream = input;

    public Parser(ITokenStream input, RecognizerSharedState state)
      : base(state)
    {
      this.TokenStream = input;
    }

    public override void Reset()
    {
      base.Reset();
      if (this.input == null)
        return;
      this.input.Seek(0);
    }

    protected override object GetCurrentInputSymbol(IIntStream input)
    {
      return (object) ((ITokenStream) input).LT(1);
    }

    protected override object GetMissingSymbol(
      IIntStream input,
      RecognitionException e,
      int expectedTokenType,
      BitSet follow)
    {
      string text = expectedTokenType != Token.EOF ? "<missing " + this.TokenNames[expectedTokenType] + ">" : "<missing EOF>";
      CommonToken missingSymbol = new CommonToken(expectedTokenType, text);
      IToken token = ((ITokenStream) input).LT(1);
      if (token.Type == Token.EOF)
        token = ((ITokenStream) input).LT(-1);
      missingSymbol.line = token.Line;
      missingSymbol.CharPositionInLine = token.CharPositionInLine;
      missingSymbol.Channel = 0;
      return (object) missingSymbol;
    }

    public virtual ITokenStream TokenStream
    {
      get => this.input;
      set
      {
        this.input = (ITokenStream) null;
        this.Reset();
        this.input = value;
      }
    }

    public override string SourceName => this.input.SourceName;

    public override IIntStream Input => (IIntStream) this.input;

    public virtual void TraceIn(string ruleName, int ruleIndex)
    {
      this.TraceIn(ruleName, ruleIndex, (object) this.input.LT(1));
    }

    public virtual void TraceOut(string ruleName, int ruleIndex)
    {
      this.TraceOut(ruleName, ruleIndex, (object) this.input.LT(1));
    }
  }
}
