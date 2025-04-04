// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.LoggerNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("logger")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class LoggerNameLayoutRenderer : LayoutRenderer
  {
    [DefaultValue(false)]
    public bool ShortName { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (this.ShortName)
      {
        int num = logEvent.LoggerName.LastIndexOf('.');
        if (num < 0)
          builder.Append(logEvent.LoggerName);
        else
          builder.Append(logEvent.LoggerName, num + 1, logEvent.LoggerName.Length - num - 1);
      }
      else
        builder.Append(logEvent.LoggerName);
    }
  }
}
