// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.EventContextLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("event-context")]
  [Obsolete("Use EventPropertiesLayoutRenderer class instead. Marked obsolete on NLog 2.0")]
  public class EventContextLayoutRenderer : LayoutRenderer
  {
    [RequiredParameter]
    [DefaultParameter]
    public string Item { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      object obj;
      if (!logEvent.HasProperties || !logEvent.Properties.TryGetValue((object) this.Item, out obj))
        return;
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      builder.Append(Convert.ToString(obj, formatProvider));
    }
  }
}
