// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.FontSizeOffsetConverter
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace MahApps.Metro.Converters
{
  public class FontSizeOffsetConverter : IValueConverter
  {
    private static FontSizeOffsetConverter _instance;

    private FontSizeOffsetConverter()
    {
    }

    public static FontSizeOffsetConverter Instance
    {
      get
      {
        return FontSizeOffsetConverter._instance ?? (FontSizeOffsetConverter._instance = new FontSizeOffsetConverter());
      }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value is double && parameter is double num ? (object) Math.Round((double) value + num) : value;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }
  }
}
