// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.TraceActivityIdLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("activityid")]
  [ThreadSafe]
  public class TraceActivityIdLayoutRenderer : LayoutRenderer
  {
    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      builder.Append(Guid.Empty.Equals(Trace.CorrelationManager.ActivityId) ? string.Empty : Trace.CorrelationManager.ActivityId.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture));
    }
  }
}
