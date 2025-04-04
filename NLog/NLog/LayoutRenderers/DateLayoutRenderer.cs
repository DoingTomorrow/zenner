// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.DateLayoutRenderer
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
  [LayoutRenderer("date")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class DateLayoutRenderer : LayoutRenderer
  {
    private string _format;
    private const string _lowTimeResolutionChars = "YyMDdHh";
    private DateLayoutRenderer.CachedDateFormatted _cachedDateFormatted = new DateLayoutRenderer.CachedDateFormatted(DateTime.MinValue, string.Empty);

    public DateLayoutRenderer()
    {
      this.Format = "yyyy/MM/dd HH:mm:ss.fff";
      this.Culture = CultureInfo.InvariantCulture;
    }

    public CultureInfo Culture { get; set; }

    [DefaultParameter]
    public string Format
    {
      get => this._format;
      set
      {
        this._format = value;
        if (DateLayoutRenderer.IsLowTimeResolutionLayout(this._format))
          this._cachedDateFormatted = new DateLayoutRenderer.CachedDateFormatted(DateTime.MaxValue, string.Empty);
        else
          this._cachedDateFormatted = new DateLayoutRenderer.CachedDateFormatted(DateTime.MinValue, string.Empty);
      }
    }

    [DefaultValue(false)]
    public bool UniversalTime { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent, (IFormatProvider) this.Culture);
      DateTime dateTime1 = logEvent.TimeStamp;
      if (this.UniversalTime)
        dateTime1 = dateTime1.ToUniversalTime();
      DateLayoutRenderer.CachedDateFormatted cachedDateFormatted = this._cachedDateFormatted;
      DateTime date1;
      if (formatProvider != CultureInfo.InvariantCulture || cachedDateFormatted.Date == DateTime.MinValue)
      {
        cachedDateFormatted = (DateLayoutRenderer.CachedDateFormatted) null;
      }
      else
      {
        DateTime date2 = cachedDateFormatted.Date;
        date1 = dateTime1.Date;
        DateTime dateTime2 = date1.AddHours((double) dateTime1.Hour);
        if (date2 == dateTime2)
        {
          builder.Append(cachedDateFormatted.FormattedDate);
          return;
        }
      }
      string formattedDate = dateTime1.ToString(this._format, formatProvider);
      if (cachedDateFormatted != null)
      {
        date1 = dateTime1.Date;
        this._cachedDateFormatted = new DateLayoutRenderer.CachedDateFormatted(date1.AddHours((double) dateTime1.Hour), formattedDate);
      }
      builder.Append(formattedDate);
    }

    private static bool IsLowTimeResolutionLayout(string dateTimeFormat)
    {
      for (int index = 0; index < dateTimeFormat.Length; ++index)
      {
        char c = dateTimeFormat[index];
        if (char.IsLetter(c) && "YyMDdHh".IndexOf(c) < 0)
          return false;
      }
      return true;
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
