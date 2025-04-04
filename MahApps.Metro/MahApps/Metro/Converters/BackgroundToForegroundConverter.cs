// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.BackgroundToForegroundConverter
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Converters
{
  public class BackgroundToForegroundConverter : IValueConverter, IMultiValueConverter
  {
    private static BackgroundToForegroundConverter _instance;

    private BackgroundToForegroundConverter()
    {
    }

    public static BackgroundToForegroundConverter Instance
    {
      get
      {
        return BackgroundToForegroundConverter._instance ?? (BackgroundToForegroundConverter._instance = new BackgroundToForegroundConverter());
      }
    }

    private Color IdealTextColor(Color bg)
    {
      return (int) byte.MaxValue - System.Convert.ToInt32((double) bg.R * 0.299 + (double) bg.G * 0.587 + (double) bg.B * 0.114) < 105 ? Colors.Black : Colors.White;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is SolidColorBrush))
        return (object) Brushes.White;
      SolidColorBrush solidColorBrush = new SolidColorBrush(this.IdealTextColor(((SolidColorBrush) value).Color));
      solidColorBrush.Freeze();
      return (object) solidColorBrush;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return DependencyProperty.UnsetValue;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      Brush brush = values.Length != 0 ? values[0] as Brush : (Brush) null;
      return (values.Length > 1 ? (object) (values[1] as Brush) : (object) (Brush) null) ?? this.Convert((object) brush, targetType, parameter, culture);
    }

    public object[] ConvertBack(
      object value,
      Type[] targetTypes,
      object parameter,
      CultureInfo culture)
    {
      return ((IEnumerable<Type>) targetTypes).Select<Type, object>((Func<Type, object>) (t => DependencyProperty.UnsetValue)).ToArray<object>();
    }
  }
}
