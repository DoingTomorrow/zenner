// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WrapperLayoutRendererBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  public abstract class WrapperLayoutRendererBase : LayoutRenderer
  {
    [DefaultParameter]
    public Layout Inner { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      string text = this.RenderInner(logEvent);
      builder.Append(this.Transform(logEvent, text));
    }

    protected virtual string Transform(LogEventInfo logEvent, string text) => this.Transform(text);

    protected abstract string Transform(string text);

    protected virtual string RenderInner(LogEventInfo logEvent) => this.Inner.Render(logEvent);
  }
}
