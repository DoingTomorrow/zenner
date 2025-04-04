// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.ThicknessBindingConverter
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
  public class ThicknessBindingConverter : IValueConverter
  {
    public IgnoreThicknessSideType IgnoreThicknessSide { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is Thickness))
        return (object) new Thickness();
      if (parameter is IgnoreThicknessSideType thicknessSideType)
        this.IgnoreThicknessSide = thicknessSideType;
      Thickness thickness = (Thickness) value;
      switch (this.IgnoreThicknessSide)
      {
        case IgnoreThicknessSideType.Left:
          return (object) new Thickness(0.0, thickness.Top, thickness.Right, thickness.Bottom);
        case IgnoreThicknessSideType.Top:
          return (object) new Thickness(thickness.Left, 0.0, thickness.Right, thickness.Bottom);
        case IgnoreThicknessSideType.Right:
          return (object) new Thickness(thickness.Left, thickness.Top, 0.0, thickness.Bottom);
        case IgnoreThicknessSideType.Bottom:
          return (object) new Thickness(thickness.Left, thickness.Top, thickness.Right, 0.0);
        default:
          return (object) thickness;
      }
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
