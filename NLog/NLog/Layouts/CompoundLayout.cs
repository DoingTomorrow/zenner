// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.CompoundLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.Layouts
{
  [Layout("CompoundLayout")]
  [NLog.Config.ThreadAgnostic]
  [NLog.Config.ThreadSafe]
  [AppDomainFixedOutput]
  public class CompoundLayout : Layout
  {
    public CompoundLayout() => this.Layouts = (IList<Layout>) new List<Layout>();

    [ArrayParameter(typeof (Layout), "layout")]
    public IList<Layout> Layouts { get; private set; }

    protected override void InitializeLayout()
    {
      base.InitializeLayout();
      foreach (Layout layout in (IEnumerable<Layout>) this.Layouts)
        layout.Initialize(this.LoggingConfiguration);
    }

    internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
    {
      if (this.ThreadAgnostic)
        return;
      this.RenderAppendBuilder(logEvent, target, true);
    }

    protected override string GetFormattedMessage(LogEventInfo logEvent)
    {
      return this.RenderAllocateBuilder(logEvent);
    }

    protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      for (int index = 0; index < this.Layouts.Count; ++index)
        this.Layouts[index].RenderAppendBuilder(logEvent, target);
    }

    protected override void CloseLayout()
    {
      foreach (Layout layout in (IEnumerable<Layout>) this.Layouts)
        layout.Close();
      base.CloseLayout();
    }

    public override string ToString()
    {
      return this.ToStringWithNestedItems<Layout>(this.Layouts, (Func<Layout, string>) (l => l.ToString()));
    }
  }
}
