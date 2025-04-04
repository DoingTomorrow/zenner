// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WrapperLayoutRendererBuilderBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers
{
  public abstract class WrapperLayoutRendererBuilderBase : WrapperLayoutRendererBase
  {
    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      using (AppendBuilderCreator appendBuilderCreator = new AppendBuilderCreator(builder, true))
      {
        this.RenderFormattedMessage(logEvent, appendBuilderCreator.Builder);
        this.TransformFormattedMesssage(logEvent, appendBuilderCreator.Builder);
      }
    }

    protected virtual void TransformFormattedMesssage(LogEventInfo logEvent, StringBuilder target)
    {
      this.TransformFormattedMesssage(target);
    }

    protected abstract void TransformFormattedMesssage(StringBuilder target);

    protected virtual void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      this.Inner.RenderAppendBuilder(logEvent, target);
    }

    protected override sealed string Transform(string text)
    {
      throw new NotSupportedException("Use TransformFormattedMesssage");
    }

    protected override sealed string RenderInner(LogEventInfo logEvent)
    {
      throw new NotSupportedException("Use RenderFormattedMessage");
    }
  }
}
