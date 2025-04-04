// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.TimeLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("time")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class TimeLayoutRenderer : LayoutRenderer
  {
    [DefaultValue(false)]
    public bool UniversalTime { get; set; }

    [DefaultValue(false)]
    public bool Invariant { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      DateTime dateTime = logEvent.TimeStamp;
      if (this.UniversalTime)
        dateTime = dateTime.ToUniversalTime();
      string str1 = ":";
      string str2 = ".";
      if (!this.Invariant)
      {
        CultureInfo culture = this.GetCulture(logEvent);
        if (culture != null)
        {
          str1 = culture.DateTimeFormat.TimeSeparator;
          str2 = culture.NumberFormat.NumberDecimalSeparator;
        }
      }
      builder.Append2DigitsZeroPadded(dateTime.Hour);
      builder.Append(str1);
      builder.Append2DigitsZeroPadded(dateTime.Minute);
      builder.Append(str1);
      builder.Append2DigitsZeroPadded(dateTime.Second);
      builder.Append(str2);
      builder.Append4DigitsZeroPadded((int) (dateTime.Ticks % 10000000L) / 1000);
    }
  }
}
