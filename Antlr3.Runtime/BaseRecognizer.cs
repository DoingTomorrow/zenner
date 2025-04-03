// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.BaseRecognizer
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using Antlr.Runtime.Debug;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Antlr.Runtime
{
  public abstract class BaseRecognizer
  {
    public const int MemoRuleFailed = -2;
    public const int MemoRuleUnknown = -1;
    public const int InitialFollowStackSize = 100;
    public const int DefaultTokenChannel = 0;
    public const int Hidden = 99;
    public const string NextTokenRuleName = "nextToken";
    protected internal RecognizerSharedState state;

    public BaseRecognizer()
      : this(new RecognizerSharedState())
    {
    }

    public BaseRecognizer(RecognizerSharedState state)
    {
      if (state == null)
        state = new RecognizerSharedState();
      this.state = state;
      this.InitDFAs();
    }

    public TextWriter TraceDestination { get; set; }

    public virtual void SetState(RecognizerSharedState value) => this.state = value;

    protected virtual void InitDFAs()
    {
    }

    public virtual void Reset()
    {
      if (this.state == null)
        return;
      this.state._fsp = -1;
      this.state.errorRecovery = false;
      this.state.lastErrorIndex = -1;
      this.state.failed = false;
      this.state.syntaxErrors = 0;
      this.state.backtracking = 0;
      for (int index = 0; this.state.ruleMemo != null && index < this.state.ruleMemo.Length; ++index)
        this.state.ruleMemo[index] = (IDictionary<int, int>) null;
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

    public virtual bool MismatchIsUnwantedToken(IIntStream input, int ttype)
    {
      return input.LA(2) == ttype;
    }

    public virtual bool MismatchIsMissingToken(IIntStream input, BitSet follow)
    {
      if (follow == null)
        return false;
      if (follow.Member(1))
      {
        BitSet sensitiveRuleFollow = this.ComputeContextSensitiveRuleFOLLOW();
        follow = follow.Or(sensitiveRuleFollow);
        if (this.state._fsp >= 0)
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
          string str1 = unwantedTokenException.Expecting != -1 ? tokenNames[unwantedTokenException.Expecting] : "EndOfFile";
          errorMessage = "extraneous input " + this.GetTokenErrorDisplay(unwantedTokenException.UnexpectedToken) + " expecting " + str1;
          break;
        case MissingTokenException _:
          MissingTokenException missingTokenException = (MissingTokenException) e;
          errorMessage = "missing " + (missingTokenException.Expecting != -1 ? tokenNames[missingTokenException.Expecting] : "EndOfFile") + " at " + this.GetTokenErrorDisplay(e.Token);
          break;
        case MismatchedTokenException _:
          MismatchedTokenException mismatchedTokenException = (MismatchedTokenException) e;
          string str2 = mismatchedTokenException.Expecting != -1 ? tokenNames[mismatchedTokenException.Expecting] : "EndOfFile";
          errorMessage = "mismatched input " + this.GetTokenErrorDisplay(e.Token) + " expecting " + str2;
          break;
        case MismatchedTreeNodeException _:
          MismatchedTreeNodeException treeNodeException = (MismatchedTreeNodeException) e;
          string str3 = treeNodeException.Expecting != -1 ? tokenNames[treeNodeException.Expecting] : "EndOfFile";
          errorMessage = "mismatched tree node: " + (treeNodeException.Node != null ? treeNodeException.Node.ToString() ?? string.Empty : string.Empty) + " expecting " + str3;
          break;
        case NoViableAltException _:
          errorMessage = "no viable alternative at input " + this.GetTokenErrorDisplay(e.Token);
          break;
        case EarlyExitException _:
          errorMessage = "required (...)+ loop did not match anything at input " + this.GetTokenErrorDisplay(e.Token);
          break;
        case MismatchedSetException _:
          MismatchedSetException mismatchedSetException = (MismatchedSetException) e;
          errorMessage = "mismatched input " + this.GetTokenErrorDisplay(e.Token) + " expecting set " + (object) mismatchedSetException.Expecting;
          break;
        case MismatchedNotSetException _:
          MismatchedNotSetException mismatchedNotSetException = (MismatchedNotSetException) e;
          errorMessage = "mismatched input " + this.GetTokenErrorDisplay(e.Token) + " expecting set " + (object) mismatchedNotSetException.Expecting;
          break;
        case FailedPredicateException _:
          FailedPredicateException predicateException = (FailedPredicateException) e;
          errorMessage = "rule " + predicateException.RuleName + " failed predicate: {" + predicateException.PredicateText + "}?";
          break;
      }
      return errorMessage;
    }

    public virtual int NumberOfSyntaxErrors => this.state.syntaxErrors;

    public virtual string GetErrorHeader(RecognitionException e)
    {
      string str = this.SourceName ?? string.Empty;
      if (str.Length > 0)
        str += (string) (object) ' ';
      return string.Format("{0}line {1}:{2}", (object) str, (object) e.Line, (object) (e.CharPositionInLine + 1));
    }

    public virtual string GetTokenErrorDisplay(IToken t)
    {
      return "'" + Regex.Replace(Regex.Replace(Regex.Replace(t.Text ?? (t.Type != -1 ? "<" + (object) t.Type + ">" : "<EOF>"), "\n", "\\\\n"), "\r", "\\\\r"), "\t", "\\\\t") + "'";
    }

    public virtual void EmitErrorMessage(string msg)
    {
      if (this.TraceDestination == null)
        return;
      this.TraceDestination.WriteLine(msg);
    }

    public virtual void Recover(IIntStream input, RecognitionException re)
    {
      if (this.state.lastErrorIndex == input.Index)
        input.Consume();
      this.state.lastErrorIndex = input.Index;
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

    protected virtual BitSet ComputeErrorRecoverySet() => this.CombineFollows(false);

    protected virtual BitSet ComputeContextSensitiveRuleFOLLOW() => this.CombineFollows(true);

    protected virtual BitSet CombineFollows(bool exact)
    {
      int fsp = this.state._fsp;
      BitSet bitSet = new BitSet();
      for (int index = fsp; index >= 0; --index)
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

    protected virtual object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow)
    {
      RecognitionException e1 = (RecognitionException) null;
      if (this.MismatchIsUnwantedToken(input, ttype))
      {
        RecognitionException e2 = (RecognitionException) new UnwantedTokenException(ttype, input, (IList<string>) this.TokenNames);
        this.BeginResync();
        input.Consume();
        this.EndResync();
        this.ReportError(e2);
        object currentInputSymbol = this.GetCurrentInputSymbol(input);
        input.Consume();
        return currentInputSymbol;
      }
      if (!this.MismatchIsMissingToken(input, follow))
        throw (RecognitionException) new MismatchedTokenException(ttype, input, (IList<string>) this.TokenNames);
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

    protected virtual object GetCurrentInputSymbol(IIntStream input) => (object) null;

    protected virtual object GetMissingSymbol(
      IIntStream input,
      RecognitionException e,
      int expectedTokenType,
      BitSet follow)
    {
      return (object) null;
    }

    public virtual void ConsumeUntil(IIntStream input, int tokenType)
    {
      for (int index = input.LA(1); index != -1 && index != tokenType; index = input.LA(1))
        input.Consume();
    }

    public virtual void ConsumeUntil(IIntStream input, BitSet set)
    {
      for (int el = input.LA(1); el != -1 && !set.Member(el); el = input.LA(1))
        input.Consume();
    }

    protected void PushFollow(BitSet fset)
    {
      if (this.state._fsp + 1 >= this.state.following.Length)
        Array.Resize<BitSet>(ref this.state.following, this.state.following.Length * 2);
      this.state.following[++this.state._fsp] = fset;
    }

    protected void PopFollow() => --this.state._fsp;

    public virtual IList<string> GetRuleInvocationStack()
    {
      return BaseRecognizer.GetRuleInvocationStack(new StackTrace(true));
    }

    public static IList<string> GetRuleInvocationStack(StackTrace trace)
    {
      if (trace == null)
        throw new ArgumentNullException(nameof (trace));
      List<string> ruleInvocationStack = new List<string>();
      StackFrame[] stackFrameArray = trace.GetFrames() ?? new StackFrame[0];
      for (int index = stackFrameArray.Length - 1; index >= 0; --index)
      {
        GrammarRuleAttribute[] customAttributes = (GrammarRuleAttribute[]) stackFrameArray[index].GetMethod().GetCustomAttributes(typeof (GrammarRuleAttribute), true);
        if (customAttributes != null && customAttributes.Length > 0)
          ruleInvocationStack.Add(customAttributes[0].Name);
      }
      return (IList<string>) ruleInvocationStack;
    }

    public virtual int BacktrackingLevel
    {
      get => this.state.backtracking;
      set => this.state.backtracking = value;
    }

    public virtual bool Failed => this.state.failed;

    public virtual string[] TokenNames => (string[]) null;

    public virtual string GrammarFileName => (string) null;

    public abstract string SourceName { get; }

    public virtual List<string> ToStrings(ICollection<IToken> tokens)
    {
      if (tokens == null)
        return (List<string>) null;
      List<string> strings = new List<string>(tokens.Count);
      foreach (IToken token in (IEnumerable<IToken>) tokens)
        strings.Add(token.Text);
      return strings;
    }

    public virtual int GetRuleMemoization(int ruleIndex, int ruleStartIndex)
    {
      if (this.state.ruleMemo[ruleIndex] == null)
        this.state.ruleMemo[ruleIndex] = (IDictionary<int, int>) new Dictionary<int, int>();
      int num;
      return !this.state.ruleMemo[ruleIndex].TryGetValue(ruleStartIndex, out num) ? -1 : num;
    }

    public virtual bool AlreadyParsedRule(IIntStream input, int ruleIndex)
    {
      int ruleMemoization = this.GetRuleMemoization(ruleIndex, input.Index);
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
      int num = this.state.failed ? -2 : input.Index - 1;
      if (this.state.ruleMemo == null && this.TraceDestination != null)
        this.TraceDestination.WriteLine("!!!!!!!!! memo array is null for " + this.GrammarFileName);
      if (ruleIndex >= this.state.ruleMemo.Length && this.TraceDestination != null)
        this.TraceDestination.WriteLine("!!!!!!!!! memo size is " + (object) this.state.ruleMemo.Length + ", but rule index is " + (object) ruleIndex);
      if (this.state.ruleMemo[ruleIndex] == null)
        return;
      this.state.ruleMemo[ruleIndex][ruleStartIndex] = num;
    }

    public virtual int GetRuleMemoizationCacheSize()
    {
      int memoizationCacheSize = 0;
      for (int index = 0; this.state.ruleMemo != null && index < this.state.ruleMemo.Length; ++index)
      {
        IDictionary<int, int> dictionary = this.state.ruleMemo[index];
        if (dictionary != null)
          memoizationCacheSize += dictionary.Count;
      }
      return memoizationCacheSize;
    }

    public virtual void TraceIn(string ruleName, int ruleIndex, object inputSymbol)
    {
      if (this.TraceDestination == null)
        return;
      this.TraceDestination.Write("enter " + ruleName + " " + inputSymbol);
      if (this.state.backtracking > 0)
        this.TraceDestination.Write(" backtracking=" + (object) this.state.backtracking);
      this.TraceDestination.WriteLine();
    }

    public virtual void TraceOut(string ruleName, int ruleIndex, object inputSymbol)
    {
      if (this.TraceDestination == null)
        return;
      this.TraceDestination.Write("exit " + ruleName + " " + inputSymbol);
      if (this.state.backtracking > 0)
      {
        this.TraceDestination.Write(" backtracking=" + (object) this.state.backtracking);
        if (this.state.failed)
          this.TraceDestination.Write(" failed");
        else
          this.TraceDestination.Write(" succeeded");
      }
      this.TraceDestination.WriteLine();
    }

    public virtual IDebugEventListener DebugListener => (IDebugEventListener) null;

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugEnterRule(string grammarFileName, string ruleName)
    {
      this.DebugListener?.EnterRule(grammarFileName, ruleName);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugExitRule(string grammarFileName, string ruleName)
    {
      this.DebugListener?.ExitRule(grammarFileName, ruleName);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugEnterSubRule(int decisionNumber)
    {
      this.DebugListener?.EnterSubRule(decisionNumber);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugExitSubRule(int decisionNumber)
    {
      this.DebugListener?.ExitSubRule(decisionNumber);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugEnterAlt(int alt) => this.DebugListener?.EnterAlt(alt);

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugEnterDecision(int decisionNumber, bool couldBacktrack)
    {
      this.DebugListener?.EnterDecision(decisionNumber, couldBacktrack);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugExitDecision(int decisionNumber)
    {
      this.DebugListener?.ExitDecision(decisionNumber);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugLocation(int line, int charPositionInLine)
    {
      this.DebugListener?.Location(line, charPositionInLine);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugSemanticPredicate(bool result, string predicate)
    {
      this.DebugListener?.SemanticPredicate(result, predicate);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugBeginBacktrack(int level)
    {
      this.DebugListener?.BeginBacktrack(level);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugEndBacktrack(int level, bool successful)
    {
      this.DebugListener?.EndBacktrack(level, successful);
    }

    [Conditional("ANTLR_DEBUG")]
    protected virtual void DebugRecognitionException(RecognitionException ex)
    {
      this.DebugListener?.RecognitionException(ex);
    }
  }
}
