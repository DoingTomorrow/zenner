// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Parser
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System.Diagnostics;

#nullable disable
namespace Antlr.Runtime
{
  public class Parser : BaseRecognizer
  {
    public ITokenStream input;

    public Parser(ITokenStream input) => this.TokenStream = input;

    public Parser(ITokenStream input, RecognizerSharedState state)
      : base(state)
    {
      this.input = input;
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
      string text = expectedTokenType != -1 ? "<missing " + this.TokenNames[expectedTokenType] + ">" : "<missing EOF>";
      CommonToken missingSymbol = new CommonToken(expectedTokenType, text);
      IToken token = ((ITokenStream) input).LT(1);
      if (token.Type == -1)
        token = ((ITokenStream) input).LT(-1);
      missingSymbol.Line = token.Line;
      missingSymbol.CharPositionInLine = token.CharPositionInLine;
      missingSymbol.Channel = 0;
      missingSymbol.InputStream = token.InputStream;
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

    [Conditional("ANTLR_TRACE")]
    public virtual void TraceIn(string ruleName, int ruleIndex)
    {
      this.TraceIn(ruleName, ruleIndex, (object) this.input.LT(1));
    }

    [Conditional("ANTLR_TRACE")]
    public virtual void TraceOut(string ruleName, int ruleIndex)
    {
      this.TraceOut(ruleName, ruleIndex, (object) this.input.LT(1));
    }
  }
}
