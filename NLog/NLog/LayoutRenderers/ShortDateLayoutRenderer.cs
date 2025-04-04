// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ShortDateLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("shortdate")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class ShortDateLayoutRenderer : LayoutRenderer
  {
    private ShortDateLayoutRenderer.CachedDateFormatted _cachedDateFormatted = new ShortDateLayoutRenderer.CachedDateFormatted(DateTime.MaxValue, string.Empty);

    [DefaultValue(false)]
    public bool UniversalTime { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      DateTime dateTime = logEvent.TimeStamp;
      if (this.UniversalTime)
        dateTime = dateTime.ToUniversalTime();
      ShortDateLayoutRenderer.CachedDateFormatted cachedDateFormatted = this._cachedDateFormatted;
      if (cachedDateFormatted.Date != dateTime.Date)
      {
        string formattedDate = dateTime.ToString("yyyy-MM-dd", (IFormatProvider) CultureInfo.InvariantCulture);
        this._cachedDateFormatted = cachedDateFormatted = new ShortDateLayoutRenderer.CachedDateFormatted(dateTime.Date, formattedDate);
      }
      builder.Append(cachedDateFormatted.FormattedDate);
    }

    private class CachedDateFormatted
    {
      public readonly DateTime Date;
      public readonly string FormattedDate;

      public CachedDateFormatted(DateTime date, string formattedDate)
      {
        this.Date = date;
        this.FormattedDate = formattedDate;
      }
    }
  }
}
