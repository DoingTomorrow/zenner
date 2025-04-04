// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.InstallContextLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("install-context")]
  [ThreadSafe]
  public class InstallContextLayoutRenderer : LayoutRenderer
  {
    [RequiredParameter]
    [DefaultParameter]
    public string Parameter { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      object obj;
      if (!logEvent.Properties.TryGetValue((object) this.Parameter, out obj))
        return;
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      builder.Append(Convert.ToString(obj, formatProvider));
    }
  }
}
