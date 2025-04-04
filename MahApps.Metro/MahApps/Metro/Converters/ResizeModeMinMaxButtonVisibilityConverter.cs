// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.ResizeModeMinMaxButtonVisibilityConverter
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace MahApps.Metro.Converters
{
  public sealed class ResizeModeMinMaxButtonVisibilityConverter : IMultiValueConverter
  {
    private static ResizeModeMinMaxButtonVisibilityConverter _instance;

    private ResizeModeMinMaxButtonVisibilityConverter()
    {
    }

    public static ResizeModeMinMaxButtonVisibilityConverter Instance
    {
      get
      {
        return ResizeModeMinMaxButtonVisibilityConverter._instance ?? (ResizeModeMinMaxButtonVisibilityConverter._instance = new ResizeModeMinMaxButtonVisibilityConverter());
      }
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      string str = parameter as string;
      if (values == null || string.IsNullOrEmpty(str))
        return (object) Visibility.Visible;
      bool flag1 = values.Length != 0 && (bool) values[0];
      bool flag2 = values.Length > 1 && (bool) values[1];
      ResizeMode resizeMode = values.Length > 2 ? (ResizeMode) values[2] : ResizeMode.CanResize;
      if (str == "CLOSE")
        return (object) (Visibility) (flag2 || !flag1 ? 2 : 0);
      switch (resizeMode)
      {
        case ResizeMode.NoResize:
          return (object) Visibility.Collapsed;
        case ResizeMode.CanMinimize:
          return str == "MIN" ? (object) (Visibility) (flag2 || !flag1 ? 2 : 0) : (object) Visibility.Collapsed;
        default:
          return (object) (Visibility) (flag2 || !flag1 ? 2 : 0);
      }
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
