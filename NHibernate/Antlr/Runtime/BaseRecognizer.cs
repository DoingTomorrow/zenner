// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.BaseRecognizer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace Antlr.Runtime
{
  internal abstract class BaseRecognizer
  {
    public const int MEMO_RULE_FAILED = -2;
    public const int MEMO_RULE_UNKNOWN = -1;
    public const int INITIAL_FOLLOW_STACK_SIZE = 100;
    public const int DEFAULT_TOKEN_CHANNEL = 0;
    public const int HIDDEN = 99;
    public static readonly string NEXT_TOKEN_RULE_NAME = "nextToken";
    protected internal RecognizerSharedState state;

    public BaseRecognizer() => this.state = new RecognizerSharedState();

    public BaseRecognizer(RecognizerSharedState state)
    {
      if (state == null)
        state = new RecognizerSharedState();
      this.state = state;
    }

    public virtual void BeginBacktrack(int level)
    {
    }

    public virtual void EndBacktrack(int level, bool successful)
    {
    }

    public abstract IIntStream Input { get; }

    public int BacktrackingLevel
    {
      get => this.state.backtracking;
      set => this.state.backtracking = value;
    }

    public bool Failed() => this.state.failed;

    public virtual void Reset()
    {
      if (this.state == null)
        return;
      this.state.followingStackPointer = -1;
      this.state.errorRecovery = false;
      this.state.lastErrorIndex = -1;
      this.state.failed = false;
      this.state.syntaxErrors = 0;
      this.state.backtracking = 0;
      for (int index = 0; this.state.ruleMemo != null && index < this.state.ruleMemo.Length; ++index)
        this.state.ruleMemo[index] = (IDictionary) null;
    }

    public virtual object Match(IIntStream input, int ttype, BitSet follow)
    {
      object currentInputSymbol = this.GetCurrentInputSymbol(input);
      if (input.LA(1) == ttype)
      {
        input.Consume();
        this.state.errorRecovery = false;
        this.state.failed = false;
        return currentInputSymbol;
      }
      if (this.state.backtracking <= 0)
        return this.RecoverFromMismatchedToken(input, ttype, follow);
      this.state.failed = true;
      return currentInputSymbol;
    }

    public virtual void MatchAny(IIntStream input)
    {
      this.state.errorRecovery = false;
      this.state.failed = false;
      input.Consume();
    }

    public bool MismatchIsUnwantedToken(IIntStream input, int ttype) => input.LA(2) == ttype;

    public bool MismatchIsMissingToken(IIntStream input, BitSet follow)
    {
      if (follow == null)
        return false;
      if (follow.Member(1))
      {
        BitSet sensitiveRuleFollow = this.ComputeContextSensitiveRuleFOLLOW();
        follow = follow.Or(sensitiveRuleFollow);
        if (this.state.followingStackPointer >= 0)
          follow.Remove(1);
      }
      return follow.Member(input.LA(1)) || follow.Member(1);
    }

    public virtual void ReportError(RecognitionException e)
    {
      if (this.state.errorRecovery)
        return;
      ++this.state.syntaxErrors;
      this.state.errorRecovery = true;
      this.DisplayRecognitionError(this.TokenNames, e);
    }

    public virtual void DisplayRecognitionError(string[] tokenNames, RecognitionException e)
    {
      this.EmitErrorMessage(this.GetErrorHeader(e) + " " + this.GetErrorMessage(e, tokenNames));
    }

    public virtual string GetErrorMessage(RecognitionException e, string[] tokenNames)
    {
      string errorMessage = e.Message;
      switch (e)
      {
        case UnwantedTokenException _:
          UnwantedTokenException unwantedTokenException = (UnwantedTokenException) e;
          string str1 = unwantedTokenException.Expecting != Token.EOF ? tokenNames[unwantedTokenException.Expecting] : "EOF";
          errorMessage = "extraneous input " + this.GetTokenErrorDisplay(unwantedTokenException.UnexpectedToken) + " expecting " + str1;
          break;
        case MissingTokenException _:
          MissingTokenException missingTokenException = (MissingTokenException) e;
          errorMessage = "missing " + (missingTokenException.Expecting != Token.EOF ? tokenNames[missingTokenException.Expecting] : "EOF") + " at " + this.GetTokenErrorDisplay(e.Token);
          break;
        case MismatchedTokenException _:
          MismatchedTokenException mismatchedTokenException = (MismatchedTokenException) e;
          string str2 = mismatchedTokenException.Expecting != Token.EOF ? tokenNames[mismatchedTokenException.Expecting] : "EOF";
          errorMessage = "mismatched input " + this.GetTokenErrorDisplay(e.Token) + " expecting " + str2;
          break;
        case MismatchedTreeNodeException _:
          MismatchedTreeNodeException treeNodeException = (MismatchedTreeNodeException) e;
          string str3 = treeNodeException.expecting != Token.EOF ? tokenNames[treeNodeException.expecting] : "EOF";
          errorMessage = "mismatched tree node: " + (treeNodeException.Node == null || treeNodeException.Node.ToString() == null ? (object) string.Empty : treeNodeException.Node) + " expecting " + str3;
          break;
        case NoViableAltException _:
          errorMessage = "no viable alternative at input " + this.GetTokenErrorDisplay(e.Token);
          break;
        case EarlyExitException _:
          errorMessage = "required (...)+ loop did not match anything at input " + this.GetTokenErrorDisplay(e.Token);
          break;
        case MismatchedSetException _:
          MismatchedSetException mismatchedSetException = (MismatchedSetException) e;
          errorMessage = "mismatched input " + this.GetTokenErrorDisplay(e.Token) + " expecting set " + (object) mismatchedSetException.expecting;
          break;
        case MismatchedNotSetException _:
          MismatchedNotSetException mismatchedNotSetException = (MismatchedNotSetException) e;
          errorMessage = "mismatched input " + this.GetTokenErrorDisplay(e.Token) + " expecting set " + (object) mismatchedNotSetException.expecting;
          break;
        case FailedPredicateException _:
          FailedPredicateException predicateException = (FailedPredicateException) e;
          errorMessage = "rule " + predicateException.ruleName + " failed predicate: {" + predicateException.predicateText + "}?";
          break;
      }
      return errorMessage;
    }

    public int NumberOfSyntaxErrors => this.state.syntaxErrors;

    public virtual string GetErrorHeader(RecognitionException e)
    {
      return "line " + (object) e.Line + ":" + (object) e.CharPositionInLine;
    }

    public virtual string GetTokenErrorDisplay(IToken t)
    {
      return "'" + (t.Text ?? (t.Type != Token.EOF ? "<" + (object) t.Type + ">" : "<EOF>")).Replace("\n", "\\\\n").Replace("\r", "\\\\r").Replace("\t", "\\\\t") + "'";
    }

    public virtual void EmitErrorMessage(string msg) => Console.Error.WriteLine(msg);

    public virtual void Recover(IIntStream input, RecognitionException re)
    {
      if (this.state.lastErrorIndex == input.Index())
        input.Consume();
      this.state.lastErrorIndex = input.Index();
      BitSet errorRecoverySet = this.ComputeErrorRecoverySet();
      this.BeginResync();
      this.ConsumeUntil(input, errorRecoverySet);
      this.EndResync();
    }

    public virtual void BeginResync()
    {
    }

    public virtual void EndResync()
    {
    }

    protected internal virtual object RecoverFromMismatchedToken(
      IIntStream input,
      int ttype,
      BitSet follow)
    {
      RecognitionException e1 = (RecognitionException) null;
      if (this.MismatchIsUnwantedToken(input, ttype))
      {
        RecognitionException e2 = (RecognitionException) new UnwantedTokenException(ttype, input);
        this.BeginResync();
        input.Consume();
        this.EndResync();
        this.ReportError(e2);
        object currentInputSymbol = this.GetCurrentInputSymbol(input);
        input.Consume();
        return currentInputSymbol;
      }
      if (!this.MismatchIsMissingToken(input, follow))
        throw (RecognitionException) new MismatchedTokenException(ttype, input);
      object missingSymbol = this.GetMissingSymbol(input, e1, ttype, follow);
      this.ReportError((RecognitionException) new MissingTokenException(ttype, input, missingSymbol));
      return missingSymbol;
    }

    public virtual object RecoverFromMismatchedSet(
      IIntStream input,
      RecognitionException e,
      BitSet follow)
    {
      if (!this.MismatchIsMissingToken(input, follow))
        throw e;
      this.ReportError(e);
      return this.GetMissingSymbol(input, e, 0, follow);
    }

    public virtual void ConsumeUntil(IIntStream input, int tokenType)
    {
      for (int index = input.LA(1); index != Token.EOF && index != tokenType; index = input.LA(1))
        input.Consume();
    }

    public virtual void ConsumeUntil(IIntStream input, BitSet set)
    {
      for (int el = input.LA(1); el != Token.EOF && !set.Member(el); el = input.LA(1))
        input.Consume();
    }

    public virtual IList GetRuleInvocationStack()
    {
      return BaseRecognizer.GetRuleInvocationStack(new Exception(), this.GetType().FullName);
    }

    public static IList GetRuleInvocationStack(Exception e, string recognizerClassName)
    {
      IList ruleInvocationStack = (IList) new ArrayList();
      StackTrace stackTrace = new StackTrace(e);
      for (int index = stackTrace.FrameCount - 1; index >= 0; --index)
      {
        StackFrame frame = stackTrace.GetFrame(index);
        if (!frame.GetMethod().DeclaringType.FullName.StartsWith("Antlr.Runtime.") && !frame.GetMethod().Name.Equals(BaseRecognizer.NEXT_TOKEN_RULE_NAME) && frame.GetMethod().DeclaringType.FullName.Equals(recognizerClassName))
          ruleInvocationStack.Add((object) frame.GetMethod().Name);
      }
      return ruleInvocationStack;
    }

    public virtual string GrammarFileName => (string) null;

    public abstract string SourceName { get; }

    public virtual IList ToStrings(IList tokens)
    {
      if (tokens == null)
        return (IList) null;
      IList strings = (IList) new ArrayList(tokens.Count);
      for (int index = 0; index < tokens.Count; ++index)
        strings.Add((object) ((IToken) tokens[index]).Text);
      return strings;
    }

    public virtual int GetRuleMemoization(int ruleIndex, int ruleStartIndex)
    {
      if (this.state.ruleMemo[ruleIndex] == null)
        this.state.ruleMemo[ruleIndex] = (IDictionary) new Hashtable();
      object obj = this.state.ruleMemo[ruleIndex][(object) ruleStartIndex];
      return obj == null ? -1 : (int) obj;
    }

    public virtual bool AlreadyParsedRule(IIntStream input, int ruleIndex)
    {
      int ruleMemoization = this.GetRuleMemoization(ruleIndex, input.Index());
      switch (ruleMemoization)
      {
        case -2:
          this.state.failed = true;
          break;
        case -1:
          return false;
        default:
          input.Seek(ruleMemoization + 1);
          break;
      }
      return true;
    }

    public virtual void Memoize(IIntStream input, int ruleIndex, int ruleStartIndex)
    {
      int num = !this.state.failed ? input.Index() - 1 : -2;
      if (this.state.ruleMemo[ruleIndex] == null)
        return;
      this.state.ruleMemo[ruleIndex][(object) ruleStartIndex] = (object) num;
    }

    public int GetRuleMemoizationCacheSize()
    {
      int memoizationCacheSize = 0;
      for (int index = 0; this.state.ruleMemo != null && index < this.state.ruleMemo.Length; ++index)
      {
        IDictionary dictionary = this.state.ruleMemo[index];
        if (dictionary != null)
          memoizationCacheSize += dictionary.Count;
      }
      return memoizationCacheSize;
    }

    public virtual void TraceIn(string ruleName, int ruleIndex, object inputSymbol)
    {
      Console.Out.Write("enter " + ruleName + " " + inputSymbol);
      if (this.state.backtracking > 0)
        Console.Out.Write(" backtracking=" + (object) this.state.backtracking);
      Console.Out.WriteLine();
    }

    public virtual void TraceOut(string ruleName, int ruleIndex, object inputSymbol)
    {
      Console.Out.Write("exit " + ruleName + " " + inputSymbol);
      if (this.state.backtracking > 0)
      {
        Console.Out.Write(" backtracking=" + (object) this.state.backtracking);
        if (this.state.failed)
          Console.Out.WriteLine(" failed" + (object) this.state.failed);
        else
          Console.Out.WriteLine(" succeeded" + (object) this.state.failed);
      }
      Console.Out.WriteLine();
    }

    public virtual string[] TokenNames => (string[]) null;

    protected internal virtual BitSet ComputeErrorRecoverySet() => this.CombineFollows(false);

    protected internal virtual BitSet ComputeContextSensitiveRuleFOLLOW()
    {
      return this.CombineFollows(true);
    }

    protected internal virtual BitSet CombineFollows(bool exact)
    {
      int followingStackPointer = this.state.followingStackPointer;
      BitSet bitSet = new BitSet();
      for (int index = followingStackPointer; index >= 0; --index)
      {
        BitSet a = this.state.following[index];
        bitSet.OrInPlace(a);
        if (exact)
        {
          if (a.Member(1))
          {
            if (index > 0)
              bitSet.Remove(1);
          }
          else
            break;
        }
      }
      return bitSet;
    }

    protected virtual object GetCurrentInputSymbol(IIntStream input) => (object) null;

    protected virtual object GetMissingSymbol(
      IIntStream input,
      RecognitionException e,
      int expectedTokenType,
      BitSet follow)
    {
      return (object) null;
    }

    protected void PushFollow(BitSet fset)
    {
      if (this.state.followingStackPointer + 1 >= this.state.following.Length)
      {
        BitSet[] destinationArray = new BitSet[this.state.following.Length * 2];
        Array.Copy((Array) this.state.following, 0, (Array) destinationArray, 0, this.state.following.Length);
        this.state.following = destinationArray;
      }
      this.state.following[++this.state.followingStackPointer] = fset;
    }
  }
}
