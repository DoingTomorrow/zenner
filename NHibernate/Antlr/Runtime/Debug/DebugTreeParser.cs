// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.DebugTreeParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Misc;
using Antlr.Runtime.Tree;
using System.IO;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class DebugTreeParser : TreeParser
  {
    protected IDebugEventListener dbg;
    public bool isCyclicDecision;

    public DebugTreeParser(
      ITreeNodeStream input,
      IDebugEventListener dbg,
      RecognizerSharedState state)
      : base(!(input is DebugTreeNodeStream) ? (ITreeNodeStream) new DebugTreeNodeStream(input, dbg) : input, state)
    {
      this.DebugListener = dbg;
    }

    public DebugTreeParser(ITreeNodeStream input, RecognizerSharedState state)
      : base(!(input is DebugTreeNodeStream) ? (ITreeNodeStream) new DebugTreeNodeStream(input, (IDebugEventListener) null) : input, state)
    {
    }

    public DebugTreeParser(ITreeNodeStream input, IDebugEventListener dbg)
      : this(!(input is DebugTreeNodeStream) ? (ITreeNodeStream) new DebugTreeNodeStream(input, dbg) : input, dbg, (RecognizerSharedState) null)
    {
    }

    public IDebugEventListener DebugListener
    {
      get => this.dbg;
      set
      {
        if (this.input is DebugTreeNodeStream)
          ((DebugTreeNodeStream) this.input).SetDebugListener(value);
        this.dbg = value;
      }
    }

    public virtual void ReportError(IOException e) => ErrorManager.InternalError((object) e);

    public override void ReportError(RecognitionException e) => this.dbg.RecognitionException(e);

    protected override object GetMissingSymbol(
      IIntStream input,
      RecognitionException e,
      int expectedTokenType,
      BitSet follow)
    {
      object missingSymbol = base.GetMissingSymbol(input, e, expectedTokenType, follow);
      this.dbg.ConsumeNode(missingSymbol);
      return missingSymbol;
    }

    public override void BeginResync() => this.dbg.BeginResync();

    public override void EndResync() => this.dbg.EndResync();

    public override void BeginBacktrack(int level) => this.dbg.BeginBacktrack(level);

    public override void EndBacktrack(int level, bool successful)
    {
      this.dbg.EndBacktrack(level, successful);
    }
  }
}
