// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.Profiler
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Misc;
using System;
using System.Collections;
using System.IO;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class Profiler : BlankDebugEventListener
  {
    public const string Version = "2";
    public const string RUNTIME_STATS_FILENAME = "runtime.stats";
    public const int NUM_RUNTIME_STATS = 29;
    public DebugParser parser;
    protected internal int ruleLevel;
    protected internal int decisionLevel;
    protected internal int maxLookaheadInCurrentDecision;
    protected internal CommonToken lastTokenConsumed;
    protected IList lookaheadStack = (IList) new ArrayList();
    public int numRuleInvocations;
    public int numGuessingRuleInvocations;
    public int maxRuleInvocationDepth;
    public int numFixedDecisions;
    public int numCyclicDecisions;
    public int numBacktrackDecisions;
    public int[] decisionMaxFixedLookaheads = new int[200];
    public int[] decisionMaxCyclicLookaheads = new int[200];
    public IList decisionMaxSynPredLookaheads = (IList) new ArrayList();
    public int numHiddenTokens;
    public int numCharsMatched;
    public int numHiddenCharsMatched;
    public int numSemanticPredicates;
    public int numSyntacticPredicates;
    protected int numberReportedErrors;
    public int numMemoizationCacheMisses;
    public int numMemoizationCacheHits;
    public int numMemoizationCacheEntries;

    public Profiler()
    {
    }

    public Profiler(DebugParser parser) => this.parser = parser;

    public override void EnterRule(string grammarFileName, string ruleName)
    {
      ++this.ruleLevel;
      ++this.numRuleInvocations;
      if (this.ruleLevel <= this.maxRuleInvocationDepth)
        return;
      this.maxRuleInvocationDepth = this.ruleLevel;
    }

    public void ExamineRuleMemoization(IIntStream input, int ruleIndex, string ruleName)
    {
      if (this.parser.GetRuleMemoization(ruleIndex, input.Index()) == -1)
      {
        ++this.numMemoizationCacheMisses;
        ++this.numGuessingRuleInvocations;
      }
      else
        ++this.numMemoizationCacheHits;
    }

    public void Memoize(IIntStream input, int ruleIndex, int ruleStartIndex, string ruleName)
    {
      ++this.numMemoizationCacheEntries;
    }

    public override void ExitRule(string grammarFileName, string ruleName) => --this.ruleLevel;

    public override void EnterDecision(int decisionNumber)
    {
      ++this.decisionLevel;
      this.lookaheadStack.Add((object) this.parser.TokenStream.Index());
    }

    public override void ExitDecision(int decisionNumber)
    {
      if (this.parser.isCyclicDecision)
        ++this.numCyclicDecisions;
      else
        ++this.numFixedDecisions;
      this.lookaheadStack.Remove((object) (this.lookaheadStack.Count - 1));
      --this.decisionLevel;
      if (this.parser.isCyclicDecision)
      {
        if (this.numCyclicDecisions >= this.decisionMaxCyclicLookaheads.Length)
        {
          int[] destinationArray = new int[this.decisionMaxCyclicLookaheads.Length * 2];
          Array.Copy((Array) this.decisionMaxCyclicLookaheads, 0, (Array) destinationArray, 0, this.decisionMaxCyclicLookaheads.Length);
          this.decisionMaxCyclicLookaheads = destinationArray;
        }
        this.decisionMaxCyclicLookaheads[this.numCyclicDecisions - 1] = this.maxLookaheadInCurrentDecision;
      }
      else
      {
        if (this.numFixedDecisions >= this.decisionMaxFixedLookaheads.Length)
        {
          int[] destinationArray = new int[this.decisionMaxFixedLookaheads.Length * 2];
          Array.Copy((Array) this.decisionMaxFixedLookaheads, 0, (Array) destinationArray, 0, this.decisionMaxFixedLookaheads.Length);
          this.decisionMaxFixedLookaheads = destinationArray;
        }
        this.decisionMaxFixedLookaheads[this.numFixedDecisions - 1] = this.maxLookaheadInCurrentDecision;
      }
      this.parser.isCyclicDecision = false;
      this.maxLookaheadInCurrentDecision = 0;
    }

    public override void ConsumeToken(IToken token) => this.lastTokenConsumed = (CommonToken) token;

    public bool InDecision() => this.decisionLevel > 0;

    public override void ConsumeHiddenToken(IToken token)
    {
      this.lastTokenConsumed = (CommonToken) token;
    }

    public override void LT(int i, IToken t)
    {
      if (!this.InDecision())
        return;
      int lookahead = (int) this.lookaheadStack[this.lookaheadStack.Count - 1];
      int j = this.parser.TokenStream.Index();
      int numberOfHiddenTokens = this.GetNumberOfHiddenTokens(lookahead, j);
      int num = i + j - lookahead - numberOfHiddenTokens;
      if (num <= this.maxLookaheadInCurrentDecision)
        return;
      this.maxLookaheadInCurrentDecision = num;
    }

    public override void BeginBacktrack(int level) => ++this.numBacktrackDecisions;

    public override void EndBacktrack(int level, bool successful)
    {
      this.decisionMaxSynPredLookaheads.Add((object) this.maxLookaheadInCurrentDecision);
    }

    public override void RecognitionException(Antlr.Runtime.RecognitionException e)
    {
      ++this.numberReportedErrors;
    }

    public override void SemanticPredicate(bool result, string predicate)
    {
      if (!this.InDecision())
        return;
      ++this.numSemanticPredicates;
    }

    public override void Terminate()
    {
      string notifyString = this.ToNotifyString();
      try
      {
        Stats.WriteReport("runtime.stats", notifyString);
      }
      catch (IOException ex)
      {
        Console.Error.WriteLine((object) ex);
        Console.Error.WriteLine(ex.StackTrace);
      }
      Console.Out.WriteLine(Profiler.ToString(notifyString));
    }

    public virtual DebugParser Parser
    {
      set => this.parser = value;
    }

    public virtual string ToNotifyString()
    {
      ITokenStream tokenStream = this.parser.TokenStream;
      for (int i = 0; i < tokenStream.Count && this.lastTokenConsumed != null && i <= this.lastTokenConsumed.TokenIndex; ++i)
      {
        IToken token = tokenStream.Get(i);
        if (token.Channel != 0)
        {
          ++this.numHiddenTokens;
          this.numHiddenCharsMatched += token.Text.Length;
        }
      }
      this.numCharsMatched = this.lastTokenConsumed.StopIndex + 1;
      this.decisionMaxFixedLookaheads = this.Trim(this.decisionMaxFixedLookaheads, this.numFixedDecisions);
      this.decisionMaxCyclicLookaheads = this.Trim(this.decisionMaxCyclicLookaheads, this.numCyclicDecisions);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("2");
      stringBuilder.Append('\t');
      stringBuilder.Append(this.parser.GetType().FullName);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numRuleInvocations);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.maxRuleInvocationDepth);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numFixedDecisions);
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Min(this.decisionMaxFixedLookaheads));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Max(this.decisionMaxFixedLookaheads));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Avg(this.decisionMaxFixedLookaheads));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Stddev(this.decisionMaxFixedLookaheads));
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numCyclicDecisions);
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Min(this.decisionMaxCyclicLookaheads));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Max(this.decisionMaxCyclicLookaheads));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Avg(this.decisionMaxCyclicLookaheads));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Stddev(this.decisionMaxCyclicLookaheads));
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numBacktrackDecisions);
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Min(this.ToArray(this.decisionMaxSynPredLookaheads)));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Max(this.ToArray(this.decisionMaxSynPredLookaheads)));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Avg(this.ToArray(this.decisionMaxSynPredLookaheads)));
      stringBuilder.Append('\t');
      stringBuilder.Append(Stats.Stddev(this.ToArray(this.decisionMaxSynPredLookaheads)));
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numSemanticPredicates);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.parser.TokenStream.Count);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numHiddenTokens);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numCharsMatched);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numHiddenCharsMatched);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numberReportedErrors);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numMemoizationCacheHits);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numMemoizationCacheMisses);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numGuessingRuleInvocations);
      stringBuilder.Append('\t');
      stringBuilder.Append(this.numMemoizationCacheEntries);
      return stringBuilder.ToString();
    }

    public override string ToString() => Profiler.ToString(this.ToNotifyString());

    protected static string[] DecodeReportData(string data)
    {
      string[] strArray = data.Split('\t');
      return strArray.Length != 29 ? (string[]) null : strArray;
    }

    public static string ToString(string notifyDataLine)
    {
      string[] strArray = Profiler.DecodeReportData(notifyDataLine);
      if (strArray == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("ANTLR Runtime Report; Profile Version ");
      stringBuilder.Append(strArray[0]);
      stringBuilder.Append('\n');
      stringBuilder.Append("parser name ");
      stringBuilder.Append(strArray[1]);
      stringBuilder.Append('\n');
      stringBuilder.Append("Number of rule invocations ");
      stringBuilder.Append(strArray[2]);
      stringBuilder.Append('\n');
      stringBuilder.Append("Number of rule invocations in \"guessing\" mode ");
      stringBuilder.Append(strArray[27]);
      stringBuilder.Append('\n');
      stringBuilder.Append("max rule invocation nesting depth ");
      stringBuilder.Append(strArray[3]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of fixed lookahead decisions ");
      stringBuilder.Append(strArray[4]);
      stringBuilder.Append('\n');
      stringBuilder.Append("min lookahead used in a fixed lookahead decision ");
      stringBuilder.Append(strArray[5]);
      stringBuilder.Append('\n');
      stringBuilder.Append("max lookahead used in a fixed lookahead decision ");
      stringBuilder.Append(strArray[6]);
      stringBuilder.Append('\n');
      stringBuilder.Append("average lookahead depth used in fixed lookahead decisions ");
      stringBuilder.Append(strArray[7]);
      stringBuilder.Append('\n');
      stringBuilder.Append("standard deviation of depth used in fixed lookahead decisions ");
      stringBuilder.Append(strArray[8]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of arbitrary lookahead decisions ");
      stringBuilder.Append(strArray[9]);
      stringBuilder.Append('\n');
      stringBuilder.Append("min lookahead used in an arbitrary lookahead decision ");
      stringBuilder.Append(strArray[10]);
      stringBuilder.Append('\n');
      stringBuilder.Append("max lookahead used in an arbitrary lookahead decision ");
      stringBuilder.Append(strArray[11]);
      stringBuilder.Append('\n');
      stringBuilder.Append("average lookahead depth used in arbitrary lookahead decisions ");
      stringBuilder.Append(strArray[12]);
      stringBuilder.Append('\n');
      stringBuilder.Append("standard deviation of depth used in arbitrary lookahead decisions ");
      stringBuilder.Append(strArray[13]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of evaluated syntactic predicates ");
      stringBuilder.Append(strArray[14]);
      stringBuilder.Append('\n');
      stringBuilder.Append("min lookahead used in a syntactic predicate ");
      stringBuilder.Append(strArray[15]);
      stringBuilder.Append('\n');
      stringBuilder.Append("max lookahead used in a syntactic predicate ");
      stringBuilder.Append(strArray[16]);
      stringBuilder.Append('\n');
      stringBuilder.Append("average lookahead depth used in syntactic predicates ");
      stringBuilder.Append(strArray[17]);
      stringBuilder.Append('\n');
      stringBuilder.Append("standard deviation of depth used in syntactic predicates ");
      stringBuilder.Append(strArray[18]);
      stringBuilder.Append('\n');
      stringBuilder.Append("rule memoization cache size ");
      stringBuilder.Append(strArray[28]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of rule memoization cache hits ");
      stringBuilder.Append(strArray[25]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of rule memoization cache misses ");
      stringBuilder.Append(strArray[26]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of evaluated semantic predicates ");
      stringBuilder.Append(strArray[19]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of tokens ");
      stringBuilder.Append(strArray[20]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of hidden tokens ");
      stringBuilder.Append(strArray[21]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of char ");
      stringBuilder.Append(strArray[22]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of hidden char ");
      stringBuilder.Append(strArray[23]);
      stringBuilder.Append('\n');
      stringBuilder.Append("number of syntax errors ");
      stringBuilder.Append(strArray[24]);
      stringBuilder.Append('\n');
      return stringBuilder.ToString();
    }

    protected int[] Trim(int[] X, int n)
    {
      if (n < X.Length)
      {
        int[] destinationArray = new int[n];
        Array.Copy((Array) X, 0, (Array) destinationArray, 0, n);
        X = destinationArray;
      }
      return X;
    }

    protected int[] ToArray(IList a)
    {
      int[] array = new int[a.Count];
      a.CopyTo((Array) array, 0);
      return array;
    }

    public int GetNumberOfHiddenTokens(int i, int j)
    {
      int numberOfHiddenTokens = 0;
      ITokenStream tokenStream = this.parser.TokenStream;
      for (int i1 = i; i1 < tokenStream.Count && i1 <= j; ++i1)
      {
        if (tokenStream.Get(i1).Channel != 0)
          ++numberOfHiddenTokens;
      }
      return numberOfHiddenTokens;
    }
  }
}
