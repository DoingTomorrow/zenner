// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.OnExceptionLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  [LayoutRenderer("onexception")]
  [ThreadAgnostic]
  [ThreadSafe]
  public sealed class OnExceptionLayoutRendererWrapper : WrapperLayoutRendererBase
  {
    protected override string Transform(string text) => text;

    protected override string RenderInner(LogEventInfo logEvent)
    {
      return logEvent.Exception != null ? base.RenderInner(logEvent) : string.Empty;
    }
  }
}
