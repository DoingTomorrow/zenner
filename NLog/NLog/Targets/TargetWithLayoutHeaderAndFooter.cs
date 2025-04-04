// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TargetWithLayoutHeaderAndFooter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;

#nullable disable
namespace NLog.Targets
{
  public abstract class TargetWithLayoutHeaderAndFooter : TargetWithLayout
  {
    [RequiredParameter]
    public override Layout Layout
    {
      get => this.LHF.Layout;
      set
      {
        if (value is LayoutWithHeaderAndFooter)
          base.Layout = value;
        else if (this.LHF == null)
          this.LHF = new LayoutWithHeaderAndFooter()
          {
            Layout = value
          };
        else
          this.LHF.Layout = value;
      }
    }

    public Layout Footer
    {
      get => this.LHF.Footer;
      set => this.LHF.Footer = value;
    }

    public Layout Header
    {
      get => this.LHF.Header;
      set => this.LHF.Header = value;
    }

    private LayoutWithHeaderAndFooter LHF
    {
      get => (LayoutWithHeaderAndFooter) base.Layout;
      set => base.Layout = (Layout) value;
    }
  }
}
