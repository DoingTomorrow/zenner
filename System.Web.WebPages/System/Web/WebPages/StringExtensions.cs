// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.StringExtensions
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace System.Web.WebPages
{
  public static class StringExtensions
  {
    public static bool IsEmpty(this string value) => string.IsNullOrEmpty(value);

    public static int AsInt(this string value) => value.AsInt(0);

    public static int AsInt(this string value, int defaultValue)
    {
      int result;
      return !int.TryParse(value, out result) ? defaultValue : result;
    }

    public static Decimal AsDecimal(this string value) => value.As<Decimal>();

    public static Decimal AsDecimal(this string value, Decimal defaultValue)
    {
      return value.As<Decimal>(defaultValue);
    }

    public static float AsFloat(this string value) => value.AsFloat(0.0f);

    public static float AsFloat(this string value, float defaultValue)
    {
      float result;
      return !float.TryParse(value, out result) ? defaultValue : result;
    }

    public static DateTime AsDateTime(this string value) => value.AsDateTime(new DateTime());

    public static DateTime AsDateTime(this string value, DateTime defaultValue)
    {
      DateTime result;
      return !DateTime.TryParse(value, out result) ? defaultValue : result;
    }

    public static TValue As<TValue>(this string value) => value.As<TValue>(default (TValue));

    public static bool AsBool(this string value) => value.AsBool(false);

    public static bool AsBool(this string value, bool defaultValue)
    {
      bool result;
      return !bool.TryParse(value, out result) ? defaultValue : result;
    }

    public static TValue As<TValue>(this string value, TValue defaultValue)
    {
      try
      {
        TypeConverter converter1 = TypeDescriptor.GetConverter(typeof (TValue));
        if (converter1.CanConvertFrom(typeof (string)))
          return (TValue) converter1.ConvertFrom((object) value);
        TypeConverter converter2 = TypeDescriptor.GetConverter(typeof (string));
        if (converter2.CanConvertTo(typeof (TValue)))
          return (TValue) converter2.ConvertTo((object) value, typeof (TValue));
      }
      catch
      {
      }
      return defaultValue;
    }

    public static bool IsBool(this string value) => bool.TryParse(value, out bool _);

    public static bool IsInt(this string value) => int.TryParse(value, out int _);

    public static bool IsDecimal(this string value) => value.Is<Decimal>();

    public static bool IsFloat(this string value) => float.TryParse(value, out float _);

    public static bool IsDateTime(this string value) => DateTime.TryParse(value, out DateTime _);

    public static bool Is<TValue>(this string value)
    {
      TypeConverter converter = TypeDescriptor.GetConverter(typeof (TValue));
      if (converter != null)
      {
        try
        {
          if (value != null)
          {
            if (!converter.CanConvertFrom((ITypeDescriptorContext) null, value.GetType()))
              goto label_5;
          }
          converter.ConvertFrom((ITypeDescriptorContext) null, CultureInfo.CurrentCulture, (object) value);
          return true;
        }
        catch
        {
        }
      }
label_5:
      return false;
    }
  }
}
