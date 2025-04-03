// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.FixedFormates
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Globalization;

#nullable disable
namespace ZR_ClassLibrary
{
  public class FixedFormates : IFormatProvider
  {
    public static FixedFormates TheFormates = new FixedFormates();
    public static string CurrentCultureFullDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
    public NumberFormatInfo NumberFormat;
    public DateTimeFormatInfo DateTimeFormat;

    private FixedFormates()
    {
      this.NumberFormat = new NumberFormatInfo();
      this.NumberFormat.CurrencyDecimalSeparator = ".";
      this.DateTimeFormat = new CultureInfo("de-de", false).DateTimeFormat;
      this.DateTimeFormat.FullDateTimePattern = "dd.MM.yyyy HH:mm:ss";
    }

    public object GetFormat(Type argType)
    {
      if (argType == typeof (NumberFormatInfo))
        return (object) this.NumberFormat;
      return argType == typeof (DateTimeFormatInfo) ? (object) this.DateTimeFormat : (object) null;
    }
  }
}
