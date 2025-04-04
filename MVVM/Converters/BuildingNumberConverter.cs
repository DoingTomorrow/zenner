// Decompiled with JetBrains decompiler
// Type: MVVM.Converters.BuildingNumberConverter
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

#nullable disable
namespace MVVM.Converters
{
  public class BuildingNumberConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string str = (string) value;
      if (string.IsNullOrEmpty(str) || str.Length != 10)
        return (object) str;
      StringBuilder stringBuilder = new StringBuilder(str);
      stringBuilder.Insert(3, '.');
      stringBuilder.Insert(7, '-');
      stringBuilder.Insert(9, '/');
      return (object) stringBuilder.ToString();
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      string str = (string) value;
      if (string.IsNullOrEmpty(str) || str.Length != 10)
        return (object) str;
      StringBuilder stringBuilder = new StringBuilder(str);
      stringBuilder.Insert(3, '.');
      stringBuilder.Insert(7, '-');
      stringBuilder.Insert(9, '/');
      return (object) stringBuilder.ToString();
    }
  }
}
