// Decompiled with JetBrains decompiler
// Type: StartupLib.PercentageConverter
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace StartupLib
{
  public class PercentageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter) / 100.0);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
