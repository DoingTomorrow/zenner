// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.DebugParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Misc;
using System.IO;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class DebugParser : Parser
  {
    protected internal IDebugEventListener dbg;
    public bool isCyclicDecision;

    public DebugParser(ITokenStream input, IDebugEventListener dbg, RecognizerSharedState state)
      : base(!(input is DebugTokenStream) ? (ITokenStream) new DebugTokenStream(input, dbg) : input, state)
    {
      this.DebugListener = dbg;
    }

    public DebugParser(ITokenStream input, RecognizerSharedState state)
      : base(!(input is DebugTokenStream) ? (ITokenStream) new DebugTokenStream(input, (IDebugEventListener) null) : input, state)
    {
    }

    public DebugParser(ITokenStream input, IDebugEventListener dbg)
      : this(!(input is DebugTokenStream) ? (ITokenStream) new DebugTokenStream(input, dbg) : input, dbg, (RecognizerSharedState) null)
    {
    }

    public virtual IDebugEventListener DebugListener
    {
      get => this.dbg;
      set
      {
        if (this.input is DebugTokenStream)
          ((DebugTokenStream) this.input).DebugListener = value;
        this.dbg = value;
      }
    }

    public virtual void ReportError(IOException e) => ErrorManager.InternalError((object) e);

    public override void BeginResync() => this.dbg.BeginResync();

    public override void EndResync() => this.dbg.EndResync();

    public override void ReportError(RecognitionException e) => this.dbg.RecognitionException(e);
  }
}
