// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Lexer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  internal abstract class Lexer : BaseRecognizer, ITokenSource
  {
    private const int TOKEN_dot_EOF = -1;
    protected internal ICharStream input;

    public Lexer()
    {
    }

    public Lexer(ICharStream input) => this.input = input;

    public Lexer(ICharStream input, RecognizerSharedState state)
      : base(state)
    {
      this.input = input;
    }

    public virtual ICharStream CharStream
    {
      get => this.input;
      set
      {
        this.input = (ICharStream) null;
        this.Reset();
        this.input = value;
      }
    }

    public override string SourceName => this.input.SourceName;

    public override IIntStream Input => (IIntStream) this.input;

    public virtual int Line => this.input.Line;

    public virtual int CharPositionInLine => this.input.CharPositionInLine;

    public virtual int CharIndex => this.input.Index();

    public virtual string Text
    {
      get
      {
        return this.state.text != null ? this.state.text : this.input.Substring(this.state.tokenStartCharIndex, this.CharIndex - 1);
      }
      set => this.state.text = value;
    }

    public override void Reset()
    {
      base.Reset();
      if (this.input != null)
        this.input.Seek(0);
      if (this.state == null)
        return;
      this.state.token = (IToken) null;
      this.state.type = 0;
      this.state.channel = 0;
      this.state.tokenStartCharIndex = -1;
      this.state.tokenStartCharPositionInLine = -1;
      this.state.tokenStartLine = -1;
      this.state.text = (string) null;
    }

    public virtual IToken NextToken()
    {
      while (true)
      {
        this.state.token = (IToken) null;
        this.state.channel = 0;
        this.state.tokenStartCharIndex = this.input.Index();
        this.state.tokenStartCharPositionInLine = this.input.CharPositionInLine;
        this.state.tokenStartLine = this.input.Line;
        this.state.text = (string) null;
        if (this.input.LA(1) != -1)
        {
          try
          {
            this.mTokens();
            if (this.state.token == null)
              this.Emit();
            else if (this.state.token == Token.SKIP_TOKEN)
              continue;
            return this.state.token;
          }
          catch (NoViableAltException ex)
          {
            this.ReportError((RecognitionException) ex);
            this.Recover((RecognitionException) ex);
          }
          catch (RecognitionException ex)
          {
            this.ReportError(ex);
          }
        }
        else
          break;
      }
      return Token.EOF_TOKEN;
    }

    public void Skip() => this.state.token = Token.SKIP_TOKEN;

    public abstract void mTokens();

    public virtual void Emit(IToken token) => this.state.token = token;

    public virtual IToken Emit()
    {
      IToken token = (IToken) new CommonToken(this.input, this.state.type, this.state.channel, this.state.tokenStartCharIndex, this.CharIndex - 1);
      token.Line = this.state.tokenStartLine;
      token.Text = this.state.text;
      token.CharPositionInLine = this.state.tokenStartCharPositionInLine;
      this.Emit(token);
      return token;
    }

    public virtual void Match(string s)
    {
      int index = 0;
      while (index < s.Length)
      {
        if (this.input.LA(1) != (int) s[index])
        {
          if (this.state.backtracking > 0)
          {
            this.state.failed = true;
            break;
          }
          MismatchedTokenException re = new MismatchedTokenException((int) s[index], (IIntStream) this.input);
          this.Recover((RecognitionException) re);
          throw re;
        }
        ++index;
        this.input.Consume();
        this.state.failed = false;
      }
    }

    public virtual void MatchAny() => this.input.Consume();

    public virtual void Match(int c)
    {
      if (this.input.LA(1) != c)
      {
        if (this.state.backtracking > 0)
        {
          this.state.failed = true;
        }
        else
        {
          MismatchedTokenException re = new MismatchedTokenException(c, (IIntStream) this.input);
          this.Recover((RecognitionException) re);
          throw re;
        }
      }
      else
      {
        this.input.Consume();
        this.state.failed = false;
      }
    }

    public virtual void MatchRange(int a, int b)
    {
      if (this.input.LA(1) < a || this.input.LA(1) > b)
      {
        if (this.state.backtracking > 0)
        {
          this.state.failed = true;
        }
        else
        {
          MismatchedRangeException re = new MismatchedRangeException(a, b, (IIntStream) this.input);
          this.Recover((RecognitionException) re);
          throw re;
        }
      }
      else
      {
        this.input.Consume();
        this.state.failed = false;
      }
    }

    public virtual void Recover(RecognitionException re) => this.input.Consume();

    public override void ReportError(RecognitionException e)
    {
      this.DisplayRecognitionError(this.TokenNames, e);
    }

    public override string GetErrorMessage(RecognitionException e, string[] tokenNames)
    {
      string errorMessage;
      switch (e)
      {
        case MismatchedTokenException _:
          MismatchedTokenException mismatchedTokenException = (MismatchedTokenException) e;
          errorMessage = "mismatched character " + this.GetCharErrorDisplay(e.Char) + " expecting " + this.GetCharErrorDisplay(mismatchedTokenException.Expecting);
          break;
        case NoViableAltException _:
          errorMessage = "no viable alternative at character " + this.GetCharErrorDisplay(e.Char);
          break;
        case EarlyExitException _:
          errorMessage = "required (...)+ loop did not match anything at character " + this.GetCharErrorDisplay(e.Char);
          break;
        case MismatchedNotSetException _:
          MismatchedSetException mismatchedSetException1 = (MismatchedSetException) e;
          errorMessage = "mismatched character " + this.GetCharErrorDisplay(mismatchedSetException1.Char) + " expecting set " + (object) mismatchedSetException1.expecting;
          break;
        case MismatchedSetException _:
          MismatchedSetException mismatchedSetException2 = (MismatchedSetException) e;
          errorMessage = "mismatched character " + this.GetCharErrorDisplay(mismatchedSetException2.Char) + " expecting set " + (object) mismatchedSetException2.expecting;
          break;
        case MismatchedRangeException _:
          MismatchedRangeException mismatchedRangeException = (MismatchedRangeException) e;
          errorMessage = "mismatched character " + this.GetCharErrorDisplay(mismatchedRangeException.Char) + " expecting set " + this.GetCharErrorDisplay(mismatchedRangeException.A) + ".." + this.GetCharErrorDisplay(mismatchedRangeException.B);
          break;
        default:
          errorMessage = base.GetErrorMessage(e, tokenNames);
          break;
      }
      return errorMessage;
    }

    public string GetCharErrorDisplay(int c)
    {
      int num = c;
      string str;
      switch (num)
      {
        case 9:
          str = "\\t";
          break;
        case 10:
          str = "\\n";
          break;
        case 13:
          str = "\\r";
          break;
        default:
          str = num == -1 ? "<EOF>" : Convert.ToString((char) c);
          break;
      }
      return "'" + str + "'";
    }

    public virtual void TraceIn(string ruleName, int ruleIndex)
    {
      string inputSymbol = ((char) this.input.LT(1)).ToString() + " line=" + (object) this.Line + ":" + (object) this.CharPositionInLine;
      this.TraceIn(ruleName, ruleIndex, (object) inputSymbol);
    }

    public virtual void TraceOut(string ruleName, int ruleIndex)
    {
      string inputSymbol = ((char) this.input.LT(1)).ToString() + " line=" + (object) this.Line + ":" + (object) this.CharPositionInLine;
      this.TraceOut(ruleName, ruleIndex, (object) inputSymbol);
    }
  }
}
