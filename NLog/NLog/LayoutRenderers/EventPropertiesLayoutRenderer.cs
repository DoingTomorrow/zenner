// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.EventPropertiesLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("event-properties")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class EventPropertiesLayoutRenderer : LayoutRenderer
  {
    public EventPropertiesLayoutRenderer() => this.Culture = CultureInfo.InvariantCulture;

    [RequiredParameter]
    [DefaultParameter]
    public string Item { get; set; }

    public string Format { get; set; }

    public CultureInfo Culture { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      object obj;
      if (!logEvent.HasProperties || !logEvent.Properties.TryGetValue((object) this.Item, out obj))
        return;
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent, (IFormatProvider) this.Culture);
      builder.AppendFormattedValue(obj, this.Format, formatProvider);
    }
  }
}
