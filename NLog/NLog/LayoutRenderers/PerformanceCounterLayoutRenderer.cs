// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.PerformanceCounterLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("performancecounter")]
  public class PerformanceCounterLayoutRenderer : LayoutRenderer
  {
    private PerformanceCounter perfCounter;

    [RequiredParameter]
    public string Category { get; set; }

    [RequiredParameter]
    public string Counter { get; set; }

    public string Instance { get; set; }

    public string MachineName { get; set; }

    protected override void InitializeLayoutRenderer()
    {
      base.InitializeLayoutRenderer();
      if (this.MachineName != null)
        this.perfCounter = new PerformanceCounter(this.Category, this.Counter, this.Instance, this.MachineName);
      else
        this.perfCounter = new PerformanceCounter(this.Category, this.Counter, this.Instance, true);
    }

    protected override void CloseLayoutRenderer()
    {
      base.CloseLayoutRenderer();
      if (this.perfCounter == null)
        return;
      this.perfCounter.Close();
      this.perfCounter = (PerformanceCounter) null;
    }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      builder.Append(this.perfCounter.NextValue().ToString(formatProvider));
    }
  }
}
