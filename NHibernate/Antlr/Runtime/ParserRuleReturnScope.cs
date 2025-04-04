// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ParserRuleReturnScope
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime
{
  internal class ParserRuleReturnScope : RuleReturnScope
  {
    private IToken start;
    private IToken stop;

    public override object Start
    {
      get => (object) this.start;
      set => this.start = (IToken) value;
    }

    public override object Stop
    {
      get => (object) this.stop;
      set => this.stop = (IToken) value;
    }
  }
}
