// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.GdcLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("gdc")]
  [ThreadSafe]
  public class GdcLayoutRenderer : LayoutRenderer
  {
    [RequiredParameter]
    [DefaultParameter]
    public string Item { get; set; }

    public string Format { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      object obj = GlobalDiagnosticsContext.GetObject(this.Item);
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      builder.AppendFormattedValue(obj, this.Format, formatProvider);
    }
  }
}
