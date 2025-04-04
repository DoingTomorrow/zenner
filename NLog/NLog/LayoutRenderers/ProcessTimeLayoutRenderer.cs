// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ProcessTimeLayoutRenderer
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
  [LayoutRenderer("processtime")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class ProcessTimeLayoutRenderer : LayoutRenderer
  {
    [DefaultValue(false)]
    public bool Invariant { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      TimeSpan ts = logEvent.TimeStamp.ToUniversalTime() - LogEventInfo.ZeroDate;
      CultureInfo culture = this.Invariant ? (CultureInfo) null : this.GetCulture(logEvent);
      ProcessTimeLayoutRenderer.WritetTimestamp(builder, ts, culture);
    }

    internal static void WritetTimestamp(StringBuilder builder, TimeSpan ts, CultureInfo culture)
    {
      string str1 = ":";
      string str2 = ".";
      if (culture != null)
      {
        str1 = culture.DateTimeFormat.TimeSeparator;
        str2 = culture.NumberFormat.NumberDecimalSeparator;
      }
      builder.Append2DigitsZeroPadded(ts.Hours);
      builder.Append(str1);
      builder.Append2DigitsZeroPadded(ts.Minutes);
      builder.Append(str1);
      builder.Append2DigitsZeroPadded(ts.Seconds);
      builder.Append(str2);
      int milliseconds = ts.Milliseconds;
      if (milliseconds < 100)
      {
        builder.Append('0');
        if (milliseconds < 10)
        {
          builder.Append('0');
          if (milliseconds < 0)
          {
            builder.Append('0');
            return;
          }
        }
      }
      builder.AppendInvariant(milliseconds);
    }
  }
}
