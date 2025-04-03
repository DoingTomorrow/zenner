// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ParserRuleReturnScope`1
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

#nullable disable
namespace Antlr.Runtime
{
  public class ParserRuleReturnScope<TToken> : IRuleReturnScope<TToken>, IRuleReturnScope
  {
    private TToken _start;
    private TToken _stop;

    public TToken Start
    {
      get => this._start;
      set => this._start = value;
    }

    public TToken Stop
    {
      get => this._stop;
      set => this._stop = value;
    }

    object IRuleReturnScope.Start => (object) this.Start;

    object IRuleReturnScope.Stop => (object) this.Stop;
  }
}
