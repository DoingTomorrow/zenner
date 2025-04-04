// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal class TreeParser : BaseRecognizer
  {
    public const int DOWN = 2;
    public const int UP = 3;
    private static readonly string dotdot = ".*[^.]\\.\\.[^.].*";
    private static readonly string doubleEtc = ".*\\.\\.\\.\\s+\\.\\.\\..*";
    private static readonly string spaces = "\\s+";
    private static readonly Regex dotdotPattern = new Regex(TreeParser.dotdot, RegexOptions.Compiled);
    private static readonly Regex doubleEtcPattern = new Regex(TreeParser.doubleEtc, RegexOptions.Compiled);
    private static readonly Regex spacesPattern = new Regex(TreeParser.spaces, RegexOptions.Compiled);
    protected internal ITreeNodeStream input;

    public TreeParser(ITreeNodeStream input) => this.TreeNodeStream = input;

    public TreeParser(ITreeNodeStream input, RecognizerSharedState state)
      : base(state)
    {
      this.TreeNodeStream = input;
    }

    public virtual ITreeNodeStream TreeNodeStream
    {
      get => this.input;
      set => this.input = value;
    }

    public override string SourceName => this.input.SourceName;

    protected override object GetCurrentInputSymbol(IIntStream input)
    {
      return ((ITreeNodeStream) input).LT(1);
    }

    protected override object GetMissingSymbol(
      IIntStream input,
      RecognitionException e,
      int expectedTokenType,
      BitSet follow)
    {
      string text = "<missing " + this.TokenNames[expectedTokenType] + ">";
      return (object) new CommonTree((IToken) new CommonToken(expectedTokenType, text));
    }

    public override void Reset()
    {
      base.Reset();
      if (this.input == null)
        return;
      this.input.Seek(0);
    }

    public override void MatchAny(IIntStream ignore)
    {
      this.state.errorRecovery = false;
      this.state.failed = false;
      object t = this.input.LT(1);
      if (this.input.TreeAdaptor.GetChildCount(t) == 0)
      {
        this.input.Consume();
      }
      else
      {
        int num = 0;
        int nodeType = this.input.TreeAdaptor.GetNodeType(t);
        while (nodeType != Token.EOF && (nodeType != 3 || num != 0))
        {
          this.input.Consume();
          nodeType = this.input.TreeAdaptor.GetNodeType(this.input.LT(1));
          switch (nodeType)
          {
            case 2:
              ++num;
              continue;
            case 3:
              --num;
              continue;
            default:
              continue;
          }
        }
        this.input.Consume();
      }
    }

    public override IIntStream Input => (IIntStream) this.input;

    protected internal override object RecoverFromMismatchedToken(
      IIntStream input,
      int ttype,
      BitSet follow)
    {
      throw new MismatchedTreeNodeException(ttype, (ITreeNodeStream) input);
    }

    public override string GetErrorHeader(RecognitionException e)
    {
      return this.GrammarFileName + ": node from " + (!e.approximateLineInfo ? (object) string.Empty : (object) "after ") + "line " + (object) e.Line + ":" + (object) e.CharPositionInLine;
    }

    public override string GetErrorMessage(RecognitionException e, string[] tokenNames)
    {
      if (this is TreeParser)
      {
        ITreeAdaptor treeAdaptor = ((ITreeNodeStream) e.Input).TreeAdaptor;
        e.Token = treeAdaptor.GetToken(e.Node);
        if (e.Token == null)
          e.Token = (IToken) new CommonToken(treeAdaptor.GetNodeType(e.Node), treeAdaptor.GetNodeText(e.Node));
      }
      return base.GetErrorMessage(e, tokenNames);
    }

    public virtual void TraceIn(string ruleName, int ruleIndex)
    {
      this.TraceIn(ruleName, ruleIndex, this.input.LT(1));
    }

    public virtual void TraceOut(string ruleName, int ruleIndex)
    {
      this.TraceOut(ruleName, ruleIndex, this.input.LT(1));
    }
  }
}
