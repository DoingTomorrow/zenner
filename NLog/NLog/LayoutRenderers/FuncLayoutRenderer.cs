// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.FuncLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  public class FuncLayoutRenderer : LayoutRenderer
  {
    public FuncLayoutRenderer(
      string layoutRendererName,
      Func<LogEventInfo, LoggingConfiguration, object> renderMethod)
    {
      this.RenderMethod = renderMethod;
      this.LayoutRendererName = layoutRendererName;
    }

    public string LayoutRendererName { get; set; }

    public Func<LogEventInfo, LoggingConfiguration, object> RenderMethod { get; private set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (this.RenderMethod == null)
        return;
      builder.Append(this.RenderMethod(logEvent, this.LoggingConfiguration));
    }
  }
}
