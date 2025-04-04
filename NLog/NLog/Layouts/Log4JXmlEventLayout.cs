// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.Log4JXmlEventLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.LayoutRenderers;
using System.Text;

#nullable disable
namespace NLog.Layouts
{
  [Layout("Log4JXmlEventLayout")]
  [NLog.Config.ThreadAgnostic]
  [NLog.Config.ThreadSafe]
  [AppDomainFixedOutput]
  public class Log4JXmlEventLayout : Layout, IIncludeContext
  {
    public Log4JXmlEventLayout() => this.Renderer = new Log4JXmlEventLayoutRenderer();

    public Log4JXmlEventLayoutRenderer Renderer { get; private set; }

    public bool IncludeMdc
    {
      get => this.Renderer.IncludeMdc;
      set => this.Renderer.IncludeMdc = value;
    }

    public bool IncludeAllProperties
    {
      get => this.Renderer.IncludeAllProperties;
      set => this.Renderer.IncludeAllProperties = value;
    }

    public bool IncludeNdc
    {
      get => this.Renderer.IncludeNdc;
      set => this.Renderer.IncludeNdc = value;
    }

    public bool IncludeMdlc
    {
      get => this.Renderer.IncludeMdlc;
      set => this.Renderer.IncludeMdlc = value;
    }

    public bool IncludeNdlc
    {
      get => this.Renderer.IncludeNdlc;
      set => this.Renderer.IncludeNdlc = value;
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
      this.Renderer.RenderAppendBuilder(logEvent, target);
    }
  }
}
