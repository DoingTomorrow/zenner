// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WhenEmptyLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("whenEmpty")]
  [AmbientProperty("WhenEmpty")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class WhenEmptyLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    [RequiredParameter]
    public Layout WhenEmpty { get; set; }

    protected override void TransformFormattedMesssage(StringBuilder target)
    {
    }

    protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      int length = target.Length;
      base.RenderFormattedMessage(logEvent, target);
      if (target.Length > length)
        return;
      this.WhenEmpty.RenderAppendBuilder(logEvent, target);
    }
  }
}
