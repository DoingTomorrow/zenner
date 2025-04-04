// Decompiled with JetBrains decompiler
// Type: MVVM.Converters.IsNotNullOrFalseConverter
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace MVVM.Converters
{
  public class IsNotNullOrFalseConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value == null || value.GetType() != typeof (bool?) && value.GetType() != typeof (bool) ? (object) false : (object) (bool) value;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new InvalidOperationException("IsNotNullConverter can only be used OneWay.");
    }
  }
}
