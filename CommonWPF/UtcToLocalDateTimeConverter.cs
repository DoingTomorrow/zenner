// Decompiled with JetBrains decompiler
// Type: CommonWPF.UtcToLocalDateTimeConverter
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace CommonWPF
{
  public class UtcToLocalDateTimeConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      DateTime result;
      return DateTime.TryParse(value.ToString(), out result) ? (object) DateTime.SpecifyKind(result, DateTimeKind.Utc).ToLocalTime() : value;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      DateTime result;
      return DateTime.TryParse(value.ToString(), out result) ? (object) result.ToUniversalTime() : value;
    }
  }
}
