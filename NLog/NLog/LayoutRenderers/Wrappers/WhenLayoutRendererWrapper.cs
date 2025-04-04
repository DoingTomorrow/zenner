// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WhenLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Conditions;
using NLog.Config;
using NLog.Layouts;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("when")]
  [AmbientProperty("When")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class WhenLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
  {
    [RequiredParameter]
    public ConditionExpression When { get; set; }

    public Layout Else { get; set; }

    protected override void TransformFormattedMesssage(StringBuilder target)
    {
    }

    protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      if (this.When == null || true.Equals(this.When.Evaluate(logEvent)))
      {
        base.RenderFormattedMessage(logEvent, target);
      }
      else
      {
        if (this.Else == null)
          return;
        this.Else.RenderAppendBuilder(logEvent, target);
      }
    }
  }
}
