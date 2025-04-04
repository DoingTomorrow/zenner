// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.LongDateLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("longdate")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class LongDateLayoutRenderer : LayoutRenderer
  {
    [DefaultValue(false)]
    public bool UniversalTime { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      DateTime dateTime = logEvent.TimeStamp;
      if (this.UniversalTime)
        dateTime = dateTime.ToUniversalTime();
      builder.Append4DigitsZeroPadded(dateTime.Year);
      builder.Append('-');
      builder.Append2DigitsZeroPadded(dateTime.Month);
      builder.Append('-');
      builder.Append2DigitsZeroPadded(dateTime.Day);
      builder.Append(' ');
      builder.Append2DigitsZeroPadded(dateTime.Hour);
      builder.Append(':');
      builder.Append2DigitsZeroPadded(dateTime.Minute);
      builder.Append(':');
      builder.Append2DigitsZeroPadded(dateTime.Second);
      builder.Append('.');
      builder.Append4DigitsZeroPadded((int) (dateTime.Ticks % 10000000L) / 1000);
    }
  }
}
